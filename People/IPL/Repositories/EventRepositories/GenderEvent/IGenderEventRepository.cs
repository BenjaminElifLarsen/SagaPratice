using PersonDomain.DL.CQRS.Queries.Events;
using PersonDomain.DL.Models;

namespace PersonDomain.IPL.Repositories.EventRepositories.GenderEvent;
public interface IGenderEventRepository
{
    public Task<Gender> GetForOperationAsync(Guid id);
    public Task<Gender> GetForOperationAtSpecificPointAsync(Guid id, DateTime timePoint);
    public void AddEvents(Gender entity);
    //need to figure out how to do cqrs with events
    //instead of operating on the domain model, it will need to operator events
    /*
     * Consider projections
     * find the required events by their aggregate id and aggregate type
     * could further filter on the uniqueIds that are of interesting, e.g. personAdded, name changed and so on
     *  could also allow to easier getting related data, e.g. names of all belonging to a specific gender
     *      query for all personIds currently know by a genderId and then project the names via the person events
     * could check if an aggregate had a remove event and return null if that aggregate is wanted or ignore it for collection
     * but how to do/write it?     
     *      
     *      
     */
    public T Test<T>(Guid id, IQueryBaseTest<T> query) where T : IProjection;
}