// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="QuestionnaireAnswerApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Microsoft.Extensions.Logging;
using Juegos.Serios.Bathroom.Application.Features.QuestionnaireAnswer.Interfaces;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Resources;
using Juegos.Serios.Bathroom.Application.Models.Request;
using Juegos.Serios.Bathroom.Domain.Models.QuestionnaireAnswer;
using AutoMapper;


namespace Juegos.Serios.Bathroom.Application.Features.QuestionnaireAnswer
{
    public class QuestionnaireAnswerApplication : IQuestionnaireAnswerApplication
    {
        private readonly IQuestionnaireAnswerService<Domain.Entities.QuestionnaireAnswer> _questionnaireAnswerService;
        private readonly ILogger<QuestionnaireAnswerApplication> _logger;
        private readonly IMapper _mapper;

        public QuestionnaireAnswerApplication(IQuestionnaireAnswerService<Domain.Entities.QuestionnaireAnswer> questionnaireAnswerService, ILogger<QuestionnaireAnswerApplication> logger, IMapper mapper)
        {
            _questionnaireAnswerService = questionnaireAnswerService ?? throw new ArgumentNullException(nameof(questionnaireAnswerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<object>> RegisterQuestionnaireAnswer(RegisterQuestionnaireAnswerRequest registerQuestionnaireAnswerRequest)
        {
            _logger.LogInformation("Initiating questionnaire answer registration.");

            try
            {               
                var questionnaireAnswerModel = _mapper.Map<RegisterQuestionnaireAnswerModel>(registerQuestionnaireAnswerRequest);
                await _questionnaireAnswerService.RegisterQuestionnaireAnswer(questionnaireAnswerModel);
                _logger.LogInformation("Questionnaire answer registration successful.");

                return new ApiResponse<object>(200, AppMessages.Api_Get_RegisterQuestionnaireAnswer_Created_Response, true, null);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Questionnaire answer registration failed. Reason: {ExceptionMessage}", dex.Message);
                return new ApiResponse<object>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during questionnaire answer registration. Exception: {ExceptionMessage}", ex.Message);
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }
    }
}

