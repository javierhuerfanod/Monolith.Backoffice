// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Api
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="AuthenticationController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Authenticacions.Api.V1
{
    using Juegos.Serios.Authenticacions.Application.Models.Dtos;
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using Aurora.Backend.Baseline.Application.Constants;
    using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
    using Juegos.Serios.Authenticacions.Application.Models.Request;
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginApplication _loginApplication;

        public AuthenticationController(ILoginApplication loginApplication)
        {
            _loginApplication = loginApplication ?? throw new ArgumentNullException(nameof(loginApplication));
        }
        /// <summary>
        /// Realiza el inicio de sesión basado en las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginRequest">Los datos de autenticación que incluyen el correo electrónico y la contraseña.</param>
        /// <returns>Una respuesta que contiene los detalles del rol del usuario si el inicio de sesión es exitoso.</returns>
        /// <response code="200">Devuelve el código 200 junto con los detalles del rol del usuario si el inicio de sesión es exitoso.</response>
        /// <response code="400">Devuelve el código 400 si los datos de la solicitud no son válidos.</response>      
        /// <response code="500">Devuelve el código 500 en caso de un error interno del servidor.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<RolDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]     
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]

        public async Task<ActionResult<ApiResponse<RolDto>>> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }
            var response = await _loginApplication.GetLogin(loginRequest);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => Ok(response),
                (int)GenericEnumerator.ResponseCode.BadRequest => BadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                _ => throw new NotImplementedException()
            };
        }
       
    }
}
