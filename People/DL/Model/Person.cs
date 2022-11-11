using Common.Events.Domain;
using Common.RepositoryPattern;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Model;

public class Person : IAggregateRoot, ISoftDeleteDate
{
    private int _personId;
    private string _firstName;
    private string _lastName;
    private DateOnly _birth;
    private IdReference _gender;
    private DateOnly? _deletedFrom;
    private readonly HashSet<IDomainEvent> _events;

    internal int PersonId { get => _personId; private set => _personId = value; }
    internal string FirstName { get => _firstName; private set => _firstName = value; }
    internal string LastName { get => _lastName; private set => _lastName = value; }
    internal DateOnly Birth { get => _birth; private set => _birth = value; }
    internal IdReference Gender { get => _gender; private set => _gender = value; }

    public DateOnly? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }

    public IEnumerable<IDomainEvent> Evnets => _events;

    private Person()
    {
        _events = new();
    }

    internal Person(string firstName, string lastName, DateOnly birth, IdReference gender)
    {
        _personId = RandomValue.GetValue;
        _firstName = firstName;
        _lastName = lastName;
        _birth = birth;
        _gender = gender;
        _events = new();
    }

    internal Person(int id, string firstName, string lastName, DateOnly birth, IdReference gender) : this(firstName, lastName, birth, gender)
    {
        _personId = id;
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

    internal void UpdateGenderIdentification(IdReference gender)
    {
        _gender = gender;
    }

    public void Delete(DateOnly? dateTime)
    {
        _deletedFrom = dateTime;
    }

    public void AddDomainEvent(IDomainEvent eventItem)
    {
        if (this == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        if (this == eventItem.AggregateId) //should cause an expection if this fails
            _events.Remove(eventItem);
    }

    public static bool operator ==(Person left, int right)
    {
        return left.PersonId == right;
    }

    public static bool operator !=(Person left, int right)
    {
        return !(left == right);
    }

    public static bool operator ==(Person left, Person right)
    {
        return left.PersonId == right.PersonId;
    }

    public static bool operator !=(Person left, Person right)
    {
        return !(left == right);
    }
}
