// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="UserAggregateService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the role service.</summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Aggregates.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.DocumentType.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text;


namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class UserAggregateService : IUserAggregateService<User>
    {
        private readonly IUserAggregateRepository _userAggregateRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly ILogger<UserAggregateService> _logger; 

        public UserAggregateService(IUserAggregateRepository userAggregateRepository, IRolRepository rolRepository, IDocumentTypeRepository documentTypeRepository, ILogger<UserAggregateService> logger)
        {
            _userAggregateRepository = userAggregateRepository ?? throw new ArgumentNullException(nameof(userAggregateRepository));
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _documentTypeRepository = documentTypeRepository ?? throw new ArgumentNullException(nameof(documentTypeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await _userAggregateRepository.GetOneAsync(UserAggregateSpecifications.ByEmail(email)) ?? throw new DomainException(AppMessages.Api_User_GetLogin_Invalid);
                if (user != null && VerifyPasswordHash(password, user.PasswordHash))
                {
                    _logger.LogInformation("User logged in successfully: {Email}", email);
                    return user;
                }
                else
                {
                    _logger.LogWarning("Invalid email or password: {Email}", email);
                    throw new DomainException(AppMessages.Api_User_GetLogin_Invalid);
                }
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Error during login");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                throw new DomainException("Unexpected error during login", ex);
            }
        }

        public async Task<User> RegisterUser(UserAggregateModel userAggregateModel)
        {
            try
            {
                if (await _rolRepository.GetByIdAsync(userAggregateModel.RoleId) == null)
                {
                    throw new DomainException(AppMessages.Api_Rol_GetById_NotFound);
                }

                if (await _documentTypeRepository.GetByIdAsync(userAggregateModel.DocumentTypeId) == null)
                {
                    throw new DomainException(AppMessages.Api_DocumentType_GetById_NotFound);
                }

                var existingUser = await _userAggregateRepository.GetManyAsync(
                    UserAggregateSpecifications.ByUsernameDocumentNumberOrEmail(
                        userAggregateModel.Username,
                        userAggregateModel.DocumentNumber,
                        userAggregateModel.Email)
                );

                if (existingUser.Any())
                {
                    _logger.LogWarning("User registration failed: user already exists");
                    throw new DomainException(AppMessages.Api_Get_User_Duplicated_Response);
                }

                User user = User.CreateNewUser(userAggregateModel);
                await _userAggregateRepository.AddAsync(user);
                _logger.LogInformation("User registered successfully: {Username}", user.Username);
                return user;
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Error during user registration");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during user registration");
                throw new DomainException("Unexpected error during user registration", ex);
            }
        }

        private static bool VerifyPasswordHash(string plainTextPassword, byte[] storedHash)
        {
            string hashAsString = Encoding.UTF8.GetString(storedHash);
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashAsString);
        }

    }
}

