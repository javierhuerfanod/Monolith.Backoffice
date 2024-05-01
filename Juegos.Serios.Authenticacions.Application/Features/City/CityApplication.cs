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
using Juegos.Serios.Authenticacions.Application.Exceptions;
using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Shared.RedisCache.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.City.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Entities.City;
using Juegos.Serios.Authenticacions.Application.Features.CityApplication.Interfaces;

namespace Juegos.Serios.Authenticacions.Application.Features.CityApplication
{
    public class CityApplication : ICityApplication
    {
        private readonly ICityService<City> __cityService;
        private readonly IMapper _mapper;
        private readonly IRedisCache _redisCache;
        public CityApplication(ICityService<City> cityService, IMapper mapper, IRedisCache redisCache)
        {
            __cityService = cityService ?? throw new ArgumentNullException(nameof(cityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<ApiResponse<List<CityDto>>> SelectAsync()
        {
            try
            {
                var responseApiCache = await _redisCache.GetCacheData<ApiResponse<List<CityDto>>>($"{nameof(CityApplication)}{nameof(SelectAsync)}");
                if (responseApiCache != null)
                {
                    return responseApiCache;
                }

                var cityEntities = await __cityService.GetAllCitiesOrderedAlphabeticallyAsync();
                var cityDtos = _mapper.Map<List<CityDto>>(cityEntities);
                var apiresponse = new ApiResponse<List<CityDto>>(200, AppMessages.Api_Get_citis_Response, true, cityDtos);
                await _redisCache.SetCacheData($"{nameof(CityApplication)}{nameof(SelectAsync)}", apiresponse, DateTimeOffset.Now.AddMinutes(5.0));
                return new ApiResponse<List<CityDto>>(200, AppMessages.Api_Get_citis_Response, true, cityDtos);
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_City_Select_Error, ex);
            }
        }
    }
}
