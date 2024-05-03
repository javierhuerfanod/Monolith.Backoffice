// ***********************************************************************
// Assembly         : Juegos.Serios.Shared.Domain
// Author           : diego diaz
// Created          : 02-05-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="IUnitOfWork.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Shared.Domain.Ports.Persistence
{
    using Juegos.Serios.Shared.Domain.Common;
    using System;
    public interface IUnitOfWork : IDisposable
    {
        IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;
        Task<int> Complete();
    }
}
