// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="PasswordRecoveryService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the Password Recovery service.</summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class PasswordRecoveryService : IPasswordRecoveryService<PasswordRecovery>
    {
        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly ILogger<PasswordRecoveryService> _logger;

        public PasswordRecoveryService(IPasswordRecoveryRepository passwordRecoveryRepository, ILogger<PasswordRecoveryService> logger)
        {
            _passwordRecoveryRepository = passwordRecoveryRepository ?? throw new ArgumentNullException(nameof(passwordRecoveryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PasswordRecovery> GetById(int id)
        {
            try
            {
                return await _passwordRecoveryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving PasswordRecovery by ID");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }

        public async Task<List<PasswordRecovery>> SelectAsync()
        {
            try
            {
                return (List<PasswordRecovery>)await DomainExceptionHandler.HandleAsync(() =>
                    _passwordRecoveryRepository.ListAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all PasswordRecovery");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}

