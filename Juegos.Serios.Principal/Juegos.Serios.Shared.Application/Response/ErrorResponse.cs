// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ErrorResponse.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.Application.Response
{
    public class ErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();

        public ErrorResponse()
        {
        }

        public ErrorResponse(string error)
        {
            Errors.Add(error);
        }

        public ErrorResponse(IEnumerable<string> errors)
        {
            Errors.AddRange(errors);
        }
    }

}
