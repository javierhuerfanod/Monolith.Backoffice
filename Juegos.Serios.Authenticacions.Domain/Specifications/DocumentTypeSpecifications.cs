// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Domain
// Author           : diego diaz
// Created          : 16-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RolSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq.Expressions;
using Juegos.Serios.Authenticacions.Domain.Entities.DocumentType;

namespace Juegos.Serios.Authenticacions.Domain.Specifications
{
    public class DocumentTypeSpecifications
    {
        public static Expression<Func<DocumentType, bool>> ById(int documentTypeId)
        {
            return r => r.DocumentTypeId == documentTypeId;
        }
    }
}