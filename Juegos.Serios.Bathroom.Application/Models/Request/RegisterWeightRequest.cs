// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RegisterWeightRequest.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Juegos.Serios.Bathroom.Application.Models.Request;

public class RegisterWeightRequest
{
    [Required(ErrorMessage = "El peso es requerido")]
    [Range(1, int.MaxValue, ErrorMessage = "El peso debe ser mayor a 0")]
    public int Weight { get; set; }
}
