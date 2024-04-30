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
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis;
    using Juegos.Serios.Shared.RedisCache.Interfaces;

    public static class RedisCacheServiceRegistration
    {
        public static IServiceCollection AddRedisCacheServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(configuration["RedisCache"]!.ToString()));
            services.AddScoped<IRedisCache, RedisCache>();
            return services;
        }
    }
}
