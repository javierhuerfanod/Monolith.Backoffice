// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DocumentType.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Common;

public partial class DocumentTypeEntity : BaseDomainModel
{
    public int DocumentTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<UserAggregate> Users { get; set; } = new List<UserAggregate>();
}
