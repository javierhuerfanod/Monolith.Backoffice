﻿// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IRecoveryPasswordAuthenticationApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Shared.Application.Response;

namespace Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces
{
    public interface IRecoveryPasswordAuthenticationApplication
    {
        Task<ApiResponse<object>> CreateRecoveryPassword(string email);
    }
}

