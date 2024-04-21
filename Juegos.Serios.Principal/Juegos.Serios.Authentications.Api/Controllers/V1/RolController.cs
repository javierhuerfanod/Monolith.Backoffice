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
    using Juegos.Serios.Authenticacions.Application.Features.Rol.Interfaces;
    using Aurora.Backend.Baseline.Application.Constants; 
    using Juegos.Serios.Authenticacions.Domain.Resources;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/v1/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly IRoleApplication _roleApplication;

        public RolController(IRoleApplication roleApplication)
        {
            _roleApplication = roleApplication;
        }
        /// <summary>
        /// Obtiene los detalles de un rol específico por su ID.
        /// </summary>
        /// <param name="id">El identificador único del rol a buscar.</param>
        /// <returns>Una respuesta con los detalles del rol si se encuentra, o un mensaje de error si no se encuentra.</returns>
        /// <response code="200">Si el rol se encuentra, devuelve el código 200 junto con los detalles del rol.</response>
        /// <response code="400">Si la solicitud es inválida, por ejemplo, si el ID no es válido, devuelve el código 400.</response>
        /// <response code="404">Si no se encuentra un rol con el ID especificado, devuelve el código 404.</response>
        /// <response code="500">Si ocurre un error interno en el servidor mientras se procesa la solicitud, devuelve el código 500.</response>
        [Authorize]
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
                (int)GenericEnumerator.ResponseCode.NoContent => NotFound(AppMessages.Api_Rol_GetById_NotFound),
                (int)GenericEnumerator.ResponseCode.BadRequest => BadRequest(response),
                (int)GenericEnumerator.ResponseCode.InternalError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                _ => throw new NotImplementedException()
            };
        }
        /// <summary>
        /// Crea un nuevo rol en el sistema.
        /// </summary>
        /// <param name="roleName">El nombre del rol que se desea crear.</param>
        /// <returns>Una respuesta con el resultado de la operación.</returns>
        /// <remarks>
        /// POST: /api/roles
        /// Este endpoint permite crear un nuevo rol en la base de datos. El nombre del rol es obligatorio.
        /// La respuesta incluye el estado de la operación y, si es exitosa, el objeto RolDto que representa el rol creado.
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RolDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.BadRequest)]       
        [ProducesResponseType(typeof(ApiResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApiResponse<RolDto>>> CreateRol(string roleName)
        {

            var response = await _roleApplication.CreateRol(roleName);
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
