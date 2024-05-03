// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IPasswordRecoveryService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Authenticacions.Domain.Interfaces.Services
{
    public interface IPasswordRecoveryService<T>
    {
        Task<List<T>> SelectAsync();

        public Task<T> GetById(int id);

    }
}