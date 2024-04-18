// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Api
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RolController.cs" company="Universidad Javeriana">
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
    using Juegos.Serios.Authenticacions.Application.Features.Role.Interfaces;
    using Aurora.Backend.Baseline.Application.Constants;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRoleApplication _roleApplication;

        public RolController(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RolDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<RolDto>>> GetRolById(int id)
        {

            var response = await _roleApplication.GetById(id);
            return response.ResponseCode switch
            {
                (int)GenericEnumerator.ResponseCode.Ok => Ok(response),
                (int)GenericEnumerator.ResponseCode.NoContent => NoContent(),
                (int)GenericEnumerator.ResponseCode.BadRequest => BadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                _ => throw new NotImplementedException()
            };
        }
    }
}
