using Common.RepositoryPattern;
using People.DL.Model.Genders;

namespace People.DL.Model.People;

public class Person : IAggregateRoot
{
    private int _personId;
    private string _firstName;
    private string _lastName; 
    private DateTime _birth;
    private Gender _gender;

    internal int PersonId { get => _personId; private set => _personId = value; }
    internal string FirstName { get => _firstName; private set => _firstName = value; }
    internal string LastName { get => _lastName; private set => _lastName = value; }
    internal DateTime Birth { get => _birth; private set => _birth = value; }
    internal Gender Gender { get => _gender; private set => _gender = value; }

    private Person()
    {

    }

    internal Person(int personId, string firstName, string lastName, DateTime birth, Gender gender)
    {
        _personId = personId;
        _firstName = firstName;
        _lastName = lastName;
        _birth = birth;
        _gender = gender;
    }

    internal void UpdateFistName(string firstName)
    {
        _firstName = firstName;
    }

    internal void UpdateLastName(string lastName)
    {
        _lastName = lastName;
    }

    internal void UpdateBirth(DateTime birth)
    {
        _birth = birth;
    }

    internal void UpdateGender(Gender gender)
    {
        _gender = gender;
    }
}
