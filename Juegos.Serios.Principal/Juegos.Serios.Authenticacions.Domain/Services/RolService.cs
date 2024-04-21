// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By :
// Last Modified On :
// ***********************************************************************
// <copyright file="RoleService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Implements the role service.</summary>
// ***********************************************************************


using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;


namespace Juegos.Serios.Authenticacions.Domain.Services
{
    public sealed class RolService : IRolService<Role>
    {
        private readonly IRolRepository _roleRepository;

        public RolService(IRolRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetById(int id)
        {
            try
            {
                return await _roleRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new DomainException("Error retrieving role by ID.", ex);
            }
        }
        public async Task<Role> GetByName(string rolename)
        {
            try
            {
                return await _roleRepository.GetOneAsync(RolSpecifications.ByName(rolename));             
            }
            catch (Exception ex)
            {
                throw new DomainException("Error retrieving role by Name.", ex);
            }
        }

        public async Task<List<Role>> SelectAsync()
        {
            try
            {
                return (List<Role>)await DomainExceptionHandler.HandleAsync(() =>
                    _roleRepository.ListAllAsync());
            }
            catch (Exception ex)
            {
                throw new DomainException("Error retrieving all roles.", ex);
            }
        }
        public async Task<Role> CreateRoleAsync(string roleName)
        {
            Role newRole = new()
            {
                RoleName = roleName
            };

            return await _roleRepository.AddAsync(newRole);
        }
    }
}

