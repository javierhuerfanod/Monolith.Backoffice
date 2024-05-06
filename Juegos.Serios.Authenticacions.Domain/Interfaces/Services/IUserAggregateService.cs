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

using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Models.RecoveryPassword.Response;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate.Dtos;
using Juegos.Serios.Shared.Domain.Models;

namespace Juegos.Serios.Authenticacions.Domain.Interfaces.Services
{
    public interface IUserAggregateService<T>
    {
        Task<T> GetByEmailAndPassword(string email, string password);
        Task<User> RegisterUser(UserAggregateModel userAggregateModel);
        Task<PasswordRecoveryResponse> RegisterRecoveryPassword(string email);
        Task UpdateUserPassword(UpdatePasswordModel updatePasswordModel);
        Task<User> GetById(int id);
        Task<PaginatedList<UserDto>> SearchUsers(string searchTerm, int pageNumber, int pageSize);
    }
}