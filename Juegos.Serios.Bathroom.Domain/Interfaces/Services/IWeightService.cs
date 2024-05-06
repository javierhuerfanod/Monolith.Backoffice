// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IWeightService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Models.Weight;
using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;
using Juegos.Serios.Shared.Domain.Models;

namespace Juegos.Serios.Bathroom.Domain.Interfaces.Services
{
    public interface IWeightService<T>
    {
        Task<bool> ValidateWeight(ValidateWeightJwtModel validateWeightJwtModel);
        Task<DomainRegisterWeightResponse> RegisterWeight(RegisterWeightModel registerWeightModel);
        Task<PaginatedList<WeightDto>> SearchWeights(string searchTerm, DateOnly? startDate, DateOnly? endDate, int pageNumber, int pageSize);
    }
}