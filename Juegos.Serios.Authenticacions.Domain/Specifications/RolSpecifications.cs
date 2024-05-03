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
using Juegos.Serios.Authenticacions.Domain.Entities;
using System.Linq.Expressions;

namespace Juegos.Serios.Authenticacions.Domain.Specifications
{
    public class RolSpecifications
    {
        public static Expression<Func<Role, bool>> ById(int roleId)
        {
            return r => r.RoleId == roleId;
        }
        public static Expression<Func<Role, bool>> ByName(string roleName)
        {
            return r => r.RoleName == roleName;
        }
    }
}