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

using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Shared.Application.Response;

namespace Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces
{
    public interface IUserApplication
    {    
        Task<ApiResponse<object>> CreateUser(UserCreateRequest userCreateRequest);
    }
}

