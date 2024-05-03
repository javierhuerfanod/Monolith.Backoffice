// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ICityService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Entities;

namespace Juegos.Serios.Authenticacions.Domain.Interfaces.Services
{
    public interface ICityService<T>
    {
        Task<List<T>> SelectAsync();
        Task<List<City>> GetAllCitiesOrderedAlphabeticallyAsync();
    }
}