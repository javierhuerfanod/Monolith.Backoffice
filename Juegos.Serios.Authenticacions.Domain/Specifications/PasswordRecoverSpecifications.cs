// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="PasswordRecoverSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq.Expressions;
using Juegos.Serios.Authenticacions.Domain.Entities.PasswordRecovery;

namespace Juegos.Serios.Authenticacions.Domain.Specifications
{
    public class PasswordRecoverSpecifications
    {
        public static Expression<Func<PasswordRecovery, bool>> ById(int recoveryId)
        {
            return r => r.RecoveryId == recoveryId;
        }
        public static Expression<Func<PasswordRecovery, bool>> ByUserId(int userId)
        {
            return r => r.UserId == userId;
        }
        public static Expression<Func<PasswordRecovery, bool>> ByUserIdAndNotExpired(int userId)
        {
            return recovery => recovery.UserId == userId && recovery.RecoveryPasswordExpiration > DateTime.UtcNow;
        }
    }
}