// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 28-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="Utils.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Authenticacions.Application.Resources.EmailsHtml;


namespace Juegos.Serios.Authenticacions.Application.Utils
{
    public static class Utils
    {
        public static string GeneratePasswordRecoveryEmail(string name, string lastName, string recoveryPassword)
        {
            var plantillaHTML = AppEmailsMessages.Emails_RecoveryPassword_Body;
            plantillaHTML = plantillaHTML.Replace("{name}", name)
                                         .Replace("{lastName}", lastName)
                                         .Replace("{recoveryPassword}", recoveryPassword);
            return plantillaHTML;
        }
    }
}


