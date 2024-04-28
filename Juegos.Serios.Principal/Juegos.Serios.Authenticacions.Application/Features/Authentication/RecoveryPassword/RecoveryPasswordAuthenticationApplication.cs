// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RecoveryPasswordAuthenticationApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Domain.Aggregates.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
using Juegos.Serios.Shared.AzureQueue.Models;
using Juegos.Serios.Authenticacions.Domain.Models.RecoveryPassword.Response;
using Juegos.Serios.Authentications.Application.Utils;
using Juegos.Serios.Authenticacions.Application.Constants;
using Juegos.Serios.Shared.AzureQueue.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Juegos.Serios.Authenticacions.Application.Resources.EmailsHtml;

namespace Juegos.Serios.Authenticacions.Application.Features.Authentication.RecoveryPassword
{
    public class RecoveryPasswordAuthenticationApplication : IRecoveryPasswordAuthenticationApplication
    {
        private readonly IUserAggregateService<User> _userAggregateService;
        private readonly IAzureQueue _azureQueue;
        private readonly ILogger<RecoveryPasswordAuthenticationApplication> _logger;
        private readonly IConfiguration _configuration;

        public RecoveryPasswordAuthenticationApplication(ILogger<RecoveryPasswordAuthenticationApplication> logger, IUserAggregateService<User> userAggregateService, IAzureQueue azureQueue, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAggregateService = userAggregateService ?? throw new ArgumentNullException(nameof(userAggregateService));
            _azureQueue = azureQueue ?? throw new ArgumentNullException(nameof(azureQueue));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ApiResponse<object>> CreateRecoveryPassword(string email)
        {
            _logger.LogInformation("Initiating password recovery for email: {Email}", email);
            try
            {
                var passwordRecovery = await _userAggregateService.RegisterRecoveryPassword(email);               
                await _azureQueue.EnqueueMessageAsync(_configuration["QueueEmails"]!.ToString(), CreateEmailRecoveryPasswordQueueAzure(passwordRecovery, email));
                _logger.LogInformation("Password recovery request successful for email: {Email}", email);
                return new ApiResponse<object>(200, AppMessages.Api_Get_RecoveryPassword_Created_Response, true, null);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Password recovery request failed for email: {Email}. Reason: {ExceptionMessage}", email, dex.Message);
                return new ApiResponse<object>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during password recovery for email: {Email}. Exception: {ExceptionMessage}", email, ex.Message);
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }
        private EmailsQueueAzure CreateEmailRecoveryPasswordQueueAzure(PasswordRecoveryResponse passwordRecoveryResponse, string email)
        {
            _logger.LogInformation("Initiating CreateEmailRecoveryPasswordQueueAzure password recovery for email: {Email}", email);

            try
            {
                var emailQueue = new EmailsQueueAzure
                {
                    Message = Utils.GeneratePasswordRecoveryEmail(passwordRecoveryResponse.Name, passwordRecoveryResponse.LastName, passwordRecoveryResponse.Password),
                    Subject = AppEmailsMessages.Emails_RecoveryPassword_Subject,
                    TypeEmailId = (int)TypeEmailEnumerator.TypeEmail.RecoveryPassword,
                    Recipients = [email],
                };

                _logger.LogInformation("Password recovery email queue created successfully for email: {Email}", email);

                return emailQueue;
            }
            catch (Exception ex)
            {
                _logger.LogError("An unexpected error occurred during password recovery for email: {Email}. Exception: {ExceptionMessage}", email, ex.Message);
                throw;
            }
        }
    }
}

