// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="InfrastructureServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Domain;

using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

public static class AuthenticationsDomainServiceRegistration
{
    public static IServiceCollection AddAuthenticationsDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IRolService<RolEntity>, RolService>();
        return services;
    }
}

