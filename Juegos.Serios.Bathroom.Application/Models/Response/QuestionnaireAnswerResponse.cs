// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 20-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="QuestionareQuestionResponse.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace Juegos.Serios.Bathroom.Application.Models.Response;

public class QuestionnaireAggregateResponse
{
    public string QuestionnaireDescription { get; set; }
    public List<QuestionAnswerResponse> Answers { get; set; } = new List<QuestionAnswerResponse>();
}

public class QuestionAnswerResponse
{
    public bool Answer { get; set; }
    public string QuestionText { get; set; }
}
