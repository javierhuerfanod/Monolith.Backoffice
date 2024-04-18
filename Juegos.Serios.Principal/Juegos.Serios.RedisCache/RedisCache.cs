// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.RedisCache
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RedisCache.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.RedisCache
{
    using Juegos.Serios.Shared.RedisCache.Interfaces;
    using Newtonsoft.Json;
    using StackExchange.Redis;   

    /// <summary>
    /// Provides methods for working with Redis cache.
    /// </summary>
    public class RedisCache : IRedisCache
    {
        private readonly IDatabase _dbRedis;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCache"/> class.
        /// </summary>
        /// <param name="connectionMultiplexer">The Redis connection multiplexer.</param>
        public RedisCache(IConnectionMultiplexer connectionMultiplexer)
        {
            _dbRedis = connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// Gets data from the cache.
        /// </summary>
        /// <typeparam name="T">The type of the data to retrieve.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>The cached data, or default value if not found.</returns>
        public async Task<T> GetCacheData<T>(string key)
        {
            var value = await _dbRedis.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        /// <summary>
        /// Removes data from the cache.
        /// </summary>
        /// <param name="key">The cache key to remove.</param>
        /// <returns>True if the data was removed, false if the key does not exist.</returns>
        public async Task<bool> RemoveData(string key)
        {
            bool isKeyExist = await _dbRedis.KeyExistsAsync(key);
            if (isKeyExist)
            {
                return await _dbRedis.KeyDeleteAsync(key);
            }
            return false;
        }

        /// <summary>
        /// Sets data in the cache with an expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the data to store.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="value">The data to store in the cache.</param>
        /// <param name="expirationTime">The expiration time for the cached data.</param>
        /// <returns>True if the data was successfully stored in the cache, false otherwise.</returns>
        public async Task<bool> SetCacheData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _dbRedis.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }

    }
}