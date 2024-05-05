// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Api
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="QuestionnaireAnswerController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Bathroom.Api.Controllers.V1
{
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Bathroom.Application.Features.QuestionnaireAnswer.Interfaces;
    using Juegos.Serios.Bathroom.Application.Models.Request;
    using Juegos.Serios.Bathroom.Domain.Resources;
    using Juegos.Serios.Shared.Api.Controllers;
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Net; 

    [ApiController]
    [Route("api/v1/[controller]")]
    public class QuestionnaireAnswerController : BaseApiController
    {
        private readonly IQuestionnaireAnswerApplication _questionnaireAnswerApplication;
        public QuestionnaireAnswerController(
            ILogger<QuestionnaireAnswerController> logger,
            IQuestionnaireAnswerApplication questionnaireAnswerApplication,
            IConfiguration configuration)
            : base(logger, configuration)
        {
            _questionnaireAnswerApplication = questionnaireAnswerApplication ?? throw new ArgumentNullException(nameof(questionnaireAnswerApplication));
        }
        /// <summary>
        /// Registra las respuestas de un cuestionario basadas en el modelo de solicitud proporcionado.
        /// </summary>
        /// <param name="registerQuestionnaireAnswerRequest">Datos de entrada que contienen las respuestas del cuestionario y la identificación del usuario.</param>
        /// <returns>Una respuesta API que indica el resultado de la operación de registro.</returns>
        /// <response code="200">Devuelve el código 200 si las respuestas del cuestionario fueron registradas correctamente.</response>
        /// <response code="400">Devuelve el código 400 si el modelo de solicitud no es válido o falta información necesaria para procesar la solicitud.</response>
        /// <response code="401">Devuelve el código 401 si el usuario no está autenticado o no tiene permisos suficientes para realizar esta acción.</response>
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor al intentar registrar las respuestas.</response>
        [HttpPost()]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> RegisterQuestionnaireAnswer(RegisterQuestionnaireAnswerRequest registerQuestionnaireAnswerRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.GetAllErrorMessages();
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                _logger.LogWarning("Password update failed due to invalid model state");
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }

            var response = await _questionnaireAnswerApplication.RegisterQuestionnaireAnswer(registerQuestionnaireAnswerRequest);
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


