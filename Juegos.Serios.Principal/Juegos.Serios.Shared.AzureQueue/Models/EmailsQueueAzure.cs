// ***********************************************************************
// Assembly         : uegos.Serios.Shared.AzureQueue
// Author           : diego.diaz
// Created          : 28-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="EmailsQueueAzure.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// ***********************************************************************

namespace Juegos.Serios.Shared.AzureQueue.Models
{
    /// <summary>
    /// Modelos de mensaje para interacción entre juegoserios y colas en azure
    /// </summary>
    public class EmailsQueueAzure
    {
        /// <summary>
        /// Destinatarios 
        /// </summary>
        public List<string> Recipients { get; set; }
        /// <summary>
        /// Sujeto del correo
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Mensaje o cuerpo del correo
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Tipo de email a enviar
        /// </summary>
        public int TypeEmailId { get; set; }
    }
}