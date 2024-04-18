// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="SessionLog.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Aggregates;

namespace Juegos.Serios.Authenticacions.Domain.Entities;
public partial class SessionLogEntity
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string? Action { get; set; }

    public string? Ipaddress { get; set; }

    public DateTime? Timestamp { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual UserAggregate? CreatedByNavigation { get; set; }

    public virtual UserAggregate? UpdatedByNavigation { get; set; }

    public virtual UserAggregate User { get; set; } = null!;

}
