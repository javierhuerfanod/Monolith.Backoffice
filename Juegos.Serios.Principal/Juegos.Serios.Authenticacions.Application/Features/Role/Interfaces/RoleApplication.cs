// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IRoleApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Shared.Application.Response;

namespace Juegos.Serios.Authenticacions.Application.Features.Role.Interfaces
{
    public interface IRoleApplication
    {
        Task<ApiResponse<RolDto>> GetById(int id);
        Task<ApiResponse<RolDto>> GetByName(string roleName);
        Task<ApiResponse<List<RolDto>>> SelectAsync();
    }
}

