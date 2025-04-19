// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Application
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="MapperProfile.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Aurora.Backend.Baseline.Application.Features.ConceptoBase.Commands
{
    using AutoMapper;
    using Juegos.Serios.Authenticacions.Application.Models.Request;
    using Juegos.Serios.Authenticacions.Application.Models.Response;
    using Juegos.Serios.Authenticacions.Domain.Entities;
    using Juegos.Serios.Authenticacions.Domain.Models.UserAggregate;
    using Juegos.Serios.Authenticacions.Domain.Models.UserAvatarBodyParts;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Role, RolResponse>();
            CreateMap<City, CityResponse>();
            CreateMap<UserAvatarBodyPart, UserBodyPartsResponse>();
            CreateMap<UserBodyPartsRequest, UserBodyPartsModel>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
               .AfterMap((src, dest, context) =>
               {
                   if (context.Items.ContainsKey("userId"))
                   {
                       dest.UserId = (int)context.Items["userId"];
                   }
               });
            CreateMap<BodyPartRequest, BodyPartModel>();
            CreateMap<UserCreateRequest, UserAggregateModel>();
            CreateMap<UpdatePasswordRequest, UpdatePasswordModel>()
               .ForMember(dest => dest.UserId, opt => opt.Ignore())
               .AfterMap((src, dest, context) =>
               {
                   if (context.Items.ContainsKey("userId"))
                   {
                       dest.UserId = (int)context.Items["userId"];
                   }
               });
        }
    }
}
