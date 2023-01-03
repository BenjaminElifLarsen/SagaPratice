using PeopleDomain.AL.ProcessManagers.Gender.Recognise;

namespace PeopleDomain.AL.ProcessManagers.Repositories.GenderRecogniseProcessRepository;
public interface IGenderRecogniseProcessRepository
{
    public Task<RecogniseProcessManager> LoadAsync(Guid correlationId);
    public void Save(RecogniseProcessManager manager);
    public void Delete(Guid correlationId);
}
