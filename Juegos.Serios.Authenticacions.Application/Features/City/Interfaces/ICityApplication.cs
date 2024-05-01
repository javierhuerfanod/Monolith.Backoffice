// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ICityApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Shared.Application.Response;

namespace Juegos.Serios.Authenticacions.Application.Features.CityApplication.Interfaces
{
    public interface ICityApplication
    {
        Task<ApiResponse<List<CityDto>>> SelectAsync();
    }
}

