// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserBodyPartsRequest.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Juegos.Serios.Authenticacions.Application.Models.Request;

public class UserBodyPartsRequest
{
    [Required(ErrorMessage = "La lista de partes del cuerpo es obligatoria.")]
    [MinLength(4, ErrorMessage = "Debe haber exactamente cuatro partes del cuerpo.")]
    [MaxLength(4, ErrorMessage = "Debe haber exactamente cuatro partes del cuerpo.")]
    public List<BodyPartRequest> BodyParts { get; set; }
}

public class BodyPartRequest
{
    [Required(ErrorMessage = "El nombre de la parte del cuerpo es obligatorio.")]
    public string BodyPartName { get; set; }

    [Required(ErrorMessage = "El ID de animación de la parte del cuerpo es obligatorio.")]
    public int BodyPartAnimationId { get; set; }
}
