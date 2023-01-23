using Common.DDD;
using Common.Events.Domain;
using Common.RepositoryPattern;

namespace Security.DL.Users;
internal class User : IAggregateRoot, ISoftDelete
{
    private string _firstName;
    private string _lastName;
    private bool _deleted;

    public Guid Id => throw new NotImplementedException();

    public IEnumerable<DomainEvent> Events => throw new NotImplementedException();

    public bool Deleted => throw new NotImplementedException();

    public void AddDomainEvent(DomainEvent eventItem)
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        throw new NotImplementedException();
    }
}
