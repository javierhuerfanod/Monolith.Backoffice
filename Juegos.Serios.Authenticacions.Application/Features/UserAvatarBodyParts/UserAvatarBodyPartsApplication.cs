// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserAvatarBodyPartsApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;
using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
using Juegos.Serios.Authenticacions.Application.Models.Request;
using Juegos.Serios.Authenticacions.Application.Models.Response;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Models.UserAvatarBodyParts;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Shared.RedisCache.Interfaces;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Authenticacions.Application.Features.UserAvatarBodyParts
{
    public class UserAvatarBodyPartsApplication : IUserAvatarBodyPartsApplication
    {
        private readonly IUserAvatarBodyPartService<UserAvatarBodyPart> _userAvatarBodyPartService;
        private readonly IMapper _mapper;
        private readonly IRedisCache _redisCache;
        private readonly ILogger<UserAvatarBodyPartsApplication> _logger;

        public UserAvatarBodyPartsApplication(IUserAvatarBodyPartService<UserAvatarBodyPart> userAvatarBodyPartService, IMapper mapper, IRedisCache redisCache, ILogger<UserAvatarBodyPartsApplication> logger)
        {
            _userAvatarBodyPartService = userAvatarBodyPartService ?? throw new ArgumentNullException(nameof(userAvatarBodyPartService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<object>> CreateUserAvatarBodyParts(UserBodyPartsRequest userBodyPartsRequest, int userId)
        {
            _logger.LogInformation("Starting the creation of avatar body parts for user ID: {UserId}", userId);

            try
            {
                var userBodyPartsModel = _mapper.Map<UserBodyPartsModel>(userBodyPartsRequest, opts =>
                {
                    opts.Items["userId"] = userId;
                });

                _logger.LogInformation("Mapped UserBodyPartsRequest to UserBodyPartsModel for user ID: {UserId}", userId);
                await _userAvatarBodyPartService.RegisterUserAvatarBodyParts(userBodyPartsModel);
                _logger.LogInformation("User avatar body parts created successfully for user ID: {UserId}", userId);
                return new ApiResponse<object>(200, AppMessages.Api_Get_UserAvatarBodyParts_Created_Response, true, null);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Domain exception occurred while creating avatar body parts for user ID: {UserId}: {Message}", userId, dex.Message);
                return new ApiResponse<object>(400, dex.Message, false, null);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating avatar body parts for user ID: {UserId}", userId);
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }

        public async Task<ApiResponse<List<UserBodyPartsResponse>>> SelectByUserId(int userId)
        {
            _logger.LogInformation("Starting to retrieve avatar body parts for user ID: {UserId}", userId);
            var cacheKey = $"{nameof(UserAvatarBodyPartsApplication)}{nameof(SelectByUserId)}";
            var responseApiCache = await _redisCache.GetCacheData<ApiResponse<List<UserBodyPartsResponse>>>(cacheKey);
            if (responseApiCache != null)
            {
                _logger.LogInformation("Returning cached cities data.");
                return responseApiCache;
            }
            try
            {
                var userAvatarBodyParts = await _userAvatarBodyPartService.GetByUserId(userId);
                if (userAvatarBodyParts == null || !userAvatarBodyParts.Any())
                {
                    _logger.LogWarning("No avatar body parts found for user ID: {UserId}", userId);
                    return new ApiResponse<List<UserBodyPartsResponse>>(404, AppMessages.Api_UserAvatarBodyParts_GetByUserId_NotFound, false, null);
                }

                var userAvatarBodyPartsResponse = _mapper.Map<List<UserBodyPartsResponse>>(userAvatarBodyParts);
                var apiResponse = new ApiResponse<List<UserBodyPartsResponse>>(200, AppMessages.Api_UserAvatarBodyParts_GetByUserId_Success, true, userAvatarBodyPartsResponse);
                _logger.LogInformation("Successfully retrieved and mapped avatar body parts for user ID: {UserId}", userId);
                await _redisCache.SetCacheData(cacheKey, apiResponse, DateTimeOffset.Now.AddMinutes(5.0));
                return apiResponse;
            }
            catch (DomainException dex)
            {
                _logger.LogError(dex, "Domain exception while retrieving avatar body parts for user ID: {UserId}", userId);
                throw new DomainException(dex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving avatar body parts by UserId: {UserId}", userId);
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}

