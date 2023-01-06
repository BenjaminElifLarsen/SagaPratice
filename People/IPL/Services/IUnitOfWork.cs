using Common.RepositoryPattern;
using PersonDomain.IPL.Repositories.DomainModels;
using PersonDomain.IPL.Repositories.GenderRecogniseProcessRepository;

namespace PersonDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IBaseEventUnitOfWork
{
    public IGenderRepository GenderRepository { get; }
    public IPersonRepository PersonRepository { get; }
    public IGenderRecogniseProcessRepository GenderRecogniseProcessRepository { get; }
}
