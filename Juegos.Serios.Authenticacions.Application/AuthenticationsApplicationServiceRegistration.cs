// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : diego diaz
// Last Modified On : 22-04-2024
// ***********************************************************************
// <copyright file="AuthenticationsApplicationServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Authenticacions.Application
{
    using Juegos.Serios.Authenticacions.Application.Features.Rol.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Features.Rol;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Juegos.Serios.Shared.AzureQueue;
    using Juegos.Serios.Shared.RedisCache;
    using Microsoft.Extensions.Configuration;
    using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
    using Juegos.Serios.Authentications.Application.Features.Login;
    using Juegos.Serios.Authenticacions.Application.Features.Authentication.RecoveryPassword;
    using Juegos.Serios.Authenticacions.Application.Features.CityApplication.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Features.CityApplication;

    public static class AuthenticationsApplicationServiceRegistration
    {
        public static IServiceCollection AddAuthenticationsApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICityApplication, CityApplication>();
            services.AddScoped<IRoleApplication, RoleApplication>();
            services.AddScoped<ILoginApplication, LoginApplication>();
            services.AddScoped<IRecoveryPasswordAuthenticationApplication, RecoveryPasswordAuthenticationApplication>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddRedisCacheServiceRegistration(configuration);
            services.AddAzureQueueServiceRegistration(configuration);
            return services;
        }
    }
}
