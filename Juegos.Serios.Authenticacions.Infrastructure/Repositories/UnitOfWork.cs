// ***********************************************************************
// Assembly         : Juegos.Serios.Authenticacions.Infrasturcture
// Author           : diego diaz
// Created          : 17-04-2024
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************
// <copyright file="UnitOfWork.cs" company="Universidad Javeriana">
//     Copyright (c) Universidad Javeriana All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Juegos.Serios.Authenticacions.Infrastructure.Repositories;

using Juegos.Serios.Authenticacions.Domain.Common;
using Juegos.Serios.Authenticacions.Domain.Ports.Persistence;
using Juegos.Serios.Authenticacions.Infrastructure.Persistence;
using System;
using System.Collections;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable _repositories;
    private readonly BdSqlAuthenticationContext _dbContext; 

    public UnitOfWork(BdSqlAuthenticationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public BdSqlAuthenticationContext BaselineDBContext => _dbContext;

    public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
    {
        if (_repositories == null)
        {
            _repositories = new Hashtable();
        }
        var type = typeof(TEntity).Name;
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(IAsyncRepository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);
            _repositories.Add(type, repositoryInstance);
        }
        return (IAsyncRepository<TEntity>)_repositories[type];
    }

    public async Task<int> Complete()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}

