// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Api
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserAvatarBodyPartController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Authenticacions.Api.V1
{
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Models.Request;
    using Juegos.Serios.Authenticacions.Application.Models.Response;
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Security.Claims;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserAvatarBodyPartController : BaseApiController
    {
        private readonly IUserAvatarBodyPartsApplication _userAvatarBodyPartsApplication;
        private new readonly ILogger<UserAvatarBodyPartController> _logger;

        public UserAvatarBodyPartController(ILogger<UserAvatarBodyPartController> logger, IUserAvatarBodyPartsApplication userAvatarBodyPartsApplication) : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAvatarBodyPartsApplication = userAvatarBodyPartsApplication ?? throw new ArgumentNullException(nameof(userAvatarBodyPartsApplication));
        }

        /// <summary>
        /// Crea partes del cuerpo del avatar para un usuario autenticado, asegurándose de que incluyan todas las partes definidas y sin duplicados.
        /// </summary>
        /// <remarks>
        /// Las partes del cuerpo que se pueden registrar son las definidas en el enum BodyPartName: Body, Hair, Torso, Legs.
        /// Es necesario incluir todas estas partes sin que ninguna esté repetida.
        /// </remarks>
        /// <param name="userBodyPartsRequest">Datos que contienen las partes del cuerpo del avatar a registrar.</param>
        /// <returns>Una respuesta que indica el resultado del intento de creación de partes del cuerpo del avatar.</returns>
        /// <response code="200">Si las partes del cuerpo del avatar se crean correctamente, devuelve el código 200 junto con los detalles del avatar.</response>
        /// <response code="400">Si la solicitud es inválida, por ejemplo, si los datos del modelo son incorrectos, devuelve el código 400 junto con un mensaje de error.</response>
        /// <response code="401">Si el usuario no está autorizado o el token es inválido, devuelve el código 401.</response>
        /// <response code="500">Si ocurre un error interno en el servidor mientras se procesa la solicitud, devuelve el código 500.</response>
        [Authorize]
        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> CreateUserAvatarBodyParts(UserBodyPartsRequest userBodyPartsRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                _logger.LogWarning("User avatar create failed due to invalid model state");
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Invalid token data");
            }
            int userId = int.Parse(userIdClaim);
            var response = await _userAvatarBodyPartsApplication.CreateUserAvatarBodyParts(userBodyPartsRequest, userId);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Obtiene la lista de partes del cuerpo del avatar asociadas con el usuario autenticado.
        /// </summary> 
        /// <returns>Una respuesta con la lista de partes del cuerpo del avatar si hay datos disponibles, o un mensaje de error si no se encuentran datos.</returns>
        /// <response code="200">Si se encuentran partes del cuerpo del avatar, devuelve el código 200 junto con los detalles de las partes del cuerpo.</response>
        /// <response code="401">Si el token de autenticación es inválido o falta, devuelve el código 401.</response>
        /// <response code="404">Si no se encuentran partes del cuerpo del avatar para el usuario, devuelve el código 404.</response>
        /// <response code="500">Si ocurre un error interno en el servidor mientras se procesa la solicitud, devuelve el código 500.</response>
        [Authorize]
        [HttpGet("GetUserAvatarBodyParts")]
        [ProducesResponseType(typeof(ApiResponse<List<UserBodyPartsResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<UserBodyPartsResponse>>>> GetUserAvatarBodyParts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Invalid token data");
            }
            int userId = int.Parse(userIdClaim);
            _logger.LogInformation("Validating token for avatar body parts list retrieval");
            var response = await _userAvatarBodyPartsApplication.SelectByUserId(userId);
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            if (response.Data == null)
            {
                _logger.LogWarning("No avatar body parts found for user ID: {UserId}", userId);
                return NotFound(AppMessages.Api_UserAvatarBodyParts_GetByUserId_NotFound);
            }
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => Ok(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => BadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                _ => throw new NotImplementedException()
            };
        }

    }
}


