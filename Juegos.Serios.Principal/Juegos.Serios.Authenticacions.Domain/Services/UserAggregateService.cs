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
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Models.RecoveryPassword;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Authenticacions.Domain.Ports.Persistence;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text;


namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class UserAggregateService : IUserAggregateService<User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAggregateRepository _userAggregateRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly ILogger<UserAggregateService> _logger;

        public UserAggregateService(IUnitOfWork unitOfWork, IUserAggregateRepository userAggregateRepository, IPasswordRecoveryRepository passwordRecoveryRepository, IRolRepository rolRepository, IDocumentTypeRepository documentTypeRepository, ILogger<UserAggregateService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userAggregateRepository = userAggregateRepository ?? throw new ArgumentNullException(nameof(userAggregateRepository));
            _passwordRecoveryRepository = passwordRecoveryRepository ?? throw new ArgumentNullException(nameof(passwordRecoveryRepository));
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
                    if (user.IsTemporaryPassword == true)
                    {
                        user.IsTemporaryPassword = false;
                        var existingRecoveryPassword = await _passwordRecoveryRepository.GetOneAsync(PasswordRecoverSpecifications.ByUserId(user.UserId));
                        if (existingRecoveryPassword != null)
                        {
                            await _passwordRecoveryRepository.DeleteAsync(existingRecoveryPassword);
                            _logger.LogInformation("User logged in successfully: {Email}", email);
                        }
                        await _userAggregateRepository.UpdateAsync(user);
                    }
                    _logger.LogInformation("User logged in successfully: {Email}", email);
                    return user;
                }
                else if (user != null && VerifyPasswordHash(password, user.PasswordHash) == false && user.IsTemporaryPassword == true)
                {
                    var existingRecoveryPassword = await _passwordRecoveryRepository.GetOneAsync(PasswordRecoverSpecifications.ByUserId(user.UserId));
                    if (existingRecoveryPassword != null)
                    {
                        if (VerifyPasswordHash(password, existingRecoveryPassword.RecoveryPassword))
                        {
                            user.IsTemporaryPassword = false;
                            await _passwordRecoveryRepository.DeleteAsync(existingRecoveryPassword);
                            await _userAggregateRepository.UpdateAsync(user);
                            _logger.LogInformation("User logged in successfully with temporary password: {Email}", email);
                            return user;
                        }
                        else
                        {
                            _logger.LogWarning("Invalid email or password: {Email}", email);
                            throw new DomainException(AppMessages.Api_User_GetLogin_Invalid);
                        }                       
                    }
                    else
                    {
                        _logger.LogWarning("Invalid email or password: {Email}", email);
                        throw new DomainException(AppMessages.Api_User_GetLogin_Invalid);
                    }
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

        public async Task<PasswordRecovery> RegisterRecoveryPassword(string email)
        {
            try
            {
                using (_unitOfWork)
                {
                    var user = await _userAggregateRepository.GetOneAsync(UserAggregateSpecifications.ByEmail(email))
                               ?? throw new DomainException(AppMessages.Api_User_GetEmail_Invalid);

                    _logger.LogInformation("User doesn't exist by email {Email}", email);

                    var existingRecoveryPassword = await _passwordRecoveryRepository.GetOneAsync(PasswordRecoverSpecifications.ByUserId(user.UserId));

                    if (existingRecoveryPassword == null)
                    {
                        _logger.LogInformation("No existing recovery password found, creating new one for user ID: {UserId}", user.UserId);
                        var recoveryPasswordData = CreateNewPasswordRecovery();

                        var recoveryPassword = new PasswordRecovery
                        {
                            UserId = user.UserId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            CreatedBy = user.UserId,
                            RecoveryPasswordExpiration = recoveryPasswordData.Expiration,
                            RecoveryPassword = recoveryPasswordData.PasswordHash
                        };

                        await _passwordRecoveryRepository.AddAsync(recoveryPassword);
                        user.IsTemporaryPassword = true;
                        await _userAggregateRepository.UpdateAsync(user);
                        _logger.LogInformation("New recovery password registered successfully for user ID: {UserId}", user.UserId);
                        await _unitOfWork.Complete();
                        return recoveryPassword;
                    }
                    else
                    {
                        _logger.LogInformation("Existing recovery password found, deleting old and creating new one for user ID: {UserId}", user.UserId);
                        await _passwordRecoveryRepository.DeleteAsync(existingRecoveryPassword);

                        var recoveryPasswordData = CreateNewPasswordRecovery();

                        var recoveryPassword = new PasswordRecovery
                        {
                            UserId = user.UserId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            CreatedBy = user.UserId,
                            RecoveryPasswordExpiration = recoveryPasswordData.Expiration,
                            RecoveryPassword = recoveryPasswordData.PasswordHash
                        };

                        await _passwordRecoveryRepository.AddAsync(recoveryPassword);
                        user.IsTemporaryPassword = true;
                        await _userAggregateRepository.UpdateAsync(user);
                        _logger.LogInformation("Old recovery password deleted and new one registered successfully for user ID: {UserId}", user.UserId);
                        await _unitOfWork.Complete();
                        return recoveryPassword;
                    }
                }
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Error during recovery password");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during recovery password");
                throw new DomainException("Unexpected error recovery password", ex);
            }
        }



        private static bool VerifyPasswordHash(string plainTextPassword, byte[] storedHash)
        {
            string hashAsString = Encoding.UTF8.GetString(storedHash);
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashAsString);
        }

        private static PasswordRecoveryData CreateNewPasswordRecovery()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";
            string specialChars = "!@#$%^&*";
            StringBuilder randomPassword = new StringBuilder(5);

            Random rand = new Random();
            randomPassword.Append(specialChars[rand.Next(specialChars.Length)]);

            string allChars = letters + numbers;
            while (randomPassword.Length < 5)
            {
                randomPassword.Append(allChars[rand.Next(allChars.Length)]);
            }

            char[] passwordArray = randomPassword.ToString().ToCharArray();
            for (int i = 0; i < passwordArray.Length; i++)
            {
                int j = rand.Next(i, passwordArray.Length);
                char temp = passwordArray[i];
                passwordArray[i] = passwordArray[j];
                passwordArray[j] = temp;
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(new string(passwordArray));

            return new PasswordRecoveryData
            {
                PasswordHash = Encoding.UTF8.GetBytes(passwordHash),
                Password = new string(passwordArray),
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}

