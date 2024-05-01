// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Api
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="AuthenticationController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Authenticacions.Api.V1
{
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Models.Request;
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Microsoft.Extensions.Logging;
    using Juegos.Serios.Shared.Api.Controllers;
    using Microsoft.Extensions.Configuration;
    using Juegos.Serios.Shared.Api.UtilCross.Swagger;
    using System.Security.Claims;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : BaseApiController
    {
        private readonly ILoginApplication _loginApplication;
        public readonly IRecoveryPasswordAuthenticationApplication _recoveryPasswordAuthenticationApplication;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            ILoginApplication loginApplication,
            IRecoveryPasswordAuthenticationApplication recoveryPasswordAuthenticationApplication,
            IConfiguration configuration)
            : base(logger, configuration)
        {
            _loginApplication = loginApplication ?? throw new ArgumentNullException(nameof(loginApplication));
            _recoveryPasswordAuthenticationApplication = recoveryPasswordAuthenticationApplication ?? throw new ArgumentNullException(nameof(recoveryPasswordAuthenticationApplication));
        }

        /// <summary>
        /// Realiza el inicio de sesión basado en las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginRequest">Los datos de autenticación que incluyen el correo electrónico y la contraseña.</param>
        /// <returns>Una respuesta que contiene los detalles del rol del usuario si el inicio de sesión es exitoso.</returns>
        /// <response code="200">Devuelve el código 200 junto con los detalles del rol del usuario si el inicio de sesión es exitoso.</response>
        /// <response code="400">Devuelve el código 400 si los datos de la solicitud no son válidos o el token de la aplicación no es válido.</response>
        /// <response code="401">Devuelve el código 401 si no se proporciona un token válido en la solicitud.</response>
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor.</response>
        [HttpPost("login")]
        [IncludeApplicationTokenHeader]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ValidateTokenApplication())
            {
                return Unauthorized(new ApiResponse<object>(401, AppMessages.Api_TokenApplication_Invalid, false));
            }

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                _logger.LogWarning("Bad request on login: {ErrorMessages}", errorMessages);
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }

            _logger.LogInformation("Attempting login for user: {Email}", loginRequest.Email);
            var response = await _loginApplication.GetLogin(loginRequest);

            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Inicia un proceso de recuperación de contraseña para un usuario.
        /// </summary>
        /// <param name="recoveryPasswordRequest">Datos que contienen la dirección de correo electrónico del usuario para la recuperación de contraseña.</param>
        /// <returns>Una respuesta API que indica el resultado de la solicitud de recuperación de contraseña.</returns>
        /// <response code="200">Devuelve el código 200 si la solicitud de recuperación de contraseña se realizó correctamente.</response>
        /// <response code="400">Devuelve el código 400 si se proporcionaron datos de solicitud no válidos. Consulte el mensaje de error para obtener más detalles.</response>
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor.</response>
        [HttpPost("RecoveryPassword")]
        [IncludeApplicationTokenHeader]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> RecoveryPassword([FromBody] RecoveryPasswordRequest recoveryPasswordRequest)
        {
            if (!ValidateTokenApplication())
            {
                return Unauthorized(new ApiResponse<object>(401, AppMessages.Api_TokenApplication_Invalid, false));
            }
            _logger.LogInformation("Attempting to register new recovery password");

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }
            var response = await _recoveryPasswordAuthenticationApplication.CreateRecoveryPassword(recoveryPasswordRequest.Email);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Refresca el token JWT de un usuario basado en el token de acceso actual.
        /// </summary>
        /// <returns>Una respuesta API que indica el resultado del refresco del token.</returns>
        /// <response code="200">Devuelve el código 200 junto con el nuevo token si el refresco se realizó correctamente.</response>
        /// <response code="401">Devuelve el código 401 si los datos del token son inválidos o si el token ha expirado.</response>
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor.</response>
        [HttpPost("RefreshToken")]
        [IncludeApplicationTokenHeader]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<string>>> RefreshToken()
        {
            _logger.LogInformation("Attempting to get new toke by jwt access token");
            if (!ValidateTokenApplication())
            {
                return Unauthorized(new ApiResponse<object>(401, AppMessages.Api_TokenApplication_Invalid, false));
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Token invalido, ingrese el token a refrescar");
            }
            int userId = int.Parse(userIdClaim);
            var response = await _loginApplication.GetRefreshToken(userId);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }
    }
}

