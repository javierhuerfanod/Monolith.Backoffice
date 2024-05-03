// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="BathroomInfrastructureServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Infrastructure;
using Juegos.Serios.Bathroom.Infrastructure.Persistence;
using Juegos.Serios.Bathroom.Infrastructure.Repositories;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class BathroomInfrastructureServiceRegistration
{
    public static IServiceCollection AddBathroomInfrastuctureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BdSqlBathroomContext>(options =>
         options.UseSqlServer(configuration.GetConnectionString("BathroomSqlDbConnection"))
         );
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));     
        return services;
    }
}

