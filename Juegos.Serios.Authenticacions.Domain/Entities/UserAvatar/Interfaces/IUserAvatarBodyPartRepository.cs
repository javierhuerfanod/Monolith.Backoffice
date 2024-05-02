// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IUserAvatarBodyPartRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Domain.Entities.UserAvatar;
using Juegos.Serios.Authenticacions.Domain.Ports.Persistence;
namespace Juegos.Serios.Authenticacions.Domain.Entities.SessionLog.Interfaces
{
    public interface IUserAvatarBodyPartRepository : IAsyncRepository<UserAvatarBodyPart>
    {

    }
}