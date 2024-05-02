// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="UserAvatarBodyPartService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the role service.</summary>
// ***********************************************************************


using Juegos.Serios.Authenticacions.Domain.Entities.SessionLog.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.UserAvatar;
using Juegos.Serios.Authenticacions.Domain.Entities.UserAvatar.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Juegos.Serios.Authenticacions.Domain.Models.UserAvatarBodyParts;


namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class UserAvatarBodyPartService : IUserAvatarBodyPartService<UserAvatarBodyPart>
    {
        private readonly IUserAvatarBodyPartRepository _userAvatarBodyPartRepository;
        private readonly ILogger<UserAvatarBodyPartService> _logger;

        public UserAvatarBodyPartService(IUserAvatarBodyPartRepository userAvatarBodyPartRepository, ILogger<UserAvatarBodyPartService> logger)
        {
            _userAvatarBodyPartRepository = userAvatarBodyPartRepository ?? throw new ArgumentNullException(nameof(userAvatarBodyPartRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> RegisterUserAvatarBodyParts(UserBodyPartsModel userBodyPartsModel)
        {
            _logger.LogInformation("Starting registration of avatar body parts for user ID: {UserId}", userBodyPartsModel.UserId);

            try
            {
                var userAvatarBodyPart = await _userAvatarBodyPartRepository.GetManyAsync(UserAvatarBodyPartSpecifications.ByUserId(userBodyPartsModel.UserId));
                if (userAvatarBodyPart.Any())
                {
                    _logger.LogWarning("Attempt to register duplicate avatar body parts for user ID: {UserId}", userBodyPartsModel.UserId);
                    throw new DomainException(AppMessages.Api_UserAvatarBodyPart_GetByUserId_Exist);
                }

                if (userBodyPartsModel.BodyParts.Any(bp => !Enum.IsDefined(typeof(BodyPartName), bp.BodyPartName)))
                {
                    _logger.LogWarning("Invalid body part names found in the request for user ID: {UserId}", userBodyPartsModel.UserId);
                    throw new DomainException(AppMessages.Api_UserAvatarBodyParts_Create_DontExist_BodyParts);
                }

                if (userBodyPartsModel.BodyParts.GroupBy(p => p.BodyPartName).Any(g => g.Count() > 1))
                {
                    _logger.LogWarning("Duplicate body part names in the request for user ID: {UserId}", userBodyPartsModel.UserId);
                    throw new DomainException(AppMessages.Api_UserAvatarBodyParts_Create_Duplicated);
                }

                var userBodyParts = UserAvatarBodyPart.UpdateUserAvatarBodyParts(userBodyPartsModel);
                foreach (var item in userBodyParts)
                {
                    _logger.LogInformation("Adding avatar body part {BodyPartName} for user ID: {UserId}", item.BodyPartName, userBodyPartsModel.UserId);
                    await _userAvatarBodyPartRepository.AddAsync(item);
                }

                _logger.LogInformation("Successfully registered avatar body parts for user ID: {UserId}", userBodyPartsModel.UserId);
                return true;
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Error during create user avatar body parts registration");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration of avatar body parts for user ID: {UserId}", userBodyPartsModel.UserId);
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
        public async Task<List<UserAvatarBodyPart>> GetByUserId(int id)
        {
            try
            {
                var userAvatarBodyPart = await _userAvatarBodyPartRepository.GetManyAsync(UserAvatarBodyPartSpecifications.ByUserId(id));
                if (userAvatarBodyPart.Count == 0)
                {
                    _logger.LogWarning("Avatar not found with UserId: {UserId}", id);
                    throw new DomainException(AppMessages.Api_User_GetById_NotFound);
                }
                return [.. userAvatarBodyPart];
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Error retrieving Avatar by UserId");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Avatar by UserId");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}

