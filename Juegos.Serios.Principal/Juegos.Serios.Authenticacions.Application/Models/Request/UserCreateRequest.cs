// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="AddUserRequest.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Juegos.Serios.Authenticacions.Application.Models.Dtos;

public class UserCreateRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public int DocumentTypeId { get; set; }

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    public string DocumentNumber { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El identificador de rol es obligatorio.")]
    public int RoleId { get; set; }
}
