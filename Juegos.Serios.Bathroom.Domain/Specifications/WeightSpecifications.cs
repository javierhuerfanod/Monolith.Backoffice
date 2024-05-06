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
        public static Expression<Func<Weight, bool>> ByWeightId(int weightId)
        {
            return weight => weight.WeightId == weightId;
        }
        public static Expression<Func<Weight, bool>> BySearchTermAndDateRange(string searchTerm, DateOnly? startDate, DateOnly? endDate)
        {
            int? weightFilter = null;
            if (int.TryParse(searchTerm, out int numericValue))
            {
                weightFilter = numericValue;
            }

            return weight =>
                (!weightFilter.HasValue || weight.Weight1 == weightFilter) &&
                (!startDate.HasValue || weight.Date >= startDate) &&
                (!endDate.HasValue || weight.Date <= endDate);
        }
    }
}