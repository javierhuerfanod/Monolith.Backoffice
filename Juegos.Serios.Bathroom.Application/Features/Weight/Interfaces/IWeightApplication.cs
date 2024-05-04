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


using Juegos.Serios.Shared.Application.Response;

namespace Juegos.Serios.Bathroom.Application.Features.Weight.Interfaces
{
    public interface IWeightApplication
    {
        Task<ApiResponse<object>> ValidateWeight(int userId, int weightCreatedInRegister, DateTime createdUser);
    }
}

