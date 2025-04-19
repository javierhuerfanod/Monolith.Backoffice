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
#pragma warning disable CS0114 // El miembro oculta el miembro heredado. Falta una contraseña de invalidación
        public T Data { get; }
#pragma warning restore CS0114 // El miembro oculta el miembro heredado. Falta una contraseña de invalidación

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
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
        public RoleApplicationException(string message) : base(message, null)
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
        {
        }

#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
        public RoleApplicationException(string message, Exception innerException) : base(message, innerException, null)
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
        {
        }
    }
}

