// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Infrasturcture
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="QuestionnaireQuestionRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Infrastructure.Persistence;
namespace Juegos.Serios.Bathroom.Infrastructure.Repositories
{
    public class QuestionnaireQuestionRepository : RepositoryBase<QuestionnaireQuestion>, IQuestionnaireQuestionRepository
    {
        public QuestionnaireQuestionRepository(BdSqlBathroomContext context) : base(context)
        {

        }
    }
}
