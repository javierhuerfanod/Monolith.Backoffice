// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="PasswordRecovery.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Aggregates;

namespace Juegos.Serios.Authenticacions.Domain.Entities;

public partial class PasswordRecovery
{
    public int RecoveryId { get; set; }

    public int UserId { get; set; }

    public byte[] RecoveryPassword { get; set; } = null!;

    public DateTime RecoveryPasswordExpiration { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual UserAggregate? CreatedByNavigation { get; set; }

    public virtual UserAggregate? UpdatedByNavigation { get; set; }

    public virtual UserAggregate User { get; set; } = null!;
}
