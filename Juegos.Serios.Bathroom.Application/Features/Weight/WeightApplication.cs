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


using Juegos.Serios.Bathroom.Application.Features.Weight.Interfaces;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Bathroom.Domain.Resources;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Bathroom.Application.Features.WeightApplication
{
    public class WeightApplication : IWeightApplication
    {
        private readonly IWeightService<Domain.Aggregates.Weight> _weightService;
        private readonly ILogger<WeightApplication> _logger;

        public WeightApplication(ILogger<WeightApplication> logger, IWeightService<Domain.Aggregates.Weight> weightService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _weightService = weightService ?? throw new ArgumentNullException(nameof(weightService));

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
    }
}

