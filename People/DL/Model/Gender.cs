using Common.Events.Domain;
using Common.RepositoryPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Model;
public class Gender : IAggregateRoot
{
    private int _genderId;
    private string _verbSubject;
    private string _verbObject;
    private readonly HashSet<IdReference> _people;
    private readonly HashSet<IDomainEvent> _events;

    internal int GenderId { get => _genderId; private set => _genderId = value; }
    internal string VerbSubject { get => _verbSubject; private set => _verbSubject = value; }
    internal string VerbObject { get => _verbObject; private set => _verbObject = value; }
    internal IEnumerable<IdReference> People { get => _people; }

    public IEnumerable<IDomainEvent> Evnets => _events;

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

    internal bool AddPerson(IdReference person)
    {
        return _people.Add(person);
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        if (this == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        if (this == eventItem.AggregateId) //should cause an expection if this fails
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
        return left.GenderId == right;
    }

    public static bool operator !=(Gender left, int right)
    {
        return !(left == right);
    }

    public static bool operator ==(Gender left, Gender right)
    {
        return left.GenderId == right.GenderId;
    }

    public static bool operator !=(Gender left, Gender right)
    {
        return !(left == right);
    }
}