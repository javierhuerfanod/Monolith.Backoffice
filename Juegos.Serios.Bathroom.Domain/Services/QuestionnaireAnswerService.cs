// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="QuestionnaireAnswerService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the Password Recovery service.</summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Models.QuestionnaireAnswer;
using Juegos.Serios.Bathroom.Domain.Resources;
using Juegos.Serios.Bathroom.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Bathroom.Domain.Services
{
    public sealed class QuestionnaireAnswerService : IQuestionnaireAnswerService<QuestionnaireAnswer>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuestionnaireAnswerRepository _questionnaireAnswerRepository;
        private readonly IQuestionnaireQuestionRepository _questionnaireQuestionRepository;
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly IWeightRepository _weightRepository;
        private readonly ILogger<QuestionnaireAnswerService> _logger;

        public QuestionnaireAnswerService(IUnitOfWork unitOfWork, IQuestionnaireAnswerRepository questionnaireAnswerRepository, IQuestionnaireQuestionRepository questionnaireQuestionRepository, IQuestionnaireRepository questionnaireRepository, IWeightRepository weightRepository, ILogger<QuestionnaireAnswerService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _weightRepository = weightRepository ?? throw new ArgumentNullException(nameof(weightRepository));
            _questionnaireAnswerRepository = questionnaireAnswerRepository ?? throw new ArgumentNullException(nameof(questionnaireAnswerRepository));
            _questionnaireQuestionRepository = questionnaireQuestionRepository ?? throw new ArgumentNullException(nameof(questionnaireQuestionRepository));
            _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> RegisterQuestionnaireAnswer(RegisterQuestionnaireAnswerModel registerQuestionnaireAnswerModel)
        {
            try
            {
                using (_unitOfWork)
                {
                    if (registerQuestionnaireAnswerModel == null || registerQuestionnaireAnswerModel.questionareQuestionModels == null)
                    {
                        _logger.LogError("Registration validation failed: Questionnaire or its questions are null.");
                        throw new DomainException(AppMessages.Api_Questionnaire_InvalidData);
                    }

                    var weight = await _weightRepository.GetOneAsync(WeightSpecifications.ByWeightId(registerQuestionnaireAnswerModel.WeightID));
                    if (weight == null)
                    {
                        _logger.LogError("Weight validation failed: No weight found with provided WeightID.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidWeight_NotFound);
                    }
                    var questionnaireAnswer = await _questionnaireAnswerRepository.GetOneAsync(QuestionnaireAnswerSpecifications.ByWeightId(weight.WeightId));
                    if (questionnaireAnswer != null)
                    {
                        _logger.LogError("Questionnaire validation failed: An answer already exists for the provided WeightID {WeightId}.", weight.WeightId);
                        throw new DomainException(AppMessages.Api_Questionnaire_AnswerAlreadyExists);
                    }
                    var questionsQuestionare = await _questionnaireRepository.GetOneAsync(QuestionareSpecifications.ByQuestionareId(registerQuestionnaireAnswerModel.QuestionnaireID));
                    if (questionsQuestionare == null)
                    {
                        _logger.LogError("Questionnaire validation failed: No questionnaire found with provided QuestionnaireID.");
                        throw new DomainException(AppMessages.Api_Questionnaire_InvalidQuestionnaireID);
                    }

                    var inputIds = registerQuestionnaireAnswerModel.questionareQuestionModels.Select(q => q.QuestionId).ToList();
                    var inputIdsSet = new HashSet<int>(inputIds);
                    if (inputIdsSet.Count != inputIds.Count)
                    {
                        _logger.LogError("Duplicate question IDs found in the input.");
                        throw new DomainException(AppMessages.Api_Questionnaire_MismatchQuestionCountDuplicated);
                    }
                    var questionnaireQuestions = await _questionnaireQuestionRepository.ListAsync(QuestionareQuestionSpecifications.ByQuestionareId(questionsQuestionare.QuestionnaireId));
                    var questionnaireQuestionsIds = questionnaireQuestions.Select(q => q.QuestionId).ToList();

                    if (inputIds.Count != questionnaireQuestionsIds.Count)
                    {
                        _logger.LogError("Mismatch in question count: {InputCount} provided, {RepositoryCount} expected.", inputIds.Count, questionnaireQuestionsIds.Count);
                        throw new DomainException(AppMessages.Api_Questionnaire_MismatchQuestionCount);
                    }

                    if (!inputIds.All(id => questionnaireQuestionsIds.Contains(id)))
                    {
                        _logger.LogError("Mismatch in question identifiers: Some provided question IDs are not valid.");
                        throw new DomainException(AppMessages.Api_Questionnaire_InvalidQuestionIDs);
                    }
                    foreach (var questionModel in registerQuestionnaireAnswerModel.questionareQuestionModels)
                    {
                        var answer = new QuestionnaireAnswer
                        {
                            WeightId = registerQuestionnaireAnswerModel.WeightID,
                            QuestionId = questionModel.QuestionId,
                            Answer = questionModel.Answer
                        };

                        await _questionnaireAnswerRepository.AddAsync(answer);
                    }
                    await _unitOfWork.Complete();
                    return true;
                }
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error during questionnaire validation.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during questionnaire validation.");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
        public async Task<List<QuestionnaireAnswer>> GetQuestionnaireAnswerByWeight(int userId, int weightId)
        {
            try
            {   
                if (userId <= 0)
                {
                    _logger.LogWarning("Invalid userId received: {UserId}.", userId);
                    throw new DomainException(AppMessages.Api_Weight_InvalidUserId);
                }
         
                if (weightId <= 0)
                {
                    _logger.LogWarning("Invalid weightId received: {WeightId}.", weightId);
                    throw new DomainException(AppMessages.Api_Weight_InvalidWeight);
                }
                
                var weight = await _weightRepository.GetOneAsync(WeightSpecifications.ByWeightIdAndUserId(weightId, userId));
                if (weight == null)
                {
                    _logger.LogWarning("No weight found for given weightId {WeightId} and userId {UserId}.", weightId, userId);
                    throw new DomainException(AppMessages.Api_Weight_InvalidWeight_null);
                }           
                var answers = await _questionnaireAnswerRepository.GetAnswersByWeightIdWithDetails(weightId);
                _logger.LogInformation("Retrieved {AnswerCount} answers for weightId {WeightId}.", answers.Count, weightId);
                return answers.ToList();
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error during questionnaire answers retrieval for userId {UserId} and weightId {WeightId}.", userId, weightId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during questionnaire answers retrieval for userId {UserId} and weightId {WeightId}.", userId, weightId);
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}

