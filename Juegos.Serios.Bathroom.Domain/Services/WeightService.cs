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

using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Models.Weight;
using Juegos.Serios.Bathroom.Domain.Resources;
using Juegos.Serios.Bathroom.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Bathroom.Domain.Services
{
    public sealed class WeightService : IWeightService<Weight>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWeightRepository _weightRepository;
        private readonly ILogger<WeightService> _logger;

        public WeightService(IUnitOfWork unitOfWork, IWeightRepository weightRepository, ILogger<WeightService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _weightRepository = weightRepository ?? throw new ArgumentNullException(nameof(weightRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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

