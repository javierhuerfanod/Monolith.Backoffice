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

public class QuestionareQuestionResponse
{
    public int QuestionId { get; set; }
    public string Question { get; set; }
}

public class RegisterWeightResponse
{
    public int QuestionnaireID { get; set; }
    public int WeightID { get; set; }
    public int StatusCondition { get; set; }
    public List<QuestionareQuestionResponse> QuestionareQuestionsResponses { get; set; }
}
