// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="PasswordRecoveryRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery.Interfaces;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories.UserRepository
{
    public class PasswordRecoveryRepository : RepositoryBase<PasswordRecovery>, IPasswordRecoveryRepository
    {
        public PasswordRecoveryRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
    }
}
