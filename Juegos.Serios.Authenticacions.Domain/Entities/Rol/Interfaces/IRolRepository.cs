// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IRoleFinder.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Ports.Persistence;
namespace Juegos.Serios.Authenticacions.Domain.Entities.Rol.Interfaces
{
    public interface IRolRepository : IAsyncRepository<Role>
    {
        
    }
}