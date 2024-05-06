// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Infrasturcture
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="WeightRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Juegos.Serios.Bathroom.Infrastructure.Repositories
{
    public class WeightRepository : RepositoryBase<Weight>, IWeightRepository
    {
        public WeightRepository(BdSqlBathroomContext context) : base(context)
        {

        }
        public async Task<(IReadOnlyList<Weight>, int)> ListPaginatedWeightAsync(Expression<Func<Weight, bool>> predicate, int pageNumber, int pageSize)
        {
            IQueryable<Weight> query = _context.Set<Weight>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            query = query.OrderByDescending(w => w.Date);
            int totalRecords = await query.CountAsync();

            int maxPageNumber = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (pageNumber > maxPageNumber)
            {
                pageNumber = Math.Max(1, maxPageNumber);
            }
            List<Weight> users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (users, totalRecords);
        }
    }
}
