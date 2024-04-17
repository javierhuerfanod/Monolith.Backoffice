// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DocumentType.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Specifications.Rol;

namespace Juegos.Serios.Authenticacions.Domain.Services
{
    public sealed class RoleService : IRolService<RolEntity>
    {
        private readonly IRolRepository _roleRepository;
        public RoleService(IRolRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RolEntity> GetById(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<RolEntity> GetByName(string rolename)
        {
            return (RolEntity)await _roleRepository.GetAsync(RolSpecifications.ByName(rolename));
        }

        public async Task<List<RolEntity>> SelectAsync()
        {
            return (List<RolEntity>)await _roleRepository.GetAllAsync();
        }
    }
}
