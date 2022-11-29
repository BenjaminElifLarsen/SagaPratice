using BaseRepository;
using PeopleDomain.DL.Models;

namespace PeopleDomain.IPL.Context;
internal static class Seeder
{
    public static void MockSeedData(IPeopleContext peopleContext)
    {
        Gender g1 = new("She", "Her");
        Gender g2 = new("She", "They");

        if (!(peopleContext as IContextData<Gender>).GetAll.Any())
        {
            peopleContext.Add(g1);
            peopleContext.Add(g2);
        }

        Person p1 = new(1,"Triss", "Nib", new(1956, 1, 2), new(g1.GenderId));
        if (!(peopleContext as IContextData<Person>).GetAll.Any())
        {
            peopleContext.Add(p1);
            g1.AddPerson(new(p1.PersonId));
        }

        peopleContext.Save();
    }
}
