// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ValidateWeightJwtModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Domain.Models.Weight;

public class ValidateWeightJwtModel
{
    public int UserId { get; set; }
    public int? WeightCreatedInRegister { get; set; }  
    public DateTime CreatedUser { get; set; }         
}

