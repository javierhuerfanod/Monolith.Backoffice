// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RolSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq.Expressions;
using Juegos.Serios.Authenticacions.Domain.Entities.Rol;

namespace Juegos.Serios.Authenticacions.Domain.Specifications.Rol
{
    public class RolSpecifications
    {
        public static Expression<Func<RolEntity, bool>> ByName(string roleName)
        {
            return r => r.RoleName == roleName;
        }
    }
}