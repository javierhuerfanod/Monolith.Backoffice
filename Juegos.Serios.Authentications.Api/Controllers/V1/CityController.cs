// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Api
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="CityController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Authenticacions.Api.V1
{
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Authenticacions.Application.Features.CityApplication.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Models.Response;
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Api.UtilCross.Swagger;
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Net;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class CityController : BaseApiController
    {
        private readonly ICityApplication _cityApplication;
        public CityController(
            ILogger<CityController> logger,
            ICityApplication cityApplication,
            IConfiguration configuration)
            : base(logger, configuration)
        {
            _cityApplication = cityApplication ?? throw new ArgumentNullException(nameof(cityApplication));
        }


        /// <summary>
        /// Obtiene la lista de todas las ciudades disponibles.
        /// </summary>
        /// <returns>Una respuesta con la lista de ciudades si hay datos disponibles, o un mensaje de error si no hay datos.</returns>
        /// <response code="200">Si se encuentran ciudades, devuelve el código 200 junto con los detalles de las ciudades.</response>
        /// <response code="400">Si la solicitud es inválida, por ejemplo, si el token de aplicación no es válido, devuelve el código 400.</response>
        /// <response code="404">Si no se encuentra ninguna ciudad, devuelve el código 404.</response>
        /// <response code="500">Si ocurre un error interno en el servidor mientras se procesa la solicitud, devuelve el código 500.</response>
        [HttpGet("Cities")]
        [IncludeApplicationTokenHeader]
        [ProducesResponseType(typeof(ApiResponse<List<CityResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<List<CityResponse>>>> GetCities()
        {
            if (!ValidateTokenApplication())
            {
                return Unauthorized(new ApiResponse<object>(401, AppMessages.Api_TokenApplication_Invalid, false));
            }

            _logger.LogInformation("Validating token for city list retrieval");
            var response = await _cityApplication.SelectAsync();
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            if (response.Data.Count == 0)
            {
                _logger.LogWarning("No cities found during retrieval");
                return NotFound(AppMessages.Api_City_GetCities_NotFound);
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


