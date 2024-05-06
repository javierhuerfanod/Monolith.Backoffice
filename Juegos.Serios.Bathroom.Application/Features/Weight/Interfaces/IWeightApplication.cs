// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IWeightApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Juegos.Serios.Bathroom.Application.Models.Request;
using Juegos.Serios.Bathroom.Application.Models.Response;
using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;
using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Shared.Domain.Models;

namespace Juegos.Serios.Bathroom.Application.Features.Weight.Interfaces
{
    public interface IWeightApplication
    {
        Task<ApiResponse<object>> ValidateWeight(int userId, int weightCreatedInRegister, DateTime createdUser);
        Task<ApiResponse<RegisterWeightResponse>> RegisterWeight(RegisterWeightRequest registerWeightRequest, int userId, string name, string lastName, string email);
        Task<ApiResponse<PaginatedList<WeightDto>>> SearchWeights(string searchTerm, string startDate, string endDate, int pageNumber, int pageSize);
    }
}

