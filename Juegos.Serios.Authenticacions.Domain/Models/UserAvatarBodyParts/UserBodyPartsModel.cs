// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UserBodyPartsModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Domain.Models.UserAvatarBodyParts
{
    public class UserBodyPartsModel
    {
        public int UserId { get; set; }
        public List<BodyPartModel> BodyParts { get; set; }
    }

    public class BodyPartModel
    {
        public string BodyPartName { get; set; }
        public int BodyPartAnimationId { get; set; }
    }
    public enum BodyPartName
    {
        Body,
        Hair,
        Torso,
        Legs
    }

}
