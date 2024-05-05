// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 05-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="RegisterQuestionnaireAnswerRequest.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace Juegos.Serios.Bathroom.Application.Models.Request;

public class RegisterQuestionnaireAnswerRequest
{
    [Required(ErrorMessage = "El ID del cuestionario es obligatorio.")]
    public int QuestionnaireID { get; set; }

    [Required(ErrorMessage = "El ID del peso es obligatorio.")]
    public int WeightID { get; set; }

    [Required(ErrorMessage = "Las preguntas del cuestionario son obligatorias.")]
    public List<QuestionareQuestionRequest> questionareQuestionRequests { get; set; }
}

public class QuestionareQuestionRequest
{
    [Required(ErrorMessage = "El ID de la pregunta es obligatorio.")]
    public int QuestionId { get; set; }

    [Required(ErrorMessage = "La respuesta de la pregunta es obligatoria.")]
    public bool Answer { get; set; }
}
