// ***********************************************************************
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

using Juegos.Serios.Shared.Application.Response;
using Juegos.Serios.Authenticacions.Domain.Resources;
using Juegos.Serios.Authenticacions.Application.Features.Authentication.Login.Interfaces;
using Juegos.Serios.Authenticacions.Application.Models.Request;
using Juegos.Serios.Authenticacions.Domain.Aggregates.Interfaces;
using Juegos.Serios.Authenticacions.Domain.Aggregates;
using Juegos.Serios.Domain.Shared.Exceptions;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Juegos.Serios.Authentications.Application.Features.Login
{
    public class LoginApplication : ILoginApplication
    {
        private readonly IUserAggregateService<User> _userAggregateService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginApplication> _logger; // Instancia del logger

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

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username), 
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("user_id", user.UserId.ToString()) 
    };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

