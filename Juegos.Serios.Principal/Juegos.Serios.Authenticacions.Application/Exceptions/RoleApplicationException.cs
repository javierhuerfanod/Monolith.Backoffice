// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RoleApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Shared.Application.Exceptions;

namespace Juegos.Serios.Authenticacions.Application.Exceptions
{
    public class RoleApplicationException : ApplicationException<object>
    {
        public RoleApplicationException(string message) : base(message, null)
        {
        }

        public RoleApplicationException(string message, Exception innerException) : base(message, innerException, null)
        {
        }
    }
}
