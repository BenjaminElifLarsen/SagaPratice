using Common.RepositoryPattern;
using People.DL.Model.Genders;

namespace People.DL.Model.People;

internal class Person : IAggregateRoot
{
    private int _personId;
    private string _firstName;
    private string _lastName;
    private DateTime _birth;
    private Gender _gender;

    public int PersonId { get => _personId; private set => _personId = value; }
    public string FirstName { get => _firstName; private set => _firstName = value; }
    public string LastName { get => _lastName; private set => _lastName = value; }
    public DateTime Birth { get => _birth; private set => _birth = value; }
    public Gender Gender { get => _gender; private set => _gender = value; }

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

    public void UpdateFistName(string firstName)
    {
        _firstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
        _lastName = lastName;
    }

    public void UpdateBirth(DateTime birth)
    {
        _birth = birth;
    }

    public void UpdateGender(Gender gender)
    {
        _gender = gender;
    }
}
