using Common.RepositoryPattern;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.EventRepositories;
using PersonDomain.IPL.Repositories.ProcesserManagers;

namespace PersonDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IBaseEventUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository { get; }
    public IGenderEventRepository GenderEventRepository { get; }
}
