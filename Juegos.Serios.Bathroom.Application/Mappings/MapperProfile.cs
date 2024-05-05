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
namespace Juegos.Serios.Bathroom.Application.Mappings
{
    using AutoMapper;
    using Juegos.Serios.Bathroom.Application.Models.Request;
    using Juegos.Serios.Bathroom.Application.Models.Response;
    using Juegos.Serios.Bathroom.Domain.Models.QuestionnaireAnswer;
    using Juegos.Serios.Bathroom.Domain.Models.Weight;
    using Juegos.Serios.Bathroom.Domain.Models.Weight.Response;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterWeightRequest, RegisterWeightModel>()
               .ForMember(dest => dest.UserId, opt => opt.Ignore())
               .AfterMap((src, dest, context) =>
               {
                   if (context.Items.ContainsKey("userId"))
                   {
                       dest.UserId = (int)context.Items["userId"];
                   }
               });
            CreateMap<QuestionareQuestionDto, QuestionareQuestionResponse>();

            CreateMap<DomainRegisterWeightResponse, RegisterWeightResponse>()
                .ForMember(dest => dest.QuestionnaireID, opt => opt.MapFrom(src => src.QuestionnaireID))
                .ForMember(dest => dest.WeightID, opt => opt.MapFrom(src => src.WeightID))
                .ForMember(dest => dest.StatusCondition, opt => opt.MapFrom(src => src.StatusCondition))
                .ForMember(dest => dest.QuestionareQuestionsResponses, opt => opt.MapFrom(src => src.questionareQuestionsDtos));

            CreateMap<RegisterQuestionnaireAnswerRequest, RegisterQuestionnaireAnswerModel>()
                .ForMember(dest => dest.questionareQuestionModels, opt => opt.MapFrom(src => src.questionareQuestionRequests));
            CreateMap<QuestionareQuestionRequest, QuestionareQuestionModel>();
                 
        }
    }
}
