// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="CityService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the Password Recovery service.</summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class CityService : ICityService<City>
    {
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<CityService> _logger;

        public CityService(ICityRepository cityRepository, ILogger<CityService> logger)
        {
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<City>> SelectAsync()
        {
            try
            {
                return (List<City>)await DomainExceptionHandler.HandleAsync(() =>
                    _cityRepository.ListAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all cities");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }

        public async Task<List<City>> GetAllCitiesOrderedAlphabeticallyAsync()
        {
            try
            {
                var cities = await _cityRepository.GetManyAsync(
                    orderBy: query => query.OrderBy(CitySpecifications.OrderByCityName())
                );

                return [.. cities];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all cities from the repository, ordered alphabetically");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}

