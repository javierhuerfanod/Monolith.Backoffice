﻿// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;
using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
using Juegos.Serios.Authenticacions.Application.Models.Request;
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate.Dtos;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Shared.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Authentications.Application.Features.Login
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserAggregateService<User> _userAggregateService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserApplication> _logger;

        public UserApplication(IUserAggregateService<User> userAggregateService, IMapper mapper, ILogger<UserApplication> logger)
        {
            _userAggregateService = userAggregateService ?? throw new ArgumentNullException(nameof(userAggregateService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<object>> CreateUser(UserCreateRequest userCreateRequest)
        {
            try
            {
                var userAggregate = _mapper.Map<UserAggregateModel>(userCreateRequest);
                var user = await _userAggregateService.RegisterUser(userAggregate);
                _logger.LogInformation("User created successfully: {UserId}", user.UserId);
                return new ApiResponse<object>(200, AppMessages.Api_Get_User_Created_Response, true, null);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Domain exception occurred: {Message}", dex.Message);
                return new ApiResponse<object>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }

        public async Task<ApiResponse<object>> UpdateUserPassword(UpdatePasswordRequest updatePasswordRequest, int userId)
        {
            try
            {
                var updatePasswordModel = _mapper.Map<UpdatePasswordModel>(updatePasswordRequest, opts =>
                {
                    opts.Items["userId"] = userId;
                });
                await _userAggregateService.UpdateUserPassword(updatePasswordModel);
                _logger.LogInformation("Password updated successfully for user: {UserId}", userId);
                return new ApiResponse<object>(200, AppMessages.Api_Get_User_UpdatedPassword_Response, true, null);
            }
            catch (DomainException dex)
            {
                _logger.LogError(dex, "Domain exception occurred while updating password");
                return new ApiResponse<object>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating password");
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }
        public async Task<ApiResponse<PaginatedList<UserDto>>> SearchUsers(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Initiating search for users with searchTerm: {SearchTerm}, pageNumber: {PageNumber}, pageSize: {PageSize}", searchTerm, pageNumber, pageSize);

                var paginatedUsers = await _userAggregateService.SearchUsers(searchTerm, pageNumber, pageSize);

                _logger.LogInformation("Search completed successfully. Total records found: {TotalRecords}", paginatedUsers.TotalCount);

                return new ApiResponse<PaginatedList<UserDto>>(200, AppMessages.Api_Usuarios_paginated_successfully, true, paginatedUsers);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Domain exception occurred: {Message}", dex.Message);
                return new ApiResponse<PaginatedList<UserDto>>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while searching for users");
                return new ApiResponse<PaginatedList<UserDto>>(500, AppMessages.Api_Servererror, false, null);
            }
        }

    }
}

