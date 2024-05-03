// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Api
// Author           : diego diaz
// Created          : 01-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="NullableDateTimeConverter.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Juegos.Serios.Shared.Api.UtilCross
{
    public class NullableDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
                string dateString = reader.GetString();
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
                if (DateTime.TryParse(dateString, out DateTime date))
                {
                    return date;
                }
            }

            throw new JsonException("Fecha inválida o formato incorrecto. Se espera una fecha en formato ISO 8601, por ejemplo: '2024-05-01T18:00:07Z'.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }

}

