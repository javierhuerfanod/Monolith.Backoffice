﻿// ***********************************************************************
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
using Juegos.Serios.Shared.AzureQueue.Models;
using Microsoft.Extensions.Logging;
using Juegos.Serios.Shared.AzureQueue.Interfaces;
using Juegos.Serios.Bathroom.Application.Resources.BathroomHtml;
using Juegos.Serios.Bathroom.Application.Constants;
using Microsoft.Extensions.Configuration;
using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;
using Juegos.Serios.Shared.Domain.Models;

namespace Juegos.Serios.Bathroom.Application.Features.WeightApplication
{
    public class WeightApplication : IWeightApplication
    {
        private readonly IWeightService<Domain.Aggregates.Weight> _weightService;
        private readonly ILogger<WeightApplication> _logger;
        private readonly IMapper _mapper;
        private readonly IAzureQueue _azureQueue;
        private readonly IConfiguration _configuration;

        public WeightApplication(ILogger<WeightApplication> logger, IWeightService<Domain.Aggregates.Weight> weightService, IMapper mapper, IAzureQueue azureQueue, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _weightService = weightService ?? throw new ArgumentNullException(nameof(weightService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _azureQueue = azureQueue ?? throw new ArgumentNullException(nameof(azureQueue));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

        public async Task<ApiResponse<RegisterWeightResponse>> RegisterWeight(RegisterWeightRequest registerWeightRequest, int userId, string name, string lastName, string email)
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
                string queueEmails = _configuration["QueueEmails"]!.ToString();

                switch (validationSuccess.StatusCondition)
                {
                    case (int)TypeEmailEnumerator.TypeEmail.Equal:
                        await _azureQueue.EnqueueMessageAsync(queueEmails, CreateEmailWeightEqualQueueAzure(name, lastName, email));
                        break;

                    case (int)TypeEmailEnumerator.TypeEmail.SuperiorByOne:
                        await _azureQueue.EnqueueMessageAsync(queueEmails, CreateEmailWeightSuperiorByOneQueueAzure(name, lastName, email));
                        break;

                    case (int)TypeEmailEnumerator.TypeEmail.SuperiorByTwo:
                        await _azureQueue.EnqueueMessageAsync(queueEmails, CreateEmailWeightSuperiorByTwoQueueAzure(name, lastName, email));
                        break;

                    default:
                        _logger.LogWarning("No email condition matched for the given weight status condition.");
                        break;
                }

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
        public async Task<ApiResponse<PaginatedList<WeightDto>>> SearchWeights(int userId, string searchTerm, string startDate, string endDate, int pageNumber, int pageSize)
        {
            _logger.LogInformation("Starting weight search with searchTerm: {SearchTerm}, startDate: {StartDate}, endDate: {EndDate}, pageNumber: {PageNumber}, pageSize: {PageSize}", searchTerm, startDate, endDate, pageNumber, pageSize);

            try
            {
                DateOnly start = string.IsNullOrWhiteSpace(startDate)
                     ? DateOnly.FromDateTime(DateTime.Now.AddMonths(-2))
                     : (DateOnly.TryParse(startDate, out DateOnly parsedStart) ? parsedStart : throw new DomainException(AppMessages.Api_InvalidDateFormat));

                DateOnly end = string.IsNullOrWhiteSpace(endDate)
                    ? DateOnly.FromDateTime(DateTime.Now)
                    : (DateOnly.TryParse(endDate, out DateOnly parsedEnd) ? parsedEnd : throw new DomainException(AppMessages.Api_InvalidDateFormat));


                if (start > end)
                {
                    _logger.LogWarning("StartDate: {StartDate} is greater than EndDate: {EndDate}.", startDate, endDate);
                    throw new DomainException(AppMessages.Api_StartDateGreaterThanEndDate);
                }

                if (start.Year != end.Year)
                {
                    _logger.LogWarning("StartDate: {StartDate} and EndDate: {EndDate} must be within the same year.", startDate, endDate);
                    throw new DomainException(AppMessages.Api_DateNotInSameYear);
                }

                var paginatedWeights = await _weightService.SearchWeights(searchTerm, start, end, pageNumber, pageSize, userId);
                _logger.LogInformation("Weight search completed successfully. Total records found: {TotalRecords}", paginatedWeights.TotalCount);

                return new ApiResponse<PaginatedList<WeightDto>>(200, AppMessages.Api_Weigths_paginated_successfully, true, paginatedWeights);
            }
            catch (DomainException dex)
            {
                _logger.LogError(dex, "Domain exception occurred during weight search");
                return new ApiResponse<PaginatedList<WeightDto>>(400, dex.Message, false, null);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during searchWeights with searchTerm: {SearchTerm}, startDate: {StartDate}, endDate: {EndDate}, pageNumber: {PageNumber}, pageSize: {PageSize}", searchTerm, startDate, endDate, pageNumber, pageSize);
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }



        private EmailsQueueAzure CreateEmailWeightEqualQueueAzure(string name, string lastName, string email)
        {
            _logger.LogInformation("Initiating CreateEmailWeightEqualQueueAzure for email: {Email}", email);

            try
            {
                var emailQueue = new EmailsQueueAzure
                {
                    Message = Utils.Utils.GenerateWeightEqualEmail(name, lastName),
                    Subject = AppBathroomMessages.Emails_WeightEqual_Subject,
                    TypeEmailId = (int)TypeEmailEnumerator.TypeEmail.Equal,
                    Recipients = [email],
                };

                _logger.LogInformation("Weight equal email queue created successfully for email: {Email}", email);

                return emailQueue;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during weight equal email creation for email: {Email}. Exception: {ExceptionMessage}", email, ex.Message);
                throw;
            }
        }

        private EmailsQueueAzure CreateEmailWeightSuperiorByOneQueueAzure(string name, string lastName, string email)
        {
            _logger.LogInformation("Initiating CreateEmailWeightSuperiorByOneQueueAzure for email: {Email}", email);

            try
            {
                var emailQueue = new EmailsQueueAzure
                {
                    Message = Utils.Utils.GenerateWeightSuperiorByOneEmail(name, lastName),
                    Subject = AppBathroomMessages.Emails_SuperiorByOne_Subject,
                    TypeEmailId = (int)TypeEmailEnumerator.TypeEmail.SuperiorByOne,
                    Recipients = [email],
                };

                _logger.LogInformation("Weight superior by one email queue created successfully for email: {Email}", email);

                return emailQueue;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during weight superior by one email creation for email: {Email}. Exception: {ExceptionMessage}", email, ex.Message);
                throw;
            }
        }

        private EmailsQueueAzure CreateEmailWeightSuperiorByTwoQueueAzure(string name, string lastName, string email)
        {
            _logger.LogInformation("Initiating CreateEmailWeightSuperiorByTwoQueueAzure for email: {Email}", email);

            try
            {
                var emailQueue = new EmailsQueueAzure
                {
                    Message = Utils.Utils.GenerateWeightSuperiorByTwoEmail(name, lastName),
                    Subject = AppBathroomMessages.Emails_SuperiorByTwo_Subject,
                    TypeEmailId = (int)TypeEmailEnumerator.TypeEmail.SuperiorByTwo,
                    Recipients = [email],
                };

                _logger.LogInformation("Weight superior by two email queue created successfully for email: {Email}", email);

                return emailQueue;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during weight superior by two email creation for email: {Email}. Exception: {ExceptionMessage}", email, ex.Message);
                throw;
            }
        }
    }
}

