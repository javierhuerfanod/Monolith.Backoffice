// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.AzureQueue.Tests
// Author           : diego diaz
// Created          : 22-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="AzureQueueTests.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.AzureQueue.Tests
{
    using Xunit;
    using Moq;
    using Azure.Storage.Queues;
    using Azure.Storage.Queues.Models;
    using Azure;
    using System.Threading.Tasks;
    using Juegos.Serios.Shared.AzureQueue;

    public class AzureQueueTests
    {
        private readonly AzureQueue _azureQueue;
        private readonly Mock<QueueClient> _mockQueueClient = new Mock<QueueClient>();
        private readonly string _queueName = "test-queue";

        public AzureQueueTests()
        {
            // Assuming AzureQueue can accept an injected QueueClient, which it currently does not.
            // You need to modify AzureQueue's constructor or method where QueueClient is instantiated to allow dependency injection.
            _azureQueue = new AzureQueue("https://fakestorageaccount.queue.core.windows.net/");

            // Setup the mock QueueClient to return a mocked Response<SendReceipt>
            _mockQueueClient.Setup(x => x.SendMessageAsync(It.IsAny<string>(), null, null, CancellationToken.None))
                .ReturnsAsync(Mock.Of<Response<SendReceipt>>(r =>
                    r.Value == Mock.Of<SendReceipt>(s =>
                        s.MessageId == "fakeMessageId" &&
                        s.PopReceipt == "fakePopReceipt" &&
                        s.InsertionTime == DateTimeOffset.UtcNow &&
                        s.ExpirationTime == DateTimeOffset.UtcNow.AddDays(2)
                    )
                ));

            // How to inject this mocked QueueClient into AzureQueue would depend on modifying AzureQueue to accept an injected client.
        }

        [Fact]
        public async Task EnqueueMessageAsync_Throws_ArgumentNullException_If_MessageIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _azureQueue.EnqueueMessageAsync<string>(_queueName, null));
        }
    
    }
}






