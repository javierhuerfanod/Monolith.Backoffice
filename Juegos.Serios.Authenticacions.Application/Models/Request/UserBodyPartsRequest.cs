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
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public List<BodyPartRequest> BodyParts { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
}

public class BodyPartRequest
{
    [Required(ErrorMessage = "El nombre de la parte del cuerpo es obligatorio.")]
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public string BodyPartName { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

    [Required(ErrorMessage = "El ID de animación de la parte del cuerpo es obligatorio.")]
    public int BodyPartAnimationId { get; set; }
}
