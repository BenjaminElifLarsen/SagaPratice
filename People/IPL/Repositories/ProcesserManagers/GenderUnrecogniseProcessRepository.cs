using Common.RepositoryPattern.ProcessManagers;
using PersonDomain.AL.ProcessManagers.Gender.Unrecognise;

namespace PersonDomain.IPL.Repositories.ProcesserManagers;
internal class GenderUnrecogniseProcessRepository : IGenderUnrecogniseProcessRepository
{
    private readonly IBaseProcessManagerRepository<GenderUnrecogniseProcessManager> _repository;

    public GenderUnrecogniseProcessRepository(IBaseProcessManagerRepository<GenderUnrecogniseProcessManager> repository)
    {
        _repository = repository;
    }

    public void Delete(Guid correlationId)
    {
        _repository.Delete(correlationId);
    }

    public async Task<GenderUnrecogniseProcessManager> LoadAsync(Guid correlationId)
    {
        return await Task.Run(() => _repository.LoadAsync(correlationId));
    }

    public void Save(GenderUnrecogniseProcessManager manager)
    {
        _repository.Save(manager);
    }
}
