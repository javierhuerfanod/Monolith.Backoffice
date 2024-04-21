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
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserApplication _userApplication;

        public UserController(IUserApplication userApplication)
        {
            _userApplication = userApplication ?? throw new ArgumentNullException(nameof(userApplication));
        }
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="userCreateRequest">Datos del usuario a registrar.</param>
        /// <returns>Un resultado de acción que puede incluir varios tipos de respuestas HTTP basadas en el resultado del proceso de creación.</returns>
        /// <remarks>
        /// Este método valida la solicitud del cliente, intenta registrar un nuevo usuario usando los datos proporcionados y
        /// maneja la respuesta según el resultado de la operación.
        /// 
        /// POST: /api/users/register
        /// </remarks>
        [Authorize]
        [HttpPost()]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<ErrorResponse>), (int)HttpStatusCode.BadRequest)]     
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]

        public async Task<ActionResult<ApiResponse<object>>> RegisterUser([FromBody] UserCreateRequest userCreateRequest)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                var errorResponse = new ErrorResponse(errorMessages.ToList());
                return BadRequest(new ApiResponse<ErrorResponse>(400, AppMessages.Api_Badrequest, false, errorResponse));
            }
            var response = await _userApplication.CreateUser(userCreateRequest);
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
