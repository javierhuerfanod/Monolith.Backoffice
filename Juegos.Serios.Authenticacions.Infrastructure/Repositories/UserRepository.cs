// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserAggregateRepository
    {
        public UserRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
        public async Task<(IReadOnlyList<User>, int)> ListPaginatedUsersAsync(Expression<Func<User, bool>> predicate, int pageNumber, int pageSize)
        {
            IQueryable<User> query = _context.Set<User>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            query = query.OrderBy(u => u.LastName).ThenBy(u => u.FirstName);
            int totalRecords = await query.CountAsync();
          
            int maxPageNumber = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (pageNumber > maxPageNumber)
            {
                pageNumber = Math.Max(1, maxPageNumber); 
            }
            List<User> users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (users, totalRecords);
        }

    }
}
