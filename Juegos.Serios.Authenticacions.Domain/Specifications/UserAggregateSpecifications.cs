// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserAggregateSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using System.Linq.Expressions;

namespace Juegos.Serios.Authenticacions.Domain.Specifications
{
    public class UserAggregateSpecifications
    {

        public static Expression<Func<User, bool>> ByUsernameDocumentNumberOrEmail(string username, string documentNumber, string email)
        {
            return user => user.Username == username || user.DocumentNumber == documentNumber || user.Email == email;
        }
        public static Expression<Func<User, bool>> ByEmail(string email)
        {
            return user => user.Email == email;
        }
        public static Expression<Func<User, bool>> ById(int userId)
        {
            return user => user.UserId == userId;
        }

    }
}