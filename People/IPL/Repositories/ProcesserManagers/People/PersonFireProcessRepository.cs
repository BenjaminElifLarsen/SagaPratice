using Common.RepositoryPattern.ProcessManagers;
using PersonDomain.AL.ProcessManagers.Person.Fire;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.People;
internal class PersonFireProcessRepository : IPersonFireProcessRepository
{
    private readonly IBaseProcessManagerRepository<FireProcessManager> _repository;

    public PersonFireProcessRepository(IBaseProcessManagerRepository<FireProcessManager> repository)
    {
        _repository = repository;
    }

    public void Delete(Guid correlationId)
    {
        _repository.Delete(correlationId);
    }

    public async Task<FireProcessManager> LoadAsync(Guid correlationId)
    {
        return await _repository.LoadAsync(correlationId);
    }

    public void Save(FireProcessManager manager)
    {
        _repository.Save(manager);
    }
}
