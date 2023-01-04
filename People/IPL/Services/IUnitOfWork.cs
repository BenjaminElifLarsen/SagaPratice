﻿using Common.Events.Domain;
using Common.RepositoryPattern;
using PeopleDomain.IPL.Repositories.DomainModels;

namespace PeopleDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IBaseEventUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
}
