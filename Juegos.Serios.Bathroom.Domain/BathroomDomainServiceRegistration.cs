// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="BathroomDomainServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Domain;

using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Interfaces.Services;
using Juegos.Serios.Bathroom.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

public static class BathroomDomainServiceRegistration
{
    public static IServiceCollection AddBathroomDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IWeightService<Weight>, WeightService>();    
        return services;
    }
}

