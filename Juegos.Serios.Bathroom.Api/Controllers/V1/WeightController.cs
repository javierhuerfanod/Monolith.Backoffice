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
    using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;
    using Juegos.Serios.Bathroom.Domain.Resources;
    using Juegos.Serios.Domain.Shared.Exceptions;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Application.Response;
    using Juegos.Serios.Shared.Domain.Models;
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
        /// Valida el peso de un usuario basado en la informaci�n extra�da del token JWT.
        /// </summary>
        /// <returns>Una respuesta API que indica el resultado de la validaci�n del peso.</returns>
        /// <response code="200">Devuelve el c�digo 200 si la validaci�n del peso se complet� correctamente.</response>
        /// <response code="401">Devuelve el c�digo 401 si el token es inv�lido o faltan datos necesarios en el token.</response>
        /// <response code="400">Devuelve el c�digo 400 si los datos extra�dos del token no son v�lidos para realizar la validaci�n.</response>
        /// <response code="500">Devuelve el c�digo 500 en caso de un error interno del servidor.</response>
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
        /// <summary>
        /// Registra y valida el peso del usuario basado en la informaci�n proporcionada.
        /// </summary>
        /// <param name="registerWeightRequest">Datos de solicitud para registrar el peso.</param>
        /// <returns>Una respuesta API que indica el resultado del registro y validaci�n del peso, incluyendo la condici�n del peso comparado con registros anteriores.</returns>
        /// <response code="200">Devuelve el c�digo 200 si el registro y la validaci�n del peso se completan correctamente, junto con el estado del peso:
        ///  0: El peso ya se tom� ese d�a.
        ///  1: Los pesos son iguales o el peso nuevo es menor.
        ///  2: El peso nuevo es superior por 1.
        ///  3: El peso nuevo es superior por 2.
        /// </response>
        /// <response code="400">Devuelve el c�digo 400 si los datos en la solicitud son inv�lidos o faltan datos en el token.</response>
        /// <response code="401">Devuelve el c�digo 401 si el token es inv�lido o falta autorizaci�n.</response>
        /// <response code="500">Devuelve el c�digo 500 en caso de un error interno del servidor.</response>   
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
                return Unauthorized("Token inv�lido, ingrese el token a refrescar");
            }

            if (string.IsNullOrEmpty(UserNameClaim) || string.IsNullOrEmpty(userLastNameClaim) || string.IsNullOrEmpty(userEmailClaim))
            {
                return Unauthorized("Datos faltantes en el token, verifique la informaci�n de usuario y peso.");
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
        /// <summary>
        /// Busca pesos dentro de un rango espec�fico y devuelve resultados paginados.
        /// </summary>
        /// <param name="searchTerm">El t�rmino de b�squeda para filtrar pesos por peso espec�fico.</param>
        /// <param name="startDate">La fecha de inicio del rango de b�squeda en formato AAAA-MM-DD.</param>
        /// <param name="endDate">La fecha de fin del rango de b�squeda en formato AAAA-MM-DD.</param>
        /// <param name="pageNumber">N�mero de p�gina para la paginaci�n de resultados.</param>
        /// <param name="pageSize">Cantidad de pesos por p�gina.</param>
        /// <returns>Una lista paginada de pesos que coinciden con los criterios de b�squeda.</returns>
        /// <response code="200">Devuelve una lista paginada de pesos.</response>
        /// <response code="400">Devuelve un error si la solicitud es inv�lida o las fechas no est�n en el formato correcto.</response>
        /// <response code="500">Devuelve un error si ocurre un problema interno en el servidor.</response>
        [HttpGet("SearchPaginatedWeights")]
        [ProducesResponseType(typeof(ApiResponse<PaginatedList<WeightDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<PaginatedList<WeightDto>>>> SearchPaginatedWeights(
            [FromQuery] string searchTerm,
            [FromQuery] string startDate,
            [FromQuery] string endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Initiating weight search with searchTerm: {SearchTerm}, startDate: {StartDate}, endDate: {EndDate}, pageNumber: {PageNumber}, pageSize: {PageSize}", searchTerm, startDate, endDate, pageNumber, pageSize);
            try
            {
                var result = await _weightApplication.SearchWeights(searchTerm, startDate, endDate, pageNumber, pageSize);
                _logger.LogInformation("Weight search completed successfully. Total records found: {TotalRecords}", result.Data.TotalCount);
                return Ok(new ApiResponse<PaginatedList<WeightDto>>(200, "Weights retrieved successfully", true, result.Data));
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Weight search failed: {Message}", dex.Message);
                return BadRequest(new ApiResponse<ErrorResponse>(400, dex.Message, false, null));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred during the weight search");
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<object>(500, "Internal server error", false, null));
            }
        }
    }
}


