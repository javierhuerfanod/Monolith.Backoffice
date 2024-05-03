// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Api
// Author           : diego diaz
// Created          : 29-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="GenericEnumerator.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Juegos.Serios.Shared.Api.UtilCross.Swagger
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ApplicationTokenHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
            var hasAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<IncludeApplicationTokenHeaderAttribute>().Any()
                               || context.MethodInfo.GetCustomAttributes(true).OfType<IncludeApplicationTokenHeaderAttribute>().Any();
#pragma warning restore CS8602 // Desreferencia de una referencia posiblemente NULL.

            if (hasAttribute)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Application-Token",
                    In = ParameterLocation.Header,
                    Description = "Application token for accessing this endpoint",
                    Required = true
                });
            }
        }
    }
}

