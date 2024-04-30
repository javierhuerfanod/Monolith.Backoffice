// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Api
// Author           : diego diaz
// Created          : 22-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ModelStateDictionaryExtensions.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Juegos.Serios.Shared.Api.Controllers
{
    public static class ModelStateDictionaryExtensions
    {
        public static IEnumerable<string> GetAllErrorMessages(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
        }
    }
}
