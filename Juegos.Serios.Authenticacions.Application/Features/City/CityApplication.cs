// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="CityApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;
using Juegos.Serios.Authenticacions.Application.Features.CityApplication.Interfaces;
using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Shared.RedisCache.Interfaces;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Authenticacions.Application.Features.CityApplication
{
    public class CityApplication : ICityApplication
    {
        private readonly ICityService<City> __cityService;
        private readonly IMapper _mapper;
        private readonly IRedisCache _redisCache;
        private readonly ILogger<CityApplication> _logger;
        public CityApplication(ICityService<City> cityService, IMapper mapper, IRedisCache redisCache, ILogger<CityApplication> logger)
        {
            __cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<List<CityDto>>> SelectAsync()
        {
            try
            {
                var cacheKey = $"{nameof(CityApplication)}{nameof(SelectAsync)}";
                var responseApiCache = await _redisCache.GetCacheData<ApiResponse<List<CityDto>>>(cacheKey);
                if (responseApiCache != null)
                {
                    _logger.LogInformation("Returning cached cities data.");
                    return responseApiCache;
                }

                var cityEntities = await __cityService.GetAllCitiesOrderedAlphabeticallyAsync();
                if (cityEntities == null || !cityEntities.Any())
                {
                    _logger.LogWarning("No cities found in the database.");
                    return new ApiResponse<List<CityDto>>(404, AppMessages.Api_City_GetCities_NotFound, false, null);
                }

                var cityDtos = _mapper.Map<List<CityDto>>(cityEntities);
                var apiResponse = new ApiResponse<List<CityDto>>(200, AppMessages.Api_Get_citis_Response, true, cityDtos);

                await _redisCache.SetCacheData(cacheKey, apiResponse, DateTimeOffset.Now.AddMinutes(5.0));
                _logger.LogInformation("Cities data retrieved and cached.");

                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cities data");
                throw new DomainException(AppMessages.Api_Servererror, ex);
            }
        }
    }
}
