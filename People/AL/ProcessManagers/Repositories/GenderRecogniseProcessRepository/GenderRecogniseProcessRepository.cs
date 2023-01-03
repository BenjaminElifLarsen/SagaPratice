using BaseRepository;
using PeopleDomain.AL.ProcessManagers.Gender.Recognise;

namespace PeopleDomain.AL.ProcessManagers.Repositories.GenderRecogniseProcessRepository;
internal sealed class GenderRecogniseProcessRepository : IGenderRecogniseProcessRepository
{
    private readonly MockProcessManagerRepository<RecogniseProcessManager> _repository;

    public GenderRecogniseProcessRepository(MockProcessManagerRepository<RecogniseProcessManager> repository)
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
