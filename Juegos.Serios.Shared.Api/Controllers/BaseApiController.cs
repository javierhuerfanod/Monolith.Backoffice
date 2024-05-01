// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Api
// Author           : diego diaz
// Created          : 22-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="BaseApiController.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.Api.Controllers
{
    using Juegos.Serios.Shared.Application.Response;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Net;
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly ILogger<BaseApiController> _logger;
        protected readonly IConfiguration _configuration; 
    
        protected BaseApiController(ILogger<BaseApiController> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }
   
        protected BaseApiController(ILogger<BaseApiController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        protected bool ValidateTokenApplication()
        {
            var validApplicationToken = _configuration["ApplicationToken"]!.ToString();
            var incomingToken = Request.Headers["Application-Token"].FirstOrDefault();

            _logger.LogInformation("Attempting token validation for access.");

            if (incomingToken != validApplicationToken)
            {
                _logger.LogWarning("Unauthorized access attempt with invalid token.");
                return false;
            }

            return true;
        }

        protected ActionResult<ApiResponse<T>> LogAndReturnOk<T>(ApiResponse<T> response, string message = null)
        {
            _logger.LogInformation("Request successful: {Message}", message ?? "Success");
            return Ok(response);
        }

        protected ActionResult<ApiResponse<T>> LogAndReturnBadRequest<T>(ApiResponse<T> response)
        {
            _logger.LogWarning("Bad request: {Reason}", response.Message);
            return BadRequest(response);
        }

        protected ActionResult<ApiResponse<T>> LogAndReturnInternalError<T>(ApiResponse<T> response, string message = null)
        {
            _logger.LogError("Internal server error: {Message}", message ?? "Error occurred");
            return StatusCode((int)HttpStatusCode.InternalServerError, response);
        }

        protected void ThrowForUnexpectedCode(int responseCode)
        {
            _logger.LogError("Unexpected response code {ResponseCode} received", responseCode);
            throw new NotImplementedException();
        }
    }

}
