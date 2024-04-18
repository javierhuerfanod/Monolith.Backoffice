// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RolDto.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Authenticacions.Application
{
    using Juegos.Serios.Authenticacions.Application.Features.Role.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Features.Role;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Juegos.Serios.Shared.AzureQueue;
    using Juegos.Serios.Shared.RedisCache;
    using Microsoft.Extensions.Configuration;

    public static class AuthenticationsApplicationServiceRegistration
    {
        public static IServiceCollection AddAuthenticationsApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRoleApplication, RoleApplication>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddRedisCacheServiceRegistration(configuration);
            services.AddAzureQueueServiceRegistration(configuration);
            return services;
        }
    }
}
