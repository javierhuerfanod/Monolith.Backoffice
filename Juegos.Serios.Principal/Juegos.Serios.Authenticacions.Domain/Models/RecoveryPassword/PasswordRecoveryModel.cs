// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 28-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="PasswordRecoveryModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Domain.Models.RecoveryPassword;

public class PasswordRecoveryModel
{
    public byte[] PasswordHash { get; set; }
    public string Password { get; set; }
    public DateTime Expiration { get; set; }
}

