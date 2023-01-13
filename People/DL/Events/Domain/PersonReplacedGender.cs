using Common.Events.Domain;
using Common.Events.Store.Event;
using PersonDomain.DL.Models;
using PersonDomain.IPL.Repositories.EventRepositories.PersonEvent;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonReplacedGender : DomainEvent
{
    public Guid NewGenderId { get; private set; }

    public Guid OldGenderId { get; private set; }

    internal PersonReplacedGender(Person aggregate, Guid oldGenderId, Guid correlationId, Guid causationId) : base(aggregate, correlationId, causationId)
    {
        NewGenderId = aggregate.Gender;
        OldGenderId = oldGenderId;
    }

    public PersonReplacedGender(Event e) : base(e)
    {
        if (e is null || e.Data is null) throw new ArgumentNullException(nameof(e));
        NewGenderId = Guid.Parse(e.Data.Single(x => x == PersonPropertyId.Gender).Value);
        //cannot set OldGenderId which makes me wonder if this event could not be improved
        //maybe make it a system event, the two methods for adding and removing a person from gender generetes their own domain events,
        //and then make a new event to also be triggered from the command handler, that trigger this that stores the new Id, so it can be saved in the store.
        //call this PersonReplacedGender and the new event called PersonChangedGender. This method would need the PersonId as a property
    }
}