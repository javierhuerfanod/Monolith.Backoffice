// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="WeightService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the Password Recovery service.</summary>
// ***********************************************************************

using AutoMapper;
using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Constants;
using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Models.Weight;
using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;
using Juegos.Serios.Bathroom.Domain.Resources;
using Juegos.Serios.Bathroom.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Domain.Models;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Juegos.Serios.Bathroom.Domain.Services
{
    public sealed class WeightService : IWeightService<Weight>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQuestionnaireQuestionRepository _questionnaireQuestionRepository;
        private readonly IWeightRepository _weightRepository;
        private readonly ILogger<WeightService> _logger;

        public WeightService(IUnitOfWork unitOfWork, IMapper mapper, IQuestionnaireQuestionRepository questionnaireQuestionRepository, IWeightRepository weightRepository, ILogger<WeightService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _weightRepository = weightRepository ?? throw new ArgumentNullException(nameof(weightRepository));
            _questionnaireQuestionRepository = questionnaireQuestionRepository ?? throw new ArgumentNullException(nameof(questionnaireQuestionRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        public async Task<DomainRegisterWeightResponse> RegisterWeight(RegisterWeightModel registerWeightModel)
        {
            try
            {
                using (_unitOfWork)
                {
                    if (registerWeightModel.Weight == 0)
                    {
                        _logger.LogError("Weight validation failed: 'WeightCreatedInRegister' is zero.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidWeight);
                    }

                    if (registerWeightModel.UserId == 0)
                    {
                        _logger.LogError("Weight validation failed: 'UserId' is zero.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidUserId);
                    }
                    _logger.LogDebug("Fetching weights for UserId: {UserId}.", registerWeightModel.UserId);
                    var weights = await _weightRepository.ListAsync(WeightSpecifications.ByUserId(registerWeightModel.UserId));
                    if (weights.Count == 0)
                    {
                        _logger.LogError("No previous weight records found for UserId {UserId} on the creation of the first record.", registerWeightModel.UserId);
                        throw new Exception(AppMessages.Api_Weight_NoPreviousRecords);
                    }
                    var mostRecentWeight = weights.OrderByDescending(w => w.Date).FirstOrDefault();

                    if (mostRecentWeight != null && mostRecentWeight.Date == DateOnly.FromDateTime(DateTime.Now.Date))
                    {
                        _logger.LogInformation("Weight already measured today for UserId {UserId}.", registerWeightModel.UserId);
                        return new DomainRegisterWeightResponse
                        {
                            QuestionnaireID = 0,
                            WeightID = 0,
                            StatusCondition = (int)DomainEnumerator.WeightComparisonResult.AlreadyMeasuredToday,
                            Message = AppMessages.Domain_Weight_IsTakeToday,
                            questionareQuestionsDtos = new List<QuestionareQuestionDto>()
                        };
                    }

                    var createWeight = new Weight
                    {
                        UserId = registerWeightModel.UserId,
                        Date = DateOnly.FromDateTime(DateTime.Now.Date),
                        Weight1 = registerWeightModel.Weight,
                        QuestionnaireAnswers = new List<QuestionnaireAnswer>()
                    };
                    var createdWeight = await _weightRepository.AddAsync(createWeight);
                    _logger.LogInformation("New weight record created for UserId {UserId}.", registerWeightModel.UserId);
                    await _unitOfWork.Complete();

                    int weightDifference = (int)(createdWeight.Weight1 - mostRecentWeight.Weight1);
                    if (weightDifference == 1)
                    {
                        var questionare1 = await _questionnaireQuestionRepository.ListAsync(QuestionareQuestionSpecifications.ByQuestionareId((int)DomainEnumerator.DifferenceStatus.SuperiorByMoreThanOne));
                        return new DomainRegisterWeightResponse
                        {
                            QuestionnaireID = (int)DomainEnumerator.DifferenceStatus.SuperiorByMoreThanOne,
                            WeightID = createdWeight.WeightId,
                            StatusCondition = (int)DomainEnumerator.WeightComparisonResult.SuperiorByOne,
                            Message = AppMessages.Domain_Weight_IsSuperiorByOne,
                            questionareQuestionsDtos = _mapper.Map<List<QuestionareQuestionDto>>(questionare1)
                        };
                    }
                    else if (weightDifference >= 2)
                    {
                        var questionare2 = await _questionnaireQuestionRepository.ListAsync(QuestionareQuestionSpecifications.ByQuestionareId((int)DomainEnumerator.DifferenceStatus.SuperiorByMoreThanTwo));
                        return new DomainRegisterWeightResponse
                        {
                            QuestionnaireID = (int)DomainEnumerator.DifferenceStatus.SuperiorByMoreThanTwo,
                            WeightID = createdWeight.WeightId,
                            StatusCondition = (int)DomainEnumerator.WeightComparisonResult.SuperiorByTwo,
                            Message = AppMessages.Domain_Weight_IsSuperiorByTwo,
                            questionareQuestionsDtos = _mapper.Map<List<QuestionareQuestionDto>>(questionare2)
                        };
                    }
                    return new DomainRegisterWeightResponse
                    {
                        QuestionnaireID = 0,
                        WeightID = 0,
                        StatusCondition = (int)DomainEnumerator.WeightComparisonResult.Equal,
                        Message = AppMessages.Domain_Weight_IsEqual,
                        questionareQuestionsDtos = new List<QuestionareQuestionDto>()
                    };

                }
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error during weight validation for UserId {UserId}.", registerWeightModel.UserId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during weight validation for UserId {UserId}.", registerWeightModel.UserId);
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }

        public async Task<PaginatedList<WeightDto>> SearchWeights(string searchTerm, DateOnly? startDate, DateOnly? endDate, int pageNumber, int pageSize, int userId)
        {
            _logger.LogInformation("Starting weight search with searchTerm: {SearchTerm}, startDate: {StartDate}, endDate: {EndDate}, pageNumber: {PageNumber}, pageSize: {PageSize}", searchTerm, startDate, endDate, pageNumber, pageSize);

            try
            {

                if (userId <= 0)
                {
                    _logger.LogWarning("Invalid userId received: {PageNumber}.", pageNumber);
                    throw new DomainException(AppMessages.Api_Weight_InvalidUserId);
                }
                if (pageNumber <= 0)
                {
                    _logger.LogWarning("Invalid pageNumber received: {PageNumber}.", pageNumber);
                    throw new DomainException(AppMessages.Api_PageNumber_Invalid);
                }
                if (pageSize <= 0)
                {
                    _logger.LogWarning("Invalid pageSize received: {PageSize}.", pageSize);
                    throw new DomainException(AppMessages.Api_PageSize_Invalid);
                }

                Expression<Func<Weight, bool>> searchPredicate = WeightSpecifications.BySearchTermAndDateRange(searchTerm, startDate, endDate, userId);

                var (weights, totalRecords) = await _weightRepository.ListPaginatedWeightAsync(searchPredicate, pageNumber, pageSize);

                var weightDtos = _mapper.Map<List<WeightDto>>(weights);
                _logger.LogInformation("Successfully retrieved weight data. Total records found: {TotalRecords}", totalRecords);

                return new PaginatedList<WeightDto>(weightDtos, totalRecords, pageNumber, pageSize);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain exception occurred during weight search");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during searchWeights");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }


        public async Task<bool> ValidateWeight(ValidateWeightJwtModel validateWeightJwtModel)
        {
            try
            {
                using (_unitOfWork)
                {
                    if (!validateWeightJwtModel.WeightCreatedInRegister.HasValue)
                    {
                        _logger.LogError("Weight validation failed: 'WeightCreatedInRegister' is null.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidWeight);
                    }

                    if (validateWeightJwtModel.UserId == 0)
                    {
                        _logger.LogError("Weight validation failed: 'UserId' is zero.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidUserId);
                    }

                    if (validateWeightJwtModel.CreatedUser == default(DateTime))
                    {
                        _logger.LogError("Weight validation failed: CreatedUser date is not set.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidCreatedUser);
                    }

                    var weight = await _weightRepository.GetOneAsync(WeightSpecifications.ByUserId(validateWeightJwtModel.UserId));
                    if (weight == null)
                    {
                        _logger.LogInformation("No existing weight record found for UserId {UserId}. Creating a new weight record.", validateWeightJwtModel.UserId);
                        var createWeight = new Weight
                        {
                            UserId = validateWeightJwtModel.UserId,
                            Date = DateOnly.FromDateTime(validateWeightJwtModel.CreatedUser),
                            Weight1 = validateWeightJwtModel.WeightCreatedInRegister.Value,
                            QuestionnaireAnswers = new List<QuestionnaireAnswer>()
                        };
                        var createdWeight = await _weightRepository.AddAsync(createWeight);
                        _logger.LogInformation("New weight record created for UserId {UserId}.", validateWeightJwtModel.UserId);
                        await _unitOfWork.Complete();
                        if (createWeight.Date < DateOnly.FromDateTime(DateTime.Now.Date))
                        {
                            _logger.LogWarning("Created weight record date is in the past for UserId {UserId}.", validateWeightJwtModel.UserId);
                            return false;
                        }
                        return true;
                    }
                    var weights = await _weightRepository.ListAsync(WeightSpecifications.ByUserId(validateWeightJwtModel.UserId));
                    var mostRecentWeight = weights.OrderByDescending(w => w.Date).FirstOrDefault();
                    if (mostRecentWeight.Date < DateOnly.FromDateTime(DateTime.Now.Date))
                    {
                        _logger.LogWarning("Most recent weight record date is in the past for UserId {UserId}.", validateWeightJwtModel.UserId);
                        return false;
                    }
                    return true;
                }
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Domain error during weight validation.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception during weight validation.");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}

