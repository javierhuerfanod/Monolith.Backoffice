// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="CitySpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Entities;
using System.Linq.Expressions;

namespace Juegos.Serios.Authenticacions.Domain.Specifications
{
    public class CitySpecifications
    {
        public static Expression<Func<City, bool>> ById(int documentTypeId)
        {
            return r => r.CityId == documentTypeId;
        }
        public static Expression<Func<City, object>> OrderByCityName()
        {
            return city => city.CityName;
        }
    }
}