// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserAvatarBodyPartRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories
{
    public class UserAvatarBodyPartRepository : RepositoryBase<UserAvatarBodyPart>, IUserAvatarBodyPartRepository
    {
        public UserAvatarBodyPartRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
    }
}
