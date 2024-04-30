// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.RedisCache
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IRedisCache.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.RedisCache.Interfaces
{
    public interface IRedisCache
    {
        /// <summary>
        /// Gets data from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached data, or default value if not found.</returns>
        Task<T> GetCacheData<T>(string key);
        /// <summary>
        /// Removes data from the cache.
        /// </summary>
        /// <param name="key">The cache key to remove.</param>
        /// <returns>True if the data was removed, false if the key does not exist.</returns>
        Task<bool> RemoveData(string key);
        /// <summary>
        /// Sets data in the cache with an expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the data to store.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="expirationTime">The expiration time for the cached data.</param>
        /// <returns>True if the data was successfully stored in the cache, false otherwise.</returns>
        Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime);
    }
}