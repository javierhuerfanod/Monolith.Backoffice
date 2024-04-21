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
using System.Text;


namespace Juegos.Serios.Authenticacions.Domain.Services
{
    public sealed class UserAggregateService : IUserAggregateService<User>
    {
        private readonly IUserAggregateRepository _userAggregateRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public UserAggregateService(IUserAggregateRepository userAggregateRepository, IRolRepository rolRepository, IDocumentTypeRepository documentTypeRepository)
        {
            _userAggregateRepository = userAggregateRepository ?? throw new ArgumentNullException(nameof(userAggregateRepository));
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
            _documentTypeRepository = documentTypeRepository ?? throw new ArgumentNullException(nameof(documentTypeRepository));
        }

        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await _userAggregateRepository.GetOneAsync(UserAggregateSpecifications.ByEmail(email)) ?? throw new DomainException(AppMessages.Api_User_GetLogin_Invalid);
                if (user != null && VerifyPasswordHash(password, user.PasswordHash))
                {
                    return user;
                }
                else
                {
                    throw new DomainException(AppMessages.Api_User_GetLogin_Invalid);
                }               
            }
            catch (DomainException)
            {
                throw;
            }
            catch (Exception ex)
            {
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
                    throw new DomainException(AppMessages.Api_Get_User_Duplicated_Response);
                }

                User user = User.CreateNewUser(userAggregateModel);
                return await _userAggregateRepository.AddAsync(user);
            }
            catch (DomainException)
            {               
                throw;
            }
            catch (Exception ex)
            {               
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

