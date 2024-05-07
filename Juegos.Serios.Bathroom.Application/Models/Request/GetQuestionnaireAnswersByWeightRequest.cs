// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 06-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="GetQuestionnaireAnswersByWeight.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Juegos.Serios.Bathroom.Application.Models.Request;

public class GetQuestionnaireAnswersByWeightRequest
{
    [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "El ID del peso es obligatorio.")]
    public int WeightID { get; set; }
}

