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

using AutoMapper;
using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Constants;
using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Models.QuestionnaireAnswer;
using Juegos.Serios.Bathroom.Domain.Models.Weight;
using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;
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
        private readonly IMapper _mapper;
        private readonly IQuestionnaireAnswerRepository _questionnaireAnswerRepository;
        private readonly IWeightRepository _weightRepository;
        private readonly ILogger<WeightService> _logger;

        public QuestionnaireAnswerService(IUnitOfWork unitOfWork, IMapper mapper, IQuestionnaireAnswerRepository questionnaireAnswerRepository, IWeightRepository weightRepository, ILogger<WeightService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _weightRepository = weightRepository ?? throw new ArgumentNullException(nameof(weightRepository));
            _questionnaireAnswerRepository = questionnaireAnswerRepository ?? throw new ArgumentNullException(nameof(questionnaireAnswerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> registerQuestionnaireAnswer(RegisterQuestionnaireAnswerModel registerQuestionnaireAnswerModel)
        {
            try
            {
                using (_unitOfWork)
                {
                    var weight = await _weightRepository.GetOneAsync(WeightSpecifications.ByWeightId(registerQuestionnaireAnswerModel.WeightID));
                    if (weight == null)
                    {
                        _logger.LogError("Weight validation failed: 'WeightCreatedInRegister' is null.");
                        throw new DomainException(AppMessages.Api_Weight_InvalidWeight);
                    }

                    return false;
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

