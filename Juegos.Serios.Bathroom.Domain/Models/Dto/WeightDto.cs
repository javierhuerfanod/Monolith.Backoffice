// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="WeightDto Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Domain.Models.Weight.Response;

public class WeightDto
{
    public int WeightId { get; set; }
    public int UserId { get; set; }
    public DateOnly? Date { get; set; }
    public int? WeightValue { get; set; } 
}
