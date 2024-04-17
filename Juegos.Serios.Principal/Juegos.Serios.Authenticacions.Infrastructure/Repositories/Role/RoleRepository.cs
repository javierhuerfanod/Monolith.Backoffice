// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RoleRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Juegos.Serios.Authenticacions.Domain.Entities.Rol;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories.Role
{

    public class RoleRepository : RepositoryBase<RolEntity>, IRolRepository
    {
        public RoleRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }    
    }
}
