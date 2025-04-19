// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.AzureQueue
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="AzureQueueServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.AzureQueue
{
    using Juegos.Serios.Shared.AzureQueue.Interfaces;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class AzureQueueServiceRegistration
    {
        public static IServiceCollection AddAzureQueueServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAzureQueue>(provider =>
            {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
                return new AzureQueue(configuration["AzureAccessStorage"]);
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            });

            return services;
        }
    }
}
