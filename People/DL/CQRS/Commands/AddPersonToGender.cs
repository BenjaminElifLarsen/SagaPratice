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
