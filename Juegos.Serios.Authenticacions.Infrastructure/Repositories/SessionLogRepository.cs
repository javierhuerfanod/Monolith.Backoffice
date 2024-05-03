// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 29-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="SessionLogRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories
{
    public class SessionLogRepository : RepositoryBase<SessionLog>, ISessionLogRepository
    {
        public SessionLogRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
    }
}
