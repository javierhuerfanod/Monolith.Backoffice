// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Application.Tests
// Author           : diego diaz
// Created          : 27-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="ApiResponseTests.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary>Defines unit tests for the ApiResponse class.</summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.Application.Tests.Response
{
    using Juegos.Serios.Shared.Application.Response;
    using Xunit;

    public class ApiResponseTests
    {
        [Fact]
        public void Constructor_Sets_Properties_Correctly()
        {
            // Arrange
            int expectedResponseCode = 200;
            string expectedMessage = "Success";
            bool expectedStatus = true;
            string expectedData = "Data here";

            // Act
            var apiResponse = new ApiResponse<string>(expectedResponseCode, expectedMessage, expectedStatus, expectedData);

            // Assert
            Assert.Equal(expectedResponseCode, apiResponse.ResponseCode);
            Assert.Equal(expectedMessage, apiResponse.Message);
            Assert.Equal(expectedStatus, apiResponse.Status);
            Assert.Equal(expectedData, apiResponse.Data);
        }

        [Fact]
        public void Constructor_Handles_Null_Data_Correctly()
        {
            // Arrange
            int expectedResponseCode = 404;
            string expectedMessage = "Not Found";
            bool expectedStatus = false;

            // Act
            var apiResponse = new ApiResponse<string>(expectedResponseCode, expectedMessage, expectedStatus);

            // Assert
            Assert.Equal(expectedResponseCode, apiResponse.ResponseCode);
            Assert.Equal(expectedMessage, apiResponse.Message);
            Assert.Equal(expectedStatus, apiResponse.Status);
            Assert.Null(apiResponse.Data);  // Data should be null if not provided
        }
    }
}

