// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserAvatarBodyPartSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq.Expressions;
using Juegos.Serios.Authenticacions.Domain.Entities.UserAvatar;

namespace Juegos.Serios.Authenticacions.Domain.Specifications
{
    public class UserAvatarBodyPartSpecifications
    {
        public static Expression<Func<UserAvatarBodyPart, bool>> ById(int userAvatarBodyPartId)
        {
            return r => r.UserAvatarBodyPartsId == userAvatarBodyPartId;
        }
        public static Expression<Func<UserAvatarBodyPart, bool>> ByUserId(int userId)
        {
            return r => r.UserId == userId;
        }
    }
}