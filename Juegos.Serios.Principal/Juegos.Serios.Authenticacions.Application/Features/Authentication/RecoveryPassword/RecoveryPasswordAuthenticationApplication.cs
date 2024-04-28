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

namespace Juegos.Serios.Authenticacions.Application.Features.Authentication.RecoveryPassword
{
    public class RecoveryPasswordAuthenticationApplication : IRecoveryPasswordAuthenticationApplication
    {
        private readonly IUserAggregateService<User> _userAggregateService;   
        private readonly ILogger<RecoveryPasswordAuthenticationApplication> _logger;

        public RecoveryPasswordAuthenticationApplication(ILogger<RecoveryPasswordAuthenticationApplication> logger, IUserAggregateService<User> userAggregateService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAggregateService = userAggregateService ?? throw new ArgumentNullException(nameof(userAggregateService));           
        }

        public async Task<ApiResponse<object>> CreateRecoveryPassword(string email)
        {
            _logger.LogInformation("Initiating password recovery for email: {Email}", email);

            try
            {
                var passwordRecovery = await _userAggregateService.RegisterRecoveryPassword(email);
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

    }
}

