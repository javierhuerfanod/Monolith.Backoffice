// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IUserAggregateService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;

namespace Juegos.Serios.Authenticacions.Domain.Aggregates.Interfaces
{
    public interface IUserAggregateService<T>
    {
        Task<T> GetByEmailAndPassword(string email, string password);
        Task<User> RegisterUser(UserAggregateModel userAggregateModel);
        Task<PasswordRecovery> RegisterRecoveryPassword(string email);
    }
}