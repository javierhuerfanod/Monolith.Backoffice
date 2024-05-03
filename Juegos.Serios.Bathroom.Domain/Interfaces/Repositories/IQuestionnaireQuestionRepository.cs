// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Domain
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IQuestionnaireQuestionRepository.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Juegos.Serios.Bathroom.Domain.Entities;
using Juegos.Serios.Shared.Domain.Ports.Persistence;
namespace Juegos.Serios.Bathroom.Domain.Interfaces.Repositories
{
    public interface IQuestionnaireQuestionRepository : IAsyncRepository<QuestionnaireQuestion>
    {

    }
}