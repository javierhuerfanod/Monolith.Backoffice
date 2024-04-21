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
using Juegos.Serios.Authenticacions.Domain.Aggregates.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

public static class AuthenticationsDomainServiceRegistration
{
    public static IServiceCollection AddAuthenticationsDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IRolService<Role>, RolService>();
        services.AddScoped<IUserAggregateService<User>, UserAggregateService>();
        return services;
    }
}

