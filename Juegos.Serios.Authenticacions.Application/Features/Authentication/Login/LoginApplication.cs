﻿// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="LoginApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
using Juegos.Serios.Authenticacions.Application.Models.Request;
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Services;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Domain.Shared.Exceptions;
using Juegos.Serios.Shared.Application.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Juegos.Serios.Authentications.Application.Features.Login
{
    public class LoginApplication : ILoginApplication
    {
        private readonly IUserAggregateService<User> _userAggregateService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginApplication> _logger;

        public LoginApplication(ILogger<LoginApplication> logger, IUserAggregateService<User> userAggregateService, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAggregateService = userAggregateService ?? throw new ArgumentNullException(nameof(userAggregateService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ApiResponse<string>> GetLogin(LoginRequest loginRequest)
        {
            _logger.LogInformation("Attempting login for user: {Email}", loginRequest.Email);

            try
            {
                var user = await _userAggregateService.GetByEmailAndPassword(loginRequest.Email, loginRequest.Password);
                var token = GenerateJwtToken(user);
                return new ApiResponse<string>(200, AppMessages.Api_Successful, true, token);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Login failed: {ExceptionMessage}", dex.Message);
                return new ApiResponse<string>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error occurred: {ExceptionMessage}", ex.Message);
                return new ApiResponse<string>(500, AppMessages.Api_Servererror, false, null);
            }
        }
        public async Task<ApiResponse<string>> GetRefreshToken(int userId)
        {
            _logger.LogInformation("Attempting refresh token for user: {UserId}", userId);

            try
            {
                var user = await _userAggregateService.GetById(userId);
                var token = GenerateJwtToken(user);
                return new ApiResponse<string>(200, AppMessages.Api_Successful, true, token);
            }
            catch (DomainException dex)
            {
                _logger.LogWarning("Login failed: {ExceptionMessage}", dex.Message);
                return new ApiResponse<string>(400, dex.Message, false, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal server error occurred: {ExceptionMessage}", ex.Message);
                return new ApiResponse<string>(500, AppMessages.Api_Servererror, false, null);
            }
        }

        private string GenerateJwtToken(User user)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
#pragma warning restore CS8604 // Posible argumento de referencia nulo
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("user_id", user.UserId.ToString()),
        new Claim("Created_user", user.CreatedAt.ToString()),
        new Claim("Created_userName", user.FirstName.ToString()),
        new Claim("Created_userLastName", user.LastName.ToString()),
        new Claim("Created_userEmail", user.Email.ToString()),
        new Claim("user_weight", user.Weight.ToString())
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

