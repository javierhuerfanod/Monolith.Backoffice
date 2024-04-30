// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 29-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DataConsentRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities.DataConsent;
using Juegos.Serios.Authenticacions.Domain.Entities.DataConsent.Interfaces;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories.UserRepository
{
    public class DataConsentRepository : RepositoryBase<DataConsent>, IDataConsentRepository
    {
        public DataConsentRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
    }
}
