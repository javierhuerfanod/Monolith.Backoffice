// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Infrasturcture
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="WeightRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Aggregates;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Infrastructure.Persistence;
namespace Juegos.Serios.Bathroom.Infrastructure.Repositories
{
    public class WeightRepository : RepositoryBase<Weight>, IWeightRepository
    {
        public WeightRepository(BdSqlBathroomContext context) : base(context)
        {

        }
    }
}
