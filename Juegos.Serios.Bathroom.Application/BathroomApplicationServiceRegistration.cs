// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="BathroomApplicationServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Bathroom.Application
{
    using Juegos.Serios.Bathroom.Application.Features.Weight.Interfaces;
    using Juegos.Serios.Bathroom.Application.Features.WeightApplication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class BathroomApplicationServiceRegistration
    {
        public static IServiceCollection AddBathroomApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IWeightApplication, WeightApplication>();          
            return services;
        }
    }
}
