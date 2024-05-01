// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="CityRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities.City;
using Juegos.Serios.Authenticacions.Domain.Entities.City.Interfaces;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories.CityRepository
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
    }
}
