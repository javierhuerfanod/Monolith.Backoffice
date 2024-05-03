// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UpdatePasswordRequest.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Juegos.Serios.Authenticacions.Application.Models.Request;

public class UpdatePasswordRequest
{
    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña debe tener al menos 5 caracteres de longitud.", MinimumLength = 5)]
    [RegularExpression(@"^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{5,100}$", ErrorMessage = "La contraseña debe contener al menos un número y un carácter especial.")]
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
    public string NewPassword { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.

}
