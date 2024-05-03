// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Domain
// Author           : diego diaz
// Created          : 02-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="BaseDomainModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.Domain.Common
{
    public abstract class BaseDomainModel
    {

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

    }
}