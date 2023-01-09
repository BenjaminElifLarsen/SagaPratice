using Common.RepositoryPattern.ProcessManagers;
using PersonDomain.AL.ProcessManagers.Gender.Recognise;

namespace PersonDomain.IPL.Repositories.ProcesserManagers;
internal sealed class GenderRecogniseProcessRepository : IGenderRecogniseProcessRepository
{
    private readonly IBaseProcessManagerRepository<GenderRecogniseProcessManager> _repository;

    public GenderRecogniseProcessRepository(IBaseProcessManagerRepository<GenderRecogniseProcessManager> repository)
    {
        _repository = repository;
    }

    public void Delete(Guid correlationId)
    {
        _repository.Delete(correlationId);
    }

    public async Task<GenderRecogniseProcessManager> LoadAsync(Guid correlationId)
    {
        return await Task.Run(() => _repository.LoadAsync(correlationId));
    }

    public void Save(GenderRecogniseProcessManager manager)
    {
        _repository.Save(manager);
    }
}
