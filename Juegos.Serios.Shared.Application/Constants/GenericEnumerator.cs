// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="GenericEnumerator.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Aurora.Backend.Baseline.Application.Constants
{
    public class GenericEnumerator
    {
        public enum Status
        {
            successful,
            failed,
            notfound
        }
        public enum ResponseCode
        {
            Ok = 200,
            BadRequest = 400,
            NoContent = 204,
            InternalError = 500,
            Unauthorized = 401
        }
    }
}

