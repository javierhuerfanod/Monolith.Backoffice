// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.AzureQueue
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IAzureQueue.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.AzureQueue.Interfaces
{
    public interface IAzureQueue
    {
        /// <summary>
        /// Enqueues a message of a generic type into an Azure Storage Queue.
        /// </summary>
        /// <typeparam name="T">The type of the message to enqueue.</typeparam>
        /// <param name="queueName">The name of the Azure Storage Queue to which the message will be enqueued.</param>
        /// <param name="message">The message object to enqueue.</param>
        /// <returns>
        ///   <c>true</c> if the message was successfully enqueued; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> is <c>null</c>.</exception>
        /// <remarks>
        ///   <para>
        ///     This method enqueues a message of the specified generic type into an Azure Storage Queue.
        ///     It serializes the message object to JSON format before enqueuing it in the specified queue.
        ///   </para>
        ///   <para>
        ///     If the <paramref name="message"/> is <c>null</c>, an <see cref="ArgumentNullException"/> is thrown.
        ///   </para>
        /// </remarks>
        Task<bool> EnqueueMessageAsync<T>(string queueName, T message);
    }
}
