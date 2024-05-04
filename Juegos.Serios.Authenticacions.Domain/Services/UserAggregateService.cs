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
using Juegos.Serios.Authenticacions.Domain.Constants;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Models.RecoveryPassword;
using Juegos.Serios.Authenticacions.Domain.Models.RecoveryPassword.Response;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.Extensions.Logging;
using System.Text;


namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class UserAggregateService : IUserAggregateService<User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAggregateRepository _userAggregateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ISessionLogRepository _sessionLogRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IDataConsentRepository _dataConsentRepository;
        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly ILogger<UserAggregateService> _logger;

        public UserAggregateService(IUnitOfWork unitOfWork, IUserAggregateRepository userAggregateRepository, ICityRepository cityRepository, IPasswordRecoveryRepository passwordRecoveryRepository, ISessionLogRepository sessionLogRepository, IRolRepository rolRepository, IDataConsentRepository dataConsentRepository, IDocumentTypeRepository documentTypeRepository, ILogger<UserAggregateService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userAggregateRepository = userAggregateRepository ?? throw new ArgumentNullException(nameof(userAggregateRepository));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(userAggregateRepository));
            _sessionLogRepository = sessionLogRepository ?? throw new ArgumentNullException(nameof(sessionLogRepository));
            _passwordRecoveryRepository = passwordRecoveryRepository ?? throw new ArgumentNullException(nameof(passwordRecoveryRepository));
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _dataConsentRepository = dataConsentRepository ?? throw new ArgumentNullException(nameof(dataConsentRepository));
            _documentTypeRepository = documentTypeRepository ?? throw new ArgumentNullException(nameof(documentTypeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<User> GetById(int id)
        {
            try
            {
                var user = await _userAggregateRepository.GetByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", id);
                    throw new DomainException(AppMessages.Api_User_GetById_NotFound);
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during retrieval of user by ID: {UserId}", id);
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
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
                    SessionLog sessionLog = SessionLog.CreateNewSessionLog(user.UserId, DomainEnumerator.ActionSessionLog.Login.ToString(), DomainEnumerator.IpConfig.NO_APLICA.ToString(), DateTime.Now);
                    await _sessionLogRepository.AddAsync(sessionLog);
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
                            SessionLog sessionLog = SessionLog.CreateNewSessionLog(user.UserId, DomainEnumerator.ActionSessionLog.RePswdLgn.ToString(), DomainEnumerator.IpConfig.NO_APLICA.ToString(), DateTime.Now);
                            await _sessionLogRepository.AddAsync(sessionLog);
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
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }

        public async Task<User> RegisterUser(UserAggregateModel userAggregateModel)
        {
            try
            {
                using (_unitOfWork)
                {
                    if (!userAggregateModel.IsConsentGranted)
                    {
                        _logger.LogWarning("User registration failed: data consent not granted.");
                        throw new DomainException(AppMessages.Api_Created_User_IsConsentendIsfalse_Response);
                    }
                    var city = await _cityRepository.GetByIdAsync(userAggregateModel.CityId);
                    if (city == null)
                    {
                        _logger.LogWarning("User registration failed: City with ID {CityId} not found.", userAggregateModel.CityId);
                        throw new DomainException(AppMessages.Api_City_GetById_NotFound);
                    }

                    var homeCity = await _cityRepository.GetByIdAsync(userAggregateModel.CityHomeId);
                    if (homeCity == null)
                    {
                        _logger.LogWarning("User registration failed: Home city with ID {CityHomeId} not found.", userAggregateModel.CityHomeId);
                        throw new DomainException(AppMessages.Api_City_GetById_NotFound);
                    }

                    var role = await _rolRepository.GetByIdAsync(userAggregateModel.RoleId);
                    if (role == null)
                    {
                        _logger.LogWarning("User registration failed: Role with ID {RoleId} not found.", userAggregateModel.RoleId);
                        throw new DomainException(AppMessages.Api_Rol_GetById_NotFound);
                    }

                    var documentType = await _documentTypeRepository.GetByIdAsync(userAggregateModel.DocumentTypeId);
                    if (documentType == null)
                    {
                        _logger.LogWarning("User registration failed: Document type with ID {DocumentTypeId} not found.", userAggregateModel.DocumentTypeId);
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
                    var userCreated = await _userAggregateRepository.AddAsync(user);
                    var dataConsented = await _dataConsentRepository.AddAsync(new DataConsent
                    {
                        UserId = userCreated.UserId,
                        ConsentStatus = userAggregateModel.IsConsentGranted,
                        ConsentDate = DateTime.Now
                    });
                    _logger.LogInformation("DataConsent registered successfully: {DataConsentId}", dataConsented.ConsentId);
                    await _unitOfWork.Complete();
                    _logger.LogInformation("User registered successfully: {Username}", user.Username);
                    return user;
                }
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

        public async Task<PasswordRecoveryResponse> RegisterRecoveryPassword(string email)
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
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CreatedBy = user.UserId,
                            RecoveryPasswordExpiration = recoveryPasswordData.Expiration,
                            RecoveryPassword = recoveryPasswordData.PasswordHash
                        };

                        await _passwordRecoveryRepository.AddAsync(recoveryPassword);
                        user.IsTemporaryPassword = true;
                        await _userAggregateRepository.UpdateAsync(user);
                        _logger.LogInformation("New recovery password registered successfully for user ID: {UserId}", user.UserId);
                        await _unitOfWork.Complete();
                        return new PasswordRecoveryResponse
                        {
                            LastName = user.LastName,
                            Name = user.FirstName,
                            Password = recoveryPasswordData.Password
                        };
                    }
                    else
                    {
                        _logger.LogInformation("Existing recovery password found, deleting old and creating new one for user ID: {UserId}", user.UserId);
                        await _passwordRecoveryRepository.DeleteAsync(existingRecoveryPassword);

                        var recoveryPasswordData = CreateNewPasswordRecovery();

                        var recoveryPassword = new PasswordRecovery
                        {
                            UserId = user.UserId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CreatedBy = user.UserId,
                            RecoveryPasswordExpiration = recoveryPasswordData.Expiration,
                            RecoveryPassword = recoveryPasswordData.PasswordHash
                        };

                        await _passwordRecoveryRepository.AddAsync(recoveryPassword);
                        user.IsTemporaryPassword = true;
                        await _userAggregateRepository.UpdateAsync(user);
                        _logger.LogInformation("Old recovery password deleted and new one registered successfully for user ID: {UserId}", user.UserId);
                        await _unitOfWork.Complete();
                        return new PasswordRecoveryResponse
                        {
                            LastName = user.LastName,
                            Name = user.FirstName,
                            Password = recoveryPasswordData.Password
                        };
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
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
        public async Task UpdateUserPassword(UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                var user = await _userAggregateRepository.GetOneAsync(UserAggregateSpecifications.ById(updatePasswordModel.UserId))
                       ?? throw new DomainException(AppMessages.Api_User_GetEmail_Invalid);

                user.UpdatePassword(updatePasswordModel);
                await _userAggregateRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during update password");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }

        private static bool VerifyPasswordHash(string plainTextPassword, byte[] storedHash)
        {
            string hashAsString = Encoding.UTF8.GetString(storedHash);
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashAsString);
        }

        private static PasswordRecoveryModel CreateNewPasswordRecovery()
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

            return new PasswordRecoveryModel
            {
                PasswordHash = Encoding.UTF8.GetBytes(passwordHash),
                Password = new string(passwordArray),
                Expiration = DateTime.Now.AddHours(1)
            };
        }
    }
}

