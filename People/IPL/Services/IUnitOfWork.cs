﻿using Common.UnitOfWork;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;
using PersonDomain.IPL.Repositories.ProcesserManagers.Genders;

namespace PersonDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IBaseEventUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository { get; }
    public IGenderEventRepository GenderEventRepository { get; }
    public IPersonEventRepository PersonEventRepository { get; }
}
