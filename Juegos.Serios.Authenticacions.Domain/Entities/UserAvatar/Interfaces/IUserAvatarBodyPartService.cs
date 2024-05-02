// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IUserAvatarBodyPartService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Models.UserAvatarBodyParts;

namespace Juegos.Serios.Authenticacions.Domain.Entities.UserAvatar.Interfaces
{
    public interface IUserAvatarBodyPartService<T>
    {
        Task<List<UserAvatarBodyPart>> GetByUserId(int id);
        Task<bool> RegisterUserAvatarBodyParts(UserBodyPartsModel userBodyPartsModel);
    }
}