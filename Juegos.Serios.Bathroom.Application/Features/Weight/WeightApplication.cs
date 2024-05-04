// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="WeightApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using AutoMapper;
using Juegos.Serios.Bathroom.Application.Features.Weight.Interfaces;
using Juegos.Serios.Bathroom.Application.Models.Request;
using Juegos.Serios.Bathroom.Application.Models.Response;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Models.Weight;
using Juegos.Serios.Bathroom.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Bathroom.Application.Features.WeightApplication
{
    public class WeightApplication : IWeightApplication
    {
        private readonly IWeightService<Domain.Aggregates.Weight> _weightService;
        private readonly ILogger<WeightApplication> _logger;
        private readonly IMapper _mapper;

        public WeightApplication(ILogger<WeightApplication> logger, IWeightService<Domain.Aggregates.Weight> weightService, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _weightService = weightService ?? throw new ArgumentNullException(nameof(weightService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApiResponse<object>> ValidateWeight(int userId, int weightCreatedInRegister, DateTime createdUser)
        {
            _logger.LogInformation("Initiating weight validation for User ID: {UserId}", userId);
            try
            {
                var validationSuccess = await _weightService.ValidateWeight(new Domain.Models.Weight.ValidateWeightJwtModel
                {
                    CreatedUser = createdUser,
                    UserId = userId,
                    WeightCreatedInRegister = weightCreatedInRegister
                });

                if (validationSuccess)
                {
                    _logger.LogInformation("Weight validation successful for User ID: {UserId}", userId);
                    return new ApiResponse<object>(200, AppMessages.Api_Get_Weight_ValidatedWeight_Response_success, true, null);
                }
                else
                {
                    _logger.LogWarning("Weight validation found issues for User ID: {UserId}", userId);
                    return new ApiResponse<object>(200, AppMessages.Api_Get_Weight_ValidatedWeight_Response_failed, false, null);
                }
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Weight validation request failed for User ID: {UserId}. Reason: {ExceptionMessage}", userId, dex.Message);
                return new ApiResponse<object>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during weight validation for User ID: {UserId}. Exception: {ExceptionMessage}", userId, ex.Message);
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }

        public async Task<ApiResponse<RegisterWeightResponse>> RegisterWeight(RegisterWeightRequest registerWeightRequest, int userId)
        {
            _logger.LogInformation("Starting registration process for weight validation. User ID: {UserId}", userId);

            try
            {
                var registerWeightModel = _mapper.Map<RegisterWeightModel>(registerWeightRequest, opts =>
                {
                    opts.Items["userId"] = userId;
                });
                _logger.LogDebug("Mapped RegisterWeightRequest to RegisterWeightModel. Starting service call for User ID: {UserId}.", userId);

                var validationSuccess = await _weightService.RegisterWeight(registerWeightModel);

                _logger.LogDebug("Service call completed. Mapping response for User ID: {UserId}.", userId);
                var questionareQuestionResponses = _mapper.Map<RegisterWeightResponse>(validationSuccess);              
                _logger.LogInformation("Weight registration completed successfully for User ID: {UserId}.", userId);
                return new ApiResponse<RegisterWeightResponse>(200, validationSuccess.Message, true, questionareQuestionResponses);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Domain exception during weight validation for User ID: {UserId}. Reason: {ExceptionMessage}", userId, dex.Message);
                return new ApiResponse<RegisterWeightResponse>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error during the weight validation process for User ID: {UserId}. Exception details: {ExceptionMessage}", userId, ex.Message);
                return new ApiResponse<RegisterWeightResponse>(500, AppMessages.Api_Servererror, false, null);
            }
        }

    }
}

