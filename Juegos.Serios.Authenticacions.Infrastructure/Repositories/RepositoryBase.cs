// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RepositoryBase.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Infrastructure.Persistence;
using Juegos.Serios.Shared.Domain.Common;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseDomainModel
    {
        protected readonly BdSqlAuthenticationContext _context;

        public RepositoryBase(BdSqlAuthenticationContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await _context.Set<T>().FindAsync(id);
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetManyAsync(
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
           Expression<Func<T, bool>> predicate = null,
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
           string includeString = null,
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
           bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) query = orderBy(query);
            return await query.ToListAsync();
        }

        public async Task<T> GetOneAsync(
    Expression<Func<T, bool>> predicate,
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    string includeString = null,
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
    bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);

#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return await query.FirstOrDefaultAsync();
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
    }
}


