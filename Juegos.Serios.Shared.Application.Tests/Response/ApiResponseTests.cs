// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Application.Tests
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ErrorResponseTests.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Defines unit tests for the ErrorResponse class.</summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.Application.Tests.Response
{
    using Juegos.Serios.Shared.Application.Response;
    using Xunit;

    public class ErrorResponseTests
    {
        [Fact]
        public void Default_Constructor_Initializes_Empty_Error_List()
        {
            // Arrange & Act
            var errorResponse = new ErrorResponse();

            // Assert
            Assert.NotNull(errorResponse.Errors);
            Assert.Empty(errorResponse.Errors);
        }

        [Fact]
        public void Constructor_With_Single_Error_Initializes_Correctly()
        {
            // Arrange
            string error = "Error message";

            // Act
            var errorResponse = new ErrorResponse(error);

            // Assert
            Assert.Single(errorResponse.Errors);
            Assert.Contains(error, errorResponse.Errors);
        }

        [Fact]
        public void Constructor_With_Multiple_Errors_Initializes_Correctly()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

            // Act
            var errorResponse = new ErrorResponse(errors);

            // Assert
            Assert.Equal(3, errorResponse.Errors.Count);
            Assert.Equal(errors, errorResponse.Errors);
        }
    }
}
