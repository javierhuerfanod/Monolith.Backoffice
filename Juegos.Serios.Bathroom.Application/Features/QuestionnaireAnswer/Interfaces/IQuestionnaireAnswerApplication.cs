// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 05-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IQuestionnaireAnswerApplication.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using Juegos.Serios.Bathroom.Application.Models.Request;
using Juegos.Serios.Bathroom.Application.Models.Response;
using Juegos.Serios.Shared.Application.Response;

namespace Juegos.Serios.Bathroom.Application.Features.QuestionnaireAnswer.Interfaces
{
    public interface IQuestionnaireAnswerApplication
    {
        Task<ApiResponse<object>> RegisterQuestionnaireAnswer(RegisterQuestionnaireAnswerRequest registerQuestionnaireAnswerRequest);
        Task<ApiResponse<QuestionnaireAggregateResponse>> GetQuestionnaireAnswersByWeight(GetQuestionnaireAnswersByWeightRequest questionnaireAnswersByWeightRequest);
    }
}

