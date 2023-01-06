using Common.Events.Domain;
using Common.RepositoryPattern;

namespace PersonDomain.DL.Models;
public sealed class Gender : IAggregateRoot
{
    private Guid _id;
    private string _verbSubject;
    private string _verbObject;
    private readonly HashSet<IdReference> _people;
    //private readonly HashSet<IDomainEvent> _events;
    private readonly HashSet<DomainEvent> _events;

    //internal int GenderId { get => _genderId; private set => _genderId = value; }
    internal string VerbSubject { get => _verbSubject; private set => _verbSubject = value; }
    internal string VerbObject { get => _verbObject; private set => _verbObject = value; }
    internal IEnumerable<Guid> People { get => _people.Select(x => x.Id); }

    public Guid Id { get => _id; private set => _id = value; }

    public IEnumerable<DomainEvent> Events => _events;

    //what is the term for things like her/him/they and the term for she/him???

    private Gender()
    {
        _events = new();
    }

    internal Gender(string subject, string @object)
    {
        _id = Guid.NewGuid();
        _people = new();
        _verbSubject = subject;
        _verbObject = @object;
        _events = new();
    }

    //public void UpdateName(string name)
    //{ //does not make sense to change subject or object
    //    _verbSubject = name;
    //}

    internal bool AddPerson(Guid person)
    {
        return _people.Add(new(person));
    }

    internal bool RemovePerson(Guid person)
    {
        return _people.Remove(new(person));
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        if(this == eventItem.AggregateId)
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        if (this == eventItem.AggregateId)
            _events.Remove(eventItem);
    }

    //public IEnumerable<Person> GetSpecificPeople(params Expression<Func<Person, bool>>[] predicates)
    //{ //if using IdReference this will not be useful 
    //    var query = _people.AsQueryable();
    //    query = predicates.Aggregate(query, (source, where) => source.Where(where));
    //    return query;
    //}

    public static bool operator ==(Gender left, Guid right)
    {
        return left.Id == right;
    }

    public static bool operator !=(Gender left, Guid right)
    {
        return !(left == right);
    }

    public static bool operator ==(Gender left, Gender right)
    {
        return left.Id == right.Id;
    }

    public static bool operator !=(Gender left, Gender right)
    {
        return !(left == right);
    }
}