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
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public List<string> Recipients { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        /// <summary>
        /// Sujeto del correo
        /// </summary>
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string Subject { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        /// <summary>
        /// Mensaje o cuerpo del correo
        /// </summary>
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public string Message { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        /// <summary>
        /// Tipo de email a enviar
        /// </summary>
        public int TypeEmailId { get; set; }
    }
}