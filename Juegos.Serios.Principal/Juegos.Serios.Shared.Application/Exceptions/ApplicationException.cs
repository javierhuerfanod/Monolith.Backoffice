// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ApplicationException.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.Application.Exceptions
{
    public class ApplicationException<T> : Exception
    {
        public T Data { get; }

        public ApplicationException(string message, T data) : base(message)
        {
            Data = data;
        }

        public ApplicationException(string message, Exception innerException, T data) : base(message, innerException)
        {
            Data = data;
        }
    }

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

