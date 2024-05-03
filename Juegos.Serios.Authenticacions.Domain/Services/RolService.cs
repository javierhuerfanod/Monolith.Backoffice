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


using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Specifications;
using Juegos.Serios.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;


namespace Juegos.Serios.Authentications.Domain.Services
{
    public sealed class RolService : IRolService<Role>
    {
        private readonly IRolRepository _roleRepository;
        private readonly ILogger<RolService> _logger; // Agrega el logger

        public RolService(IRolRepository roleRepository, ILogger<RolService> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Role> GetById(int id)
        {
            try
            {
                return await _roleRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by ID");
                throw new DomainException("Error retrieving role by ID.", ex);
            }
        }

        public async Task<Role> GetByName(string roleName)
        {
            try
            {
                return await _roleRepository.GetOneAsync(RolSpecifications.ByName(roleName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by Name");
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
                _logger.LogError(ex, "Error retrieving all roles");
                throw new DomainException("Error retrieving all roles.", ex);
            }
        }

        public async Task<Role> CreateRoleAsync(string roleName)
        {
            try
            {
                Role newRole = new()
                {
                    RoleName = roleName
                };

                return await _roleRepository.AddAsync(newRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                throw new DomainException("Error creating role.", ex);
            }
        }
    }
}

