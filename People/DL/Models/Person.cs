using Common.Events.Domain;
using Common.RepositoryPattern;

namespace PersonDomain.DL.Models;

public sealed class Person : IAggregateRoot, ISoftDeleteDate
{
    private Guid _id;
    private string _firstName;
    private string _lastName;
    private DateOnly _birth;
    private IdReference _gender;
    private DateOnly? _deletedFrom;
    private readonly HashSet<DomainEvent> _events;

    internal string FirstName { get => _firstName; private set => _firstName = value; }
    internal string LastName { get => _lastName; private set => _lastName = value; }
    internal DateOnly Birth { get => _birth; private set => _birth = value; }
    internal Guid Gender { get => _gender; private set => _gender = value; }

    public DateOnly? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }

    public Guid Id { get => _id; private set => _id = value; }

    public IEnumerable<DomainEvent> Events => _events;

    private Person()
    {
        _events = new();
    }

    internal Person(string firstName, string lastName, DateOnly birth, Guid gender)
    {
        _id = Guid.NewGuid();
        _firstName = firstName;
        _lastName = lastName;
        _birth = birth;
        _gender = gender;
        _events = new();
    }

    internal Person(Guid id, string firstName, string lastName, DateOnly birth, Guid gender) : this(firstName, lastName, birth, gender)
    {
        _id = id;
    }

    internal void ReplaceFistName(string firstName)
    {
        _firstName = firstName;
    }

    internal void ReplaceLastName(string lastName)
    {
        _lastName = lastName;
    }

    internal void UpdateBirth(DateOnly birth)
    {
        _birth = birth;
    }

    internal void UpdateGenderIdentification(Guid gender)
    {
        _gender = gender;
    }

    public void Delete(DateOnly? dateTime)
    {
        _deletedFrom = dateTime;
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        if (this == eventItem.AggregateId) //could cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        if (this == eventItem.AggregateId) //could cause an expection if this fails
            _events.Remove(eventItem);
    }

    public static bool operator ==(Person left, Guid right)
    {
        return left.Id == right;
    }

    public static bool operator !=(Person left, Guid right)
    {
        return !(left == right);
    }

    public static bool operator ==(Person left, Person right)
    {
        return left.Id == right.Id;
    }

    public static bool operator !=(Person left, Person right)
    {
        return !(left == right);
    }
}
