namespace PeopleDomain.DL.CQRS.Commands;
internal class AddPersonToGender //consider a better name
{
    public int PersonId { get; set; }
    public int GenderId { get; set; }
    public AddPersonToGender(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}

internal class RemovePersonFromGender
{
    public int PersonId { get; set; }
    public int GenderId { get; set; }
    public RemovePersonFromGender(int personId, int genderId)
    {
        PersonId = personId;
        GenderId = genderId;
    }
}

internal class ChangePersonGender
{

    public int PersonId { get; set; }
    public int NewGenderId { get; set; }
    public int OldGenderId { get; set; }

    public ChangePersonGender(int personId, int newGenderId, int oldGenderId)
    {
        PersonId = personId;
        NewGenderId = newGenderId;
        OldGenderId = oldGenderId;
    }
}