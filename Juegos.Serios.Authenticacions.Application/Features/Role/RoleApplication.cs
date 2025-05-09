﻿// ***********************************************************************
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
using Juegos.Serios.Authenticacions.Application.Features.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Application.Models.Response;
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Shared.AzureQueue.Interfaces;
using Juegos.Serios.Shared.RedisCache.Interfaces;

namespace Juegos.Serios.Authenticacions.Application.Features.Rol
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IRolService<Role> _rolService;
        private readonly IMapper _mapper;
        private readonly IRedisCache _redisCache;
        private readonly IAzureQueue _azureQueue;

        public RoleApplication(IRolService<Role> rolService, IMapper mapper, IRedisCache redisCache, IAzureQueue azureQueue)
        {
            _rolService = rolService ?? throw new ArgumentNullException(nameof(rolService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _azureQueue = azureQueue ?? throw new ArgumentNullException(nameof(azureQueue));
        }

        public async Task<ApiResponse<RolResponse>> GetById(int id)
        {
            try
            {
                //tiene ejemplo de cola en azure, solo es de referencia.
                var responseApiCache = await _redisCache.GetCacheData<ApiResponse<RolResponse>>($"{nameof(GetById)}{id}");
                if (responseApiCache != null)
                {
                    return responseApiCache;
                }

                var roleEntity = await _rolService.GetById(id);
                if (roleEntity == null)
                {
                    return new ApiResponse<RolResponse>(204, AppMessages.Api_Get_Rol_Response, true, null);
                }
                var roleDto = _mapper.Map<RolResponse>(roleEntity);
                var apiresponse = new ApiResponse<RolResponse>(200, AppMessages.Api_Get_Rol_Response, true, roleDto);
                await _redisCache.SetCacheData($"{nameof(GetById)}{id}", apiresponse, DateTimeOffset.Now.AddMinutes(5.0));
                return apiresponse;
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_Rol_GetById_Error, ex);
            }
        }

        public async Task<ApiResponse<RolResponse>> CreateRol(string rolename)
        {
            try
            {
                var roleFindEntity = await _rolService.GetByName(rolename);
                if (roleFindEntity != null)
                {
                    return new ApiResponse<RolResponse>(400, AppMessages.Api_Get_Rol_Duplicated_Response, false, null);
                }
                var roleEntity = await _rolService.CreateRoleAsync(rolename);
                var roleDto = _mapper.Map<RolResponse>(roleEntity);
                var apiresponse = new ApiResponse<RolResponse>(200, AppMessages.Api_Get_Rol_Created_Response, true, roleDto);
                return apiresponse;
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_Rol_GetById_Error, ex);
            }
        }

        public async Task<ApiResponse<List<RolResponse>>> SelectAsync()
        {
            try
            {
                var roleEntities = await _rolService.SelectAsync();
                var roleDtos = _mapper.Map<List<RolResponse>>(roleEntities);
                return new ApiResponse<List<RolResponse>>(200, AppMessages.Api_Get_Rol_Response, true, roleDtos);
            }
            catch (Exception ex)
            {
                throw new RoleApplicationException(AppMessages.Api_Rol_Select_Error, ex);
            }
        }
    }
}
