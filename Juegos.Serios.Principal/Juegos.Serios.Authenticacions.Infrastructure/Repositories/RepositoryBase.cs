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

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories;

using Juegos.Serios.Authenticacions.Domain.Common;
using Juegos.Serios.Authenticacions.Domain.Ports.Persistence;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

/// <summary>
/// Base repository class for data access operations with entities of type T.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class RepositoryBase<T> : IAsyncRepository<T> where T : BaseDomainModel
{
    protected readonly BdSqlAuthenticationContext _context;

    public RepositoryBase(BdSqlAuthenticationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all entities of type T asynchronously.
    /// </summary>
    /// <returns>An asynchronous task that returns a list of all entities of type T.</returns>
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Retrieves entities of type T asynchronously based on a specified predicate.
    /// </summary>
    /// <param name="predicate">A lambda expression representing the filtering criteria.</param>
    /// <returns>An asynchronous task that returns a list of entities of type T that match the given predicate.</returns>
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Retrieves entities of type T asynchronously based on optional filtering, sorting, and related entity inclusion.
    /// </summary>
    /// <param name="predicate">A lambda expression representing the filtering criteria (optional).</param>
    /// <param name="orderBy">A function for ordering the results (optional).</param>
    /// <param name="includeString">A string specifying related entities to include (optional).</param>
    /// <param name="disableTracking">A boolean indicating whether to disable change tracking (optional).</param>
    /// <returns>An asynchronous task that returns a list of entities of type T that meet the specified criteria.</returns>
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeString = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves entities of type T asynchronously based on optional filtering, sorting, and related entity inclusion.
    /// </summary>
    /// <param name="predicate">A lambda expression representing the filtering criteria (optional).</param>
    /// <param name="orderBy">A function for ordering the results (optional).</param>
    /// <param name="includes">A list of expressions specifying related entities to include (optional).</param>
    /// <param name="disableTracking">A boolean indicating whether to disable change tracking (optional).</param>
    /// <returns>An asynchronous task that returns a list of entities of type T that meet the specified criteria.</returns>
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<Expression<Func<T, object>>> includes = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();
        if (disableTracking) query = query.AsNoTracking();
        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));
        if (predicate != null) query = query.Where(predicate);
        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves an entity of type T asynchronously by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>An asynchronous task that returns the entity of type T with the specified unique identifier.</returns>
    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Adds an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>An asynchronous task that returns the added entity of type T.</returns>
    public async Task<T> AddAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Updates an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>An asynchronous task that returns the updated entity of type T.</returns>
    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes an entity of type T asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public async Task<bool> DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public void AddEntity(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void UpdateEntity(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteEntity(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}

