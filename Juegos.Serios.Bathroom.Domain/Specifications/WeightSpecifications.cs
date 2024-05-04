// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="WeightSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Bathroom.Domain.Aggregates;
using System.Linq.Expressions;

namespace Juegos.Serios.Bathroom.Domain.Specifications
{
    public class WeightSpecifications
    {
        public static Expression<Func<Weight, bool>> ByUserId(int userId)
        {
            return weight => weight.UserId == userId;
        }
    }
}