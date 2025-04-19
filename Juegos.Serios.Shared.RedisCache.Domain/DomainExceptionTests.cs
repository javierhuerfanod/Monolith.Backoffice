// ***********************************************************************
// Assembly         : Juegos.Serios.Domain.Shared.Tests
// Author           : diego diaz
// Created          : 22-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DomainExceptionTests.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Domain.Shared.Tests
{
    using Juegos.Serios.Domain.Shared.Exceptions;
    using Xunit;

    public class DomainExceptionTests
    {
        // Tests for DomainException
        [Fact]
        public void DomainException_WithOnlyMessage_SetsMessageCorrectly()
        {
            // Arrange & Act
            var exception = new DomainException("Test message");

            // Assert
            Assert.Equal("Test message", exception.Message);
        }

        [Fact]
        public void DomainException_WithMessageAndInnerException_SetsPropertiesCorrectly()
        {
            // Arrange
            var innerException = new InvalidOperationException("Invalid operation");

            // Act
            var exception = new DomainException("Test message", innerException);

            // Assert
            Assert.Equal("Test message", exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        // Tests for NotFoundException
        [Fact]
        public void NotFoundException_CreatesCorrectMessage()
        {
            // Arrange
            string name = "User";
            int key = 404;

            // Act
            var exception = new NotFoundException(name, key);

            // Assert
            Assert.Equal($"Entity \"User\" (404) was not found.", exception.Message);
        }

        // Tests for ValidationException
        [Fact]
        public void ValidationException_SetsMessageCorrectly()
        {
            // Arrange & Act
            var exception = new ValidationException("Validation failed");

            // Assert
            Assert.Equal("Validation failed", exception.Message);
        }
    }
}



