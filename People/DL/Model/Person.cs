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

    internal int PersonId { get => _personId; private set => _personId = value; }
    internal string FirstName { get => _firstName; private set => _firstName = value; }
    internal string LastName { get => _lastName; private set => _lastName = value; }
    internal DateOnly Birth { get => _birth; private set => _birth = value; }
    internal IdReference Gender { get => _gender; private set => _gender = value; }

    public DateOnly? DeletedFrom { get => _deletedFrom; private set => _deletedFrom = value; }

    private Person()
    {

    }

    internal Person(string firstName, string lastName, DateOnly birth, IdReference gender)
    {
        _personId = RandomValue.GetValue;
        _firstName = firstName;
        _lastName = lastName;
        _birth = birth;
        _gender = gender;
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
