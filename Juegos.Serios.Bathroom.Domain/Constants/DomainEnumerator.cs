// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 29-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DomainEnumerator.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Domain.Constants
{
    public class DomainEnumerator
    {
        public enum DifferenceStatus
        {
            SuperiorByMoreThanTwo = 2,
            SuperiorByMoreThanOne = 1
        }
        public enum WeightComparisonResult
        {
            AlreadyMeasuredToday = 0, // El peso ya se tomó ese día
            Equal = 1,            // Los pesos son iguales
            SuperiorByOne = 2,    // El peso nuevo es superior por 1
            SuperiorByTwo = 3     // El peso nuevo es superior por 2
        }
    }
}

