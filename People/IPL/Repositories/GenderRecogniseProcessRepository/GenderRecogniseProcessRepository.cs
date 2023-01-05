using BaseRepository;
using Common.RepositoryPattern.ProcessManagers;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;

namespace PeopleDomain.IPL.Repositories.GenderRecogniseProcessRepository;
internal sealed class GenderRecogniseProcessRepository : IGenderRecogniseProcessRepository
{
    private readonly IBaseProcessManagerRepository<RecogniseProcessManager> _repository;

    public GenderRecogniseProcessRepository(IBaseProcessManagerRepository<RecogniseProcessManager> repository)
    {
        _repository = repository;
    }

    public void Delete(Guid correlationId)
    {
        _repository.Delete(correlationId);
    }

    public async Task<RecogniseProcessManager> LoadAsync(Guid correlationId)
    {
        return await Task.Run(() => _repository.LoadAsync(correlationId));
    }

    public void Save(RecogniseProcessManager manager)
    {
        _repository.Save(manager);
    }
}
