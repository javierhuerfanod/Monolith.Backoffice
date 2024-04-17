// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IAsyncRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Authenticacions.Domain.Ports.Persistence
{
    using Juegos.Serios.Authenticacions.Domain.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IAsyncRepository<T> where T : BaseDomainModel
    {
        /// <summary>
        /// Retrieves all items asynchronously.
        /// </summary>
        /// <returns>An asynchronous task that returns an IReadOnlyList of items of type T.</returns>
        Task<IReadOnlyList<T>> GetAllAsync();
        /// <summary>
        /// Retrieves items asynchronously that satisfy the specified filtering criteria.
        /// </summary>
        /// <param name="predicate">A lambda expression representing the filtering criteria.</param>
        /// <returns>An asynchronous task that returns an IReadOnlyList of items of type T that meet the given predicate.</returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Retrieves items asynchronously based on a specified predicate.
        /// </summary>
        /// <param name="predicate">A lambda expression representing the filtering criteria.</param>
        /// <returns>An asynchronous task that returns an IReadOnlyList of items of type T that match the given predicate.</returns>
        /// <summary>
        /// Retrieves items asynchronously based on optional filtering, sorting, and including related entities.
        /// </summary>
        /// <param name="predicate">A lambda expression representing the filtering criteria (optional).</param>
        /// <param name="orderBy">A function for ordering the results (optional).</param>
        /// <param name="includeString">A string specifying related entities to include (optional).</param>
        /// <param name="disableTracking">A boolean indicating whether to disable change tracking (optional).</param>
        /// <returns>An asynchronous task that returns an IReadOnlyList of items of type T that meet the specified criteria.</returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true);

        /// <summary>
        /// Retrieves items asynchronously based on optional filtering, sorting, and including related entities.
        /// </summary>
        /// <param name="predicate">A lambda expression representing the filtering criteria (optional).</param>
        /// <param name="orderBy">A function for ordering the results (optional).</param>
        /// <param name="includes">A list of lambda expressions specifying related entities to include (optional).</param>
        /// <param name="disableTracking">A boolean indicating whether to disable change tracking (optional).</param>
        /// <returns>An asynchronous task that returns an IReadOnlyList of items of type T that meet the specified criteria.</returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);

        /// <summary>
        /// Retrieves an item asynchronously by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to retrieve.</param>
        /// <returns>An asynchronous task that returns an item of type T with the specified identifier.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds an entity asynchronously to the data source.
        /// </summary>
        /// <param name="entity">The entity of type T to add.</param>
        /// <returns>An asynchronous task that returns the added entity of type T.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an entity asynchronously in the data source.
        /// </summary>
        /// <param name="entity">The entity of type T to update.</param>
        /// <returns>An asynchronous task that returns the updated entity of type T.</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity asynchronously from the data source.
        /// </summary>
        /// <param name="entity">The entity of type T to delete.</param>
        /// <returns>An asynchronous task representing the deletion operation.</returns>
        Task<bool> DeleteAsync(T entity);

        void AddEntity(T entity);

        void UpdateEntity(T entity);

        void DeleteEntity(T entity);
    }
}