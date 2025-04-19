// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IWeightRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using System.Linq.Expressions;
namespace Juegos.Serios.Bathroom.Domain.Interfaces.Repositories
{
    public interface IWeightRepository : IAsyncRepository<Weight>
    {
      Task<(IReadOnlyList<Weight>, int)> ListPaginatedWeightAsync(Expression<Func<Weight, bool>> predicate, int pageNumber, int pageSize);
    }
}