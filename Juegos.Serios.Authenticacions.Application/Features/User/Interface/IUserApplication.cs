// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IUserApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Application.Models.Request;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate.Dtos;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Shared.Domain.Models;

namespace Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces
{
    public interface IUserApplication
    {
        Task<ApiResponse<object>> CreateUser(UserCreateRequest userCreateRequest);
        Task<ApiResponse<object>> UpdateUserPassword(UpdatePasswordRequest updatePasswordRequest, int userId);
        Task<ApiResponse<PaginatedList<UserDto>>> SearchUsers(string searchTerm, int pageNumber, int pageSize);
    }
}

