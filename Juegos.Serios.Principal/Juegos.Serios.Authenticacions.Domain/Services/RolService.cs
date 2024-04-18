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
using Juegos.Serios.Authenticacions.Domain.Specifications.Rol;
using Juegos.Serios.Domain.Shared.Exceptions;


namespace Juegos.Serios.Authenticacions.Domain.Services
{
    public sealed class RolService : IRolService<RolEntity>
    {
        private readonly IRolRepository _roleRepository;

        public RolService(IRolRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RolEntity> GetById(int id)
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

        public async Task<RolEntity> GetByName(string rolename)
        {
            try
            {
                return (RolEntity)await DomainExceptionHandler.HandleAsync(() =>
                    _roleRepository.GetAsync(RolSpecifications.ByName(rolename)));
            }
            catch (NotFoundException)
            {                
                throw;
            }
            catch (Exception ex)
            {               
                throw new DomainException($"Error retrieving role by name: {rolename}", ex);
            }
        }

        public async Task<List<RolEntity>> SelectAsync()
        {
            try
            {
                return (List<RolEntity>)await DomainExceptionHandler.HandleAsync(() =>
                    _roleRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                throw new DomainException("Error retrieving all roles.", ex);
            }
        }
    }
}

