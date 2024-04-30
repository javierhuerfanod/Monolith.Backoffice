// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 29-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DataConsent.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Common;

namespace Juegos.Serios.Authenticacions.Domain.Entities.DataConsent
{
    public partial class DataConsent : BaseDomainModel
    {
        public int ConsentId { get; set; }

        public int UserId { get; set; }

        public DateTime ConsentDate { get; set; }

        public bool ConsentStatus { get; set; }

        public virtual User CreatedByNavigation { get; set; } = null!;

        public virtual User? UpdatedByNavigation { get; set; }

        public virtual User User { get; set; } = null!;
    }
}


