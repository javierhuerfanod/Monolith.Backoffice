// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 03-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IQuestionnaireAnswerService.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Juegos.Serios.Bathroom.Domain.Models.QuestionnaireAnswer;

namespace Juegos.Serios.Bathroom.Domain.Interfaces.Services
{
    public interface IQuestionnaireAnswerService<T>
    {
        Task<bool> RegisterQuestionnaireAnswer(RegisterQuestionnaireAnswerModel registerQuestionnaireAnswerModel);
    }
}