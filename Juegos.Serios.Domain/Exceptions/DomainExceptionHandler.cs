// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Domain
// Author           : diego diaz
// Created          : 18-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="DomainExceptionHandler.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Juegos.Serios.Domain.Shared.Exceptions;

public static class DomainExceptionHandler
{
    public static async Task<T> HandleAsync<T>(Func<Task<T>> func)
    {
        try
        {
            return await func();
        }
        catch (NotFoundException ex)
        {
            Log(ex);
            throw new Exception("Resource not found.", ex);
        }
        catch (ValidationException ex)
        {
            Log(ex);
            throw new Exception("Validation failed.", ex);
        }
        catch (Exception ex)
        {
            Log(ex);
            throw new Exception("An unexpected error occurred.", ex);
        }
    }

    private static void Log(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}
