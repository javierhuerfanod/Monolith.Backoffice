// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RegisterWeightModel.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Bathroom.Domain.Models.Weight.Response;

public class DomainRegisterWeightResponse
{
    public int WeightID { get; set; }
    public int StatusCondition { get; set; }
    public string Message { get; set; }
    public List<QuestionareQuestionDto> questionareQuestionsDtos { get; set; }
}

