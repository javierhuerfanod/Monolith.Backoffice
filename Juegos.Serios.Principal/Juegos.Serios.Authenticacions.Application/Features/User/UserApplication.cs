// ***********************************************************************
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
using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Aggregates.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Domain.Shared.Exceptions;

namespace Juegos.Serios.Authenticacions.Application.Features.Login
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserAggregateService<User> _userAggregateService;
        private readonly IMapper _mapper;

        public UserApplication(IUserAggregateService<User> userAggregateService, IMapper mapper)
        {
            _userAggregateService = userAggregateService ?? throw new ArgumentNullException(nameof(userAggregateService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        public async Task<ApiResponse<object>> CreateUser(UserCreateRequest userCreateRequest)
        {
            try
            {
                var userAggregate = _mapper.Map<UserAggregateModel>(userCreateRequest);
                var user = await _userAggregateService.RegisterUser(userAggregate);
                return new ApiResponse<object>(200, AppMessages.Api_Get_User_Created_Response, true, null);
            }
            catch (DomainException dex)
            {                
                return new ApiResponse<object>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {               
                return new ApiResponse<object>(500, AppMessages.Api_Servererror, false, null);
            }
        }

    }
}
