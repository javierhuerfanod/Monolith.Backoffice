// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="QuestionareSpecifications.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Bathroom.Domain.Entities;
using System.Linq.Expressions;

namespace Juegos.Serios.Bathroom.Domain.Specifications
{
    public class QuestionareQuestionSpecifications
    {
        public static Expression<Func<QuestionnaireQuestion, bool>> ByQuestionareId(int questionareId)
        {
            return QuestionnaireQuestion => QuestionnaireQuestion.QuestionnaireId == questionareId;
        }
    }    
}