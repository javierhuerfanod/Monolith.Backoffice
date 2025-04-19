// ***********************************************************************
// Assembly         : Juegos.Serios.Domain.Shared.Tests
// Author           : diego diaz
// Created          : 22-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DomainExceptionHandlerTests.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Domain.Shared.Tests
{
    using Juegos.Serios.Domain.Shared.Exceptions;
    using Xunit;
    public class DomainExceptionHandlerTests
    {
        [Fact]
        public async Task HandleAsync_Returns_Result_If_No_Exception_Thrown()
        {
            // Arrange
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
            Func<Task<string>> func = async () => "Success";
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica

            // Act
            var result = await DomainExceptionHandler.HandleAsync(func);

            // Assert
            Assert.Equal("Success", result);
        }

        [Fact]
        public async Task HandleAsync_Throws_NewException_On_NotFoundException()
        {
            // Arrange
#pragma warning disable CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.
            Func<Task<string>> func = () => throw new NotFoundException("Item not found", null);
#pragma warning restore CS8625 // No se puede convertir un literal NULL en un tipo de referencia que no acepta valores NULL.

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => DomainExceptionHandler.HandleAsync(func));
            Assert.Equal("Resource not found.", exception.Message);
            Assert.IsType<NotFoundException>(exception.InnerException);
        }


        [Fact]
        public async Task HandleAsync_Throws_NewException_On_ValidationException()
        {
            // Arrange
            Func<Task<string>> func = () => throw new ValidationException("Validation failed");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => DomainExceptionHandler.HandleAsync(func));
            Assert.Equal("Validation failed.", exception.Message);
            Assert.IsType<ValidationException>(exception.InnerException);
        }

        [Fact]
        public async Task HandleAsync_Throws_NewException_On_GenericException()
        {
            // Arrange
            Func<Task<string>> func = () => throw new Exception("Error occurred");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => DomainExceptionHandler.HandleAsync(func));
            Assert.Equal("An unexpected error occurred.", exception.Message);
            Assert.IsType<Exception>(exception.InnerException);
        }
    }
}


