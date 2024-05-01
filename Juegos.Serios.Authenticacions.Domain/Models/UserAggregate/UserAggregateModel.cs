// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserAggregateModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;

public class UserAggregateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentNumber { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
    public int Weight { get; set; }
    public int CityId { get; set; }
    public int CityHomeId { get; set; }
    public DateTime? BirthdayDate { get; set; }
    public bool IsConsentGranted { get; set; }
}
