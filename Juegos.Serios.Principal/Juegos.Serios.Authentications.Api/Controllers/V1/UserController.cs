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
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Microsoft.Extensions.Logging;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Api.UtilCross.Swagger;
    using Juegos.Serios.Authenticacions.Application.Models.Request;
    using Juegos.Serios.Domain.Shared.Exceptions;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IUserApplication _userApplication;
        private new readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, IUserApplication userApplication, IConfiguration configuration) : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userApplication = userApplication ?? throw new ArgumentNullException(nameof(userApplication));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userCreateRequest">Datos del usuario a registrar que incluyen nombre, correo electr�nico, contrase�a, etc.</param>
        /// <returns>Un resultado de acci�n que puede incluir varios tipos de respuestas HTTP basadas en el resultado del proceso de creaci�n.</returns>
        /// <remarks>
        /// Este m�todo valida la solicitud del cliente, verifica el token de autenticaci�n, intenta registrar un nuevo usuario usando los datos proporcionados y
        /// maneja la respuesta seg�n el resultado de la operaci�n. Los campos necesarios en el cuerpo de la solicitud incluyen:
        /// <list type="bullet">
        /// <item>
        /// <description>Nombre de usuario: Requerido y debe ser �nico.</description>
        /// </item>
        /// <item>
        /// <description>Correo electr�nico: Requerido y debe seguir un formato v�lido de correo electr�nico.</description>
        /// </item>
        /// <item>
        /// <description>Contrase�a: Requerida y debe cumplir con los siguientes criterios de seguridad:</description>
        /// <list type="bullet">
        /// <item>
        /// <description>Longitud m�nima de 5 caracteres.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un n�mero.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un car�cter especial (ej. !@#$%^&amp;*).</description>
        /// </item>
        /// </list>
        /// </item>
        /// </list>
        /// Adem�s, se espera que el token de aplicaci�n proporcionado en los encabezados HTTP concuerde con el configurado en el servidor. Si no es as�, se retornar� un error 401 (Unauthorized).
        /// POST: /api/users/register
        /// </remarks>
        [HttpPost()]
        [IncludeApplicationTokenHeader]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<ApiResponse<object>>> RegisterUser([FromBody] UserCreateRequest userCreateRequest)
        {

            var validApplicationToken = _configuration["ApplicationToken"]!.ToString();

            var incomingToken = Request.Headers["Application-Token"].FirstOrDefault();

            _logger.LogInformation("Attempting to register new user with token validation");

            if (incomingToken != validApplicationToken)
            {
                _logger.LogWarning("Unauthorized access attempt with invalid token");
                return Unauthorized(AppMessages.Api_TokenApplication_Invalid);
            }

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                _logger.LogWarning("User registration failed due to invalid model state");
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }

            var response = await _userApplication.CreateUser(userCreateRequest);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Actualiza la contrase�a de un usuario existente.
        /// </summary>
        /// <param name="updatePasswordRequest">Datos necesarios para actualizar la contrase�a, incluyendo el email del usuario y la nueva contrase�a.</param>
        /// <returns>Una respuesta que indica si la actualizaci�n fue exitosa o no.</returns>
        /// <remarks>
        /// Este m�todo valida la solicitud del cliente, verifica el token de autenticaci�n y, si es v�lido, intenta actualizar la contrase�a del usuario. La nueva contrase�a debe cumplir con los siguientes requisitos:
        /// <list type="bullet">
        /// <item>
        /// <description>Longitud m�nima de 5 caracteres.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un n�mero.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un car�cter especial (ej. !@#$%^&amp;*).</description>
        /// </item>
        /// </list>
        /// POST: /api/users/UpdatePassword
        /// </remarks>
        [HttpPost("UpdatePassword")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<ApiResponse<object>>> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                _logger.LogWarning("Password update failed due to invalid model state");
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Invalid token data");
                }
                int userId = int.Parse(userIdClaim);
                await _userApplication.UpdateUserPassword(updatePasswordRequest, userId);
                return Ok(new ApiResponse<object>(200, "Password updated successfully.", true, null));
            }
            catch (DomainException ex)
            {
                _logger.LogError("Domain exception occurred while updating password: {Message}", ex.Message);
                return BadRequest(new ApiResponse<ErrorResponse>(400, ex.Message, false, null));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while updating password: {Message}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<object>(500, "Internal server error.", false, null));
            }
        }
    }
}

