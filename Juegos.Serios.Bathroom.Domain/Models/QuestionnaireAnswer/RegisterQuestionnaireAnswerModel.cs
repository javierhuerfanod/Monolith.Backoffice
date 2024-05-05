// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RegisterQuestionnaireAnswerModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Domain.Models.QuestionnaireAnswer;

public class RegisterQuestionnaireAnswerModel
{
    public int WeightID { get; set; }

    public List<QuestionareQuestionModel> questionareQuestionModels { get; set; }
}
public class QuestionareQuestionModel
{
    public int QuestionId { get; set; }
    public string Answer { get; set; }
}

