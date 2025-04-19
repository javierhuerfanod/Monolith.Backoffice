// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.RedisCache.Tests
// Author           : diego diaz
// Created          : 22-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RedisCacheTests.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.RedisCache.Tests
{
    using Juegos.Serios.Shared.RedisCache.Interfaces;
    using Moq;
    using StackExchange.Redis;
    using Xunit;
    public class RedisCacheTests
    {
        [Fact]
        public async Task GetCacheData_Returns_Default_If_Key_Not_Found()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                        .ReturnsAsync(default(RedisValue));

            var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            mockConnectionMultiplexer.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                                    .Returns(mockDatabase.Object);

            var mockRedisCache = new Mock<IRedisCache>();
#pragma warning disable CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.
            mockRedisCache.Setup(c => c.GetCacheData<string>(It.IsAny<string>()))
                          .ReturnsAsync(default(string)); // Puedes ajustar esto según tu necesidad
#pragma warning restore CS8620 // El argumento no se puede usar para el parámetro debido a las diferencias en la nulabilidad de los tipos de referencia.

            // Act
            var result = await mockRedisCache.Object.GetCacheData<string>("nonexistent_key");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveData_Returns_True_If_Key_Exists()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(d => d.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                        .ReturnsAsync(true);
            mockDatabase.Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                        .ReturnsAsync(true);

            var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            mockConnectionMultiplexer.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                                    .Returns(mockDatabase.Object);

            var mockRedisCache = new Mock<IRedisCache>();
            mockRedisCache.Setup(c => c.RemoveData(It.IsAny<string>()))
                          .ReturnsAsync(true);

            // Act
            var result = await mockRedisCache.Object.RemoveData("existing_key");

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task RemoveData_Returns_False_If_Key_Does_Not_Exist()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(d => d.KeyExistsAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                        .ReturnsAsync(false);

            var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            mockConnectionMultiplexer.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                                    .Returns(mockDatabase.Object);

            var mockRedisCache = new Mock<IRedisCache>();
            mockRedisCache.Setup(c => c.RemoveData(It.IsAny<string>()))
                          .ReturnsAsync(false);

            // Act
            var result = await mockRedisCache.Object.RemoveData("nonexistent_key");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SetCacheData_Returns_True_If_Data_Is_Successfully_Set()
        {
            // Arrange
            var mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(d => d.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                        .ReturnsAsync(true);

            var mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            mockConnectionMultiplexer.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                                    .Returns(mockDatabase.Object);

            var mockRedisCache = new Mock<IRedisCache>();
            mockRedisCache.Setup(c => c.SetCacheData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                          .ReturnsAsync(true);

            // Act
            var result = await mockRedisCache.Object.SetCacheData("key", "value", DateTimeOffset.Now.AddMinutes(10));

            // Assert
            Assert.True(result);
        }


    }
}

