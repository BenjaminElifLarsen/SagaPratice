using Common.Events.Store.ProcessManager;
using Common.RepositoryPattern;
using Common.RepositoryPattern.ProcessManagers;

namespace BaseRepository;
public class MockProcessManagerRepository<TProcessManager, TBaseContext> : IBaseProcessManagerRepository<TProcessManager> where TProcessManager : IBaseProcessManager where TBaseContext : IBaseContext
{
    private readonly TBaseContext _context; //move over to a context file when closer to finalise the new design regarding its implemntation.
    private readonly IEnumerable<TProcessManager> _data;
    public MockProcessManagerRepository(TBaseContext context)
    {
        _context = context;
        _data = _context.Set<TProcessManager>();
    }

    public void Delete(Guid correlationId)
    {
        var pm = _context.LoadProcessManagerAsync(correlationId).Result;
        _context.Remove(pm);
    }

    public async Task<TProcessManager> LoadAsync(Guid correlationId)
    {
        return await Task.Run(() => _data.SingleOrDefault(x => x.CorrelationId == correlationId));
    }

    public void Save(TProcessManager processManager)
    {
        if (_context.LoadProcessManagerAsync(processManager.CorrelationId).Result is null)
        {
            _context.Add(processManager);
        }
    }
}
