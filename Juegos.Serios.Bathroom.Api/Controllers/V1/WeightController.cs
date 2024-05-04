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
    using Juegos.Serios.Shared.Api.Controllers;   
    using Juegos.Serios.Shared.Application.Response;
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
        /// Valida el peso de un usuario basado en la informaci�n extra�da del token JWT.
        /// </summary>
        /// <returns>Una respuesta API que indica el resultado de la validaci�n del peso.</returns>
        /// <response code="200">Devuelve el c�digo 200 si la validaci�n del peso se complet� correctamente.</response>
        /// <response code="401">Devuelve el c�digo 401 si el token es inv�lido o faltan datos necesarios en el token.</response>
        /// <response code="400">Devuelve el c�digo 400 si los datos extra�dos del token no son v�lidos para realizar la validaci�n.</response>
        /// <response code="500">Devuelve el c�digo 500 en caso de un error interno del servidor.</response>
        [HttpGet("ValidateWeight")]
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
                return Unauthorized("Token inv�lido, ingrese el token a refrescar");
            }

            if (string.IsNullOrEmpty(createdUserClaim) || string.IsNullOrEmpty(userWeightClaim))
            {
                return Unauthorized("Datos faltantes en el token, verifique la informaci�n de usuario y peso.");
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
    }
}


