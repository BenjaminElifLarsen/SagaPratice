using Common.Events.Domain;
using Common.RepositoryPattern;

namespace PeopleDomain.DL.Models;
public sealed class Gender : IAggregateRoot
{
    private int _genderId;
    private string _verbSubject;
    private string _verbObject;
    private readonly HashSet<IdReference<int>> _people;
    //private readonly HashSet<IDomainEvent> _events;
    private readonly HashSet<DomainEvent> _events;

    //internal int GenderId { get => _genderId; private set => _genderId = value; }
    internal string VerbSubject { get => _verbSubject; private set => _verbSubject = value; }
    internal string VerbObject { get => _verbObject; private set => _verbObject = value; }
    internal IEnumerable<IdReference<int>> People { get => _people; }

    public IEnumerable<IDomainEvent> OldEventsDesign => throw new NotImplementedException();

    public int Id { get => _genderId; private set => _genderId = value; }

    public IEnumerable<DomainEvent> Events => throw new NotImplementedException();

    //what is the term for things like her/him/they and the term for she/him???

    private Gender()
    {
        _events = new();
    }

    internal Gender(string subject, string @object)
    {
        _genderId = RandomValue.GetValue;
        _people = new();
        _verbSubject = subject;
        _verbObject = @object;
        _events = new();
    }

    //public void UpdateName(string name)
    //{ //does not make sense to change subject or object
    //    _verbSubject = name;
    //}

    internal bool AddPerson(IdReference<int> person)
    {
        return _people.Add(person);
    }

    internal bool RemovePerson(IdReference<int> person)
    {
        return _people.Remove(person);
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        //if (this == eventItem.AggregateId) //should cause an expection if this fails
        //    _events.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        //if (this == eventItem.AggregateId) //should cause an expection if this fails
        //    _events.Remove(eventItem);
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        if(this == eventItem.AggregateId)
            _events.Add(eventItem);
    }

    //public IEnumerable<Person> GetSpecificPeople(params Expression<Func<Person, bool>>[] predicates)
    //{ //if using IdReference this will not be useful 
    //    var query = _people.AsQueryable();
    //    query = predicates.Aggregate(query, (source, where) => source.Where(where));
    //    return query;
    //}

    public static bool operator ==(Gender left, int right)
    {
        return left.Id == right;
    }

    public static bool operator !=(Gender left, int right)
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