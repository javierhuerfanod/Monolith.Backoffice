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
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Models.Request;
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Api.UtilCross.Swagger;
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Security.Claims;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : BaseApiController
    {
        private readonly IUserApplication _userApplication;

        public UserController(
            ILogger<UserController> logger,
            IUserApplication userApplication,
            IConfiguration configuration)
            : base(logger, configuration)
        {
            _userApplication = userApplication ?? throw new ArgumentNullException(nameof(userApplication));
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userCreateRequest">Datos del usuario a registrar que incluyen nombre, correo electrónico, contraseña, etc.</param>
        /// <returns>Un resultado de acción que puede incluir varios tipos de respuestas HTTP basadas en el resultado del proceso de creación.</returns>
        /// <remarks>
        /// Este método valida la solicitud del cliente, verifica el token de autenticación, intenta registrar un nuevo usuario usando los datos proporcionados y
        /// maneja la respuesta según el resultado de la operación. Los campos necesarios en el cuerpo de la solicitud incluyen:
        /// <list type="bullet">
        /// <item>
        /// <description>Nombre de usuario: Requerido y debe ser único.</description>
        /// </item>
        /// <item>
        /// <description>Correo electrónico: Requerido y debe seguir un formato válido de correo electrónico.</description>
        /// </item>
        /// <item>
        /// <description>Contraseña: Requerida y debe cumplir con los siguientes criterios de seguridad:</description>
        /// <list type="bullet">
        /// <item>
        /// <description>Longitud mínima de 5 caracteres.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un número.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un carácter especial (ej. !@#$%^&amp;*).</description>
        /// </item>
        /// </list>
        /// </item>
        /// </list>
        /// Además, se espera que el token de aplicación proporcionado en los encabezados HTTP concuerde con el configurado en el servidor. Si no es así, se retornará un error 401 (Unauthorized).
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
        /// Actualiza la contraseña de un usuario existente.
        /// </summary>
        /// <param name="updatePasswordRequest">Datos necesarios para actualizar la contraseña, incluyendo el email del usuario y la nueva contraseña.</param>
        /// <returns>Una respuesta que indica si la actualización fue exitosa o no.</returns>
        /// <remarks>
        /// Este método valida la solicitud del cliente, verifica el token de autenticación y, si es válido, intenta actualizar la contraseña del usuario. La nueva contraseña debe cumplir con los siguientes requisitos:
        /// <list type="bullet">
        /// <item>
        /// <description>Longitud mínima de 5 caracteres.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un número.</description>
        /// </item>
        /// <item>
        /// <description>Debe contener al menos un carácter especial (ej. !@#$%^&amp;*).</description>
        /// </item>
        /// </list>
        /// PATCH: /api/users/UpdatePassword
        /// </remarks>
        [HttpPatch("UpdatePassword")]
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Invalid token data");
            }
            int userId = int.Parse(userIdClaim);
            var response = await _userApplication.UpdateUserPassword(updatePasswordRequest, userId);
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

