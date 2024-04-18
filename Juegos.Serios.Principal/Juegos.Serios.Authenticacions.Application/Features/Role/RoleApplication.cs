// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RoleApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;
using Juegos.Serios.Authenticacions.Application.Exceptions;
using Juegos.Serios.Authenticacions.Application.Models.Dtos;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Application.Features.Role.Interfaces;
using Juegos.Serios.Shared.RedisCache.Interfaces;
using Juegos.Serios.Shared.AzureQueue.Interfaces;
using Juegos.Serios.Shared.AzureQueue;
using Newtonsoft.Json;

namespace Juegos.Serios.Authenticacions.Application.Features.Role
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IRolService<RolEntity> _rolService;
        private readonly IMapper _mapper;
        private readonly IRedisCache _redisCache;
        private readonly IAzureQueue _azureQueue;

        public RoleApplication(IRolService<RolEntity> rolService, IMapper mapper, IRedisCache redisCache, IAzureQueue azureQueue)
        {
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _azureQueue = azureQueue ?? throw new ArgumentNullException(nameof(azureQueue));
        }

        public async Task<ApiResponse<RolDto>> GetById(int id)
        {
            try
            {
                //tiene ejemplo de cola en azure, solo es de referencia.
                var responseApiCache = await _redisCache.GetCacheData<ApiResponse<RolDto>>($"{nameof(GetById)}{id}");
                if (responseApiCache != null)
                {
                    return responseApiCache;
                }

                var roleEntity = await _rolService.GetById(id);
                var roleDto = _mapper.Map<RolDto>(roleEntity);
                var apiresponse = new ApiResponse<RolDto>(200, AppMessages.Api_Get_Rol_Response, true, roleDto);
                await _azureQueue.EnqueueMessageAsync("emails", JsonConvert.SerializeObject(responseApiCache, Formatting.Indented));
                await _redisCache.SetCacheData($"{nameof(GetById)}{id}", apiresponse, DateTimeOffset.Now.AddMinutes(5.0));
                return apiresponse;
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_Rol_GetById_Error, ex);
            }
        }

        public async Task<ApiResponse<RolDto>> GetByName(string roleName)
        {
            try
            {
                var roleEntity = await _rolService.GetByName(roleName);
                var roleDto = _mapper.Map<RolDto>(roleEntity);
                return new ApiResponse<RolDto>(200, AppMessages.Api_Get_Rol_Response, true, roleDto);
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_Rol_GetByName_Error, ex);
            }
        }

        public async Task<ApiResponse<List<RolDto>>> SelectAsync()
        {
            try
            {
                var roleEntities = await _rolService.SelectAsync();
                var roleDtos = _mapper.Map<List<RolDto>>(roleEntities);
                return new ApiResponse<List<RolDto>>(200, AppMessages.Api_Get_Rol_Response, true, roleDtos);
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_Rol_Select_Error, ex);
            }
        }
    }
}
