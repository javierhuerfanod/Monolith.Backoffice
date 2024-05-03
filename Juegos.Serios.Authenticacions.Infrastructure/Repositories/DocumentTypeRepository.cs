// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DocumentTypeRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Juegos.Serios.Authenticacions.Domain.Entities;
using Juegos.Serios.Authenticacions.Domain.Interfaces.Repositories;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories
{

    public class DocumentTypeRepository : RepositoryBase<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(BdSqlAuthenticationContext context) : base(context)
        {

        }
    }
}
