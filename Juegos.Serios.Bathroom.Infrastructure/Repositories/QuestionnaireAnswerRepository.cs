// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Infrasturcture
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="QuestionnaireAnswerRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Bathroom.Domain.Interfaces.Repositories;
using Juegos.Serios.Bathroom.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace Juegos.Serios.Bathroom.Infrastructure.Repositories
{
    public class QuestionnaireAnswerRepository : RepositoryBase<QuestionnaireAnswer>, IQuestionnaireAnswerRepository
    {
        public QuestionnaireAnswerRepository(BdSqlBathroomContext context) : base(context)
        {

        }
        public async Task<IReadOnlyList<QuestionnaireAnswer>> GetAnswersByWeightIdWithDetails(int weightId)
        {
            return await _context.QuestionnaireAnswers
                                 .Where(answer => answer.WeightId == weightId)
                                 .Include(answer => answer.Question)
                                     .ThenInclude(question => question.Questionnaire)
                                 .Include(answer => answer.Weight)
                                 .AsNoTracking()
                                 .ToListAsync();
        }
    }
}
