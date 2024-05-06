// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 20-04-2024
// Last Modified On : diego diaz
// ***********************************************************************
// <copyright file="InfrastructureServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Domain;

using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authentications.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class AuthenticationsDomainServiceRegistration
{
    public static IServiceCollection AddAuthenticationsDomainServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IUserAvatarBodyPartService<UserAvatarBodyPart>, UserAvatarBodyPartService>();
        services.AddScoped<IRolService<Role>, RolService>();
        services.AddScoped<IUserAggregateService<User>, UserAggregateService>();
        services.AddScoped<ICityService<City>, CityService>();
        return services;
    }
}

