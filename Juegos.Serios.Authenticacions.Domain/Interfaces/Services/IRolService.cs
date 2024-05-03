// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IRoleFinder.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Entities;

namespace Juegos.Serios.Authenticacions.Domain.Interfaces.Services
{
    public interface IRolService<T>
    {
        Task<List<T>> SelectAsync();
        Task<Role> GetByName(string rolename);

        public Task<T> GetById(int id);
        Task<Role> CreateRoleAsync(string roleName);

    }
}