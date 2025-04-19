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
    using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate.Dtos;
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Juegos.Serios.Domain.Shared.Exceptions;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Api.UtilCross.Swagger;    
    using Juegos.Serios.Shared.Application.Response;
    using Juegos.Serios.Shared.Domain.Models;
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
            // userCreateRequest.Password = EncryptionHelper.DecryptString(userCreateRequest.Password, _configuration["AppKeyEncrypt"]!.ToString());

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
            //updatePasswordRequest.NewPassword = EncryptionHelper.DecryptString(updatePasswordRequest.NewPassword, _configuration["AppKeyEncrypt"]!.ToString());
            var response = await _userApplication.UpdateUserPassword(updatePasswordRequest, userId);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Busca usuarios basándose en términos de búsqueda y devuelve resultados paginados.
        /// Este método filtra los usuarios según coincidencias en su nombre, apellido, correo electrónico o número de documento.
        /// Si no se proporciona un término de búsqueda, los usuarios se devolverán ordenados alfabéticamente por apellido y, en caso de coincidencia, por nombre.
        /// </summary>
        /// <param name="searchTerm">El término de búsqueda para filtrar usuarios. Dejar este parámetro vacío resultará en una lista de todos los usuarios, ordenados alfabéticamente por apellido y nombre.</param>
        /// <param name="pageNumber">Número de página para la paginación de resultados. El valor predeterminado es 1.</param>
        /// <param name="pageSize">Cantidad de usuarios por página. El valor predeterminado es 10.</param>
        /// <returns>Una lista paginada de usuarios que coinciden con los criterios de búsqueda o una lista completa de usuarios si no se proporciona un término de búsqueda.</returns>
        /// <response code="200">Devuelve una lista paginada de usuarios.</response>
        /// <response code="400">Devuelve un error si la solicitud es inválida.</response>
        /// <response code="500">Devuelve un error si ocurre un problema interno en el servidor.</response>
        [HttpGet("SearchPaginatedUsers")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<UserDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<PaginatedList<UserDto>>>> SearchPaginatedUsers([FromQuery] string searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Initiating user search with searchTerm: {SearchTerm}, pageNumber: {PageNumber}, pageSize: {PageSize}", searchTerm, pageNumber, pageSize);
            try
            {
                var result = await _userApplication.SearchUsers(searchTerm, pageNumber, pageSize);
                _logger.LogInformation("User search completed successfully. Total records found: {TotalRecords}", result.Data.TotalCount);
                return Ok(new ApiResponse<PaginatedList<UserDto>>(200, "Users retrieved successfully", true, result.Data));
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("User search failed: {Message}", dex.Message);
                return BadRequest(new ApiResponse<ErrorResponse>(400, dex.Message, false, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during the user search");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<object>(500, "Internal server error", false, null));
            }
        }

    }
}

