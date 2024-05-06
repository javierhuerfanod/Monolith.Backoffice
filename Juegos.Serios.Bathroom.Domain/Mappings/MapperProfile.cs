// ***********************************************************************
// Assembly         : Juegos.Serios.Bathroom.Application
// Author           : diego diaz
// Created          : 04-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="MapperProfile.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Bathroom.Domain.Mappings
{
    using AutoMapper;
    using Juegos.Serios.Bathroom.Domain.Aggregates;
    using Juegos.Serios.Bathroom.Domain.Entities;
    using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<QuestionnaireQuestion, QuestionareQuestionDto>()
                 .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                 .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question));

            CreateMap<Weight, WeightDto>()
            .ForMember(dest => dest.WeightValue, opt => opt.MapFrom(src => src.Weight1));
        }
    }
}
