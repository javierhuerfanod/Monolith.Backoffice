// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ApiResponse.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.Application.Response
{
    public class ApiResponse<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int responseCode, string message, bool status, T? data = default)
        {
            ResponseCode = responseCode;
            Message = message;
            Status = status;
            Data = data;
        }          

    }
}
