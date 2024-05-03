// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="AuthenticationsInfrastructureServiceRegistration.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Infrastructure;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;
using Juegos.Serios.Authenticacions.Infrastructure.Repositories;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class AuthenticationsInfrastructureServiceRegistration
{
    public static IServiceCollection AddAuthenticationsInfrastuctureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BdSqlAuthenticationContext>(options =>
         options.UseSqlServer(configuration.GetConnectionString("AuthenticationsSqlDbConnection"))
         );
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IRolRepository, RoleRepository>();
        services.AddScoped<IUserAggregateRepository, UserRepository>();
        services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
        services.AddScoped<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
        services.AddScoped<IDataConsentRepository, DataConsentRepository>();
        services.AddScoped<ISessionLogRepository, SessionLogRepository>();
        services.AddScoped<IUserAvatarBodyPartRepository, UserAvatarBodyPartRepository>();
        return services;
    }
}

