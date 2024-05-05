// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Api
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="WeightController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Bathroom.Api.Controllers.V1
{
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Bathroom.Application.Features.Weight.Interfaces;
    using Juegos.Serios.Bathroom.Application.Models.Request;
    using Juegos.Serios.Bathroom.Application.Models.Response;
    using Juegos.Serios.Bathroom.Domain.Resources;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using System.Security.Claims;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeightController : BaseApiController
    {
        private readonly IWeightApplication _weightApplication;
        public WeightController(
            ILogger<WeightController> logger,
            IWeightApplication weightApplication,
            IConfiguration configuration)
            : base(logger, configuration)
        {
            _weightApplication = weightApplication ?? throw new ArgumentNullException(nameof(weightApplication));
        }
        /// <summary>
        /// Valida el peso de un usuario basado en la información extraída del token JWT.
        /// </summary>
        /// <returns>Una respuesta API que indica el resultado de la validación del peso.</returns>
        /// <response code="200">Devuelve el código 200 si la validación del peso se completó correctamente.</response>
        /// <response code="401">Devuelve el código 401 si el token es inválido o faltan datos necesarios en el token.</response>
        /// <response code="400">Devuelve el código 400 si los datos extraídos del token no son válidos para realizar la validación.</response>
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor.</response>
        [HttpGet("ValidateWeight")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> ValidateWeight()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdUserClaim = User.FindFirst("Created_user")?.Value;
            var userWeightClaim = User.FindFirst("user_weight")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Token inválido, ingrese el token a refrescar");
            }

            if (string.IsNullOrEmpty(createdUserClaim) || string.IsNullOrEmpty(userWeightClaim))
            {
                return Unauthorized("Datos faltantes en el token, verifique la información de usuario y peso.");
            }

            int userId = int.Parse(userIdClaim);
            DateTime createdUser = DateTime.Parse(createdUserClaim);
            int weightCreatedInRegister = int.Parse(userWeightClaim);

            var response = await _weightApplication.ValidateWeight(userId, weightCreatedInRegister, createdUser);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => LogAndReturnOk(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => LogAndReturnBadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => LogAndReturnInternalError(response),
                _ => throw new NotImplementedException()
            };
        }
        /// <summary>
        /// Registra y valida el peso del usuario basado en la información proporcionada.
        /// </summary>
        /// <param name="registerWeightRequest">Datos de solicitud para registrar el peso.</param>
        /// <returns>Una respuesta API que indica el resultado del registro y validación del peso, incluyendo la condición del peso comparado con registros anteriores.</returns>
        /// <response code="200">Devuelve el código 200 si el registro y la validación del peso se completan correctamente, junto con el estado del peso:
        ///  0: El peso ya se tomó ese día.
        ///  1: Los pesos son iguales o el peso nuevo es menor.
        ///  2: El peso nuevo es superior por 1.
        ///  3: El peso nuevo es superior por 2.
        /// </response>
        /// <response code="400">Devuelve el código 400 si los datos en la solicitud son inválidos o faltan datos en el token.</response>
        /// <response code="401">Devuelve el código 401 si el token es inválido o falta autorización.</response>
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor.</response>   
        [HttpPost()]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<RegisterWeightResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<RegisterWeightResponse>>> RegisterWeight(RegisterWeightRequest registerWeightRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var UserNameClaim = User.FindFirst("Created_userName")?.Value;
            var userLastNameClaim = User.FindFirst("Created_userLastName")?.Value;
            var userEmailClaim = User.FindFirst("Created_userEmail")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Token inválido, ingrese el token a refrescar");
            }

            if (string.IsNullOrEmpty(UserNameClaim) || string.IsNullOrEmpty(userLastNameClaim) || string.IsNullOrEmpty(userEmailClaim))
            {
                return Unauthorized("Datos faltantes en el token, verifique la información de usuario y peso.");
            }

            int userId = int.Parse(userIdClaim);
            string userName = UserNameClaim;
            string userLastname = userLastNameClaim;
            string userEmail = userEmailClaim;

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                _logger.LogWarning("User registration weight failed due to invalid model state. Errors: {ErrorMessages}", string.Join(", ", errorMessages));
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }

            _logger.LogInformation("Proceeding with weight registration for User ID: {UserId}", userId);
            var response = await _weightApplication.RegisterWeight(registerWeightRequest, userId, userName, userLastname, userEmail);

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


