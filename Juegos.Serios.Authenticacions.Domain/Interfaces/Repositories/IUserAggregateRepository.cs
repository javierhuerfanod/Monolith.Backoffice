// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IUserAggregateRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using System.Linq.Expressions;

namespace Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories
{
    public interface IUserAggregateRepository : IAsyncRepository<User>
    {
        Task<(IReadOnlyList<User>, int)> ListPaginatedUsersAsync(Expression<Func<User, bool>> predicate, int pageNumber, int pageSize);
    }
}