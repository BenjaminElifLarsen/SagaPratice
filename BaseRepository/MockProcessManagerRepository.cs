using Common.Events.Store.ProcessManager;
using Common.RepositoryPattern.ProcessManagers;

namespace BaseRepository;
public class MockProcessManagerRepository<TProcessManager> : IBaseProcessManagerRepository<TProcessManager> where TProcessManager : BaseProcessManager
{
    private static HashSet<TProcessManager> _processManagers; //move over to a context file when closer to finalise the new design regarding its implemntation.

    public MockProcessManagerRepository()
    {
        _processManagers = new();
    }

    public void Delete(Guid correlationId)
    {
        throw new NotImplementedException();
    }

    public Task<TProcessManager> LoadAsync(Guid correlationId)
    {
        throw new NotImplementedException();
    }

    public void Save(TProcessManager processManager)
    {
        throw new NotImplementedException();
    }
}
