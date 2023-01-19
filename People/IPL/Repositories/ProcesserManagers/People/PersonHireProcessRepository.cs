using Common.RepositoryPattern.ProcessManagers;
using PersonDomain.AL.ProcessManagers.Person.Hire;

namespace PersonDomain.IPL.Repositories.ProcesserManagers.People;
internal class PersonHireProcessRepository : IPersonHireProcessRepository
{
    private readonly IBaseProcessManagerRepository<HireProcessManager> _repository;

    public PersonHireProcessRepository(IBaseProcessManagerRepository<HireProcessManager> repository)
    {
        _repository = repository;
    }

    public void Delete(Guid correlationId)
    {
        _repository.Delete(correlationId);
    }

    public async Task<HireProcessManager> LoadAsync(Guid correlationId)
    {
        return await _repository.LoadAsync(correlationId);
    }

    public void Save(HireProcessManager manager)
    {
        _repository.Save(manager);
    }
}
