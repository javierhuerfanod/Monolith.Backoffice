// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="Utils.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Juegos.Serios.Bathroom.Application.Resources.BathroomHtml;


namespace Juegos.Serios.Bathroom.Application.Utils
{
    public static class Utils
    {
        public static string GenerateWeightEqualEmail(string name, string lastName)
        {
            var plantillaHTML = AppBathroomMessages.Emails_WeightEqual_Body;
            plantillaHTML = plantillaHTML.Replace("{name}", name)
                                         .Replace("{lastName}", lastName);
            return plantillaHTML;
        }

        public static string GenerateWeightSuperiorByOneEmail(string name, string lastName)
        {
            var plantillaHTML = AppBathroomMessages.Emails_SuperiorByOne_Body;
            plantillaHTML = plantillaHTML.Replace("{name}", name)
                                         .Replace("{lastName}", lastName);
            return plantillaHTML;
        }

        public static string GenerateWeightSuperiorByTwoEmail(string name, string lastName)
        {
            var plantillaHTML = AppBathroomMessages.Emails_SuperiorByTwo_Body;
            plantillaHTML = plantillaHTML.Replace("{name}", name)
                                         .Replace("{lastName}", lastName);
            return plantillaHTML;
        }
    }
}


