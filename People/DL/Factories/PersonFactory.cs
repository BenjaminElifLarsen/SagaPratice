using Common.Events.Domain;
using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Errrors;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;

namespace PersonDomain.DL.Factories;
internal sealed class PersonFactory : IPersonFactory
{
    public Result<Person> CreatePerson(HirePersonFromUser person, PersonValidationData validationData)
    {
        List<string> errors = new();

        var flag = new PersonHireValidator(person, validationData).Validate();
        if(!flag)
        {
            errors.AddRange(PersonErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Person>(errors.ToArray());
        }

        Person entity = new(person.FirstName, person.LastName, new(person.Birth.Year, person.Birth.Month, person.Birth.Day), person.Gender);
        return new SuccessResult<Person>(entity);
    }

    public Person HydratePerson(IEnumerable<DomainEvent> events)
    {
        if (events is null || !events.Any()) return null;

        Person? entity = default;

        //should validate the events, correct order on version, no missing values over the versions, only one recognised event and at most one unrecognised event
        foreach (var e in events)
        {
            switch(e.EventType)
            {
                case nameof(PersonHiredSucceeded): //move the code in each case into their own method at some point
                    EventHandling(e as PersonHiredSucceeded, ref entity);
                    break;

                case nameof(PersonFiredSucceeded):
                    EventHandling(e as PersonFiredSucceeded, ref entity);
                    //need the fired from field 
                    break;

                case nameof(PersonChangedGender):
                    EventHandling(e as PersonChangedGender, ref entity);
                    break;

                case nameof(PersonChangedBirth):
                    EventHandling(e as PersonChangedBirth, ref entity);
                    break;

                case nameof(PersonChangedLastName):
                    EventHandling(e as PersonChangedLastName, ref entity);
                    break;

                case nameof(PersonChangedFirstName):
                    EventHandling(e as PersonChangedFirstName, ref entity);
                    break;

                default: throw new Exception("Unknown event");
            }
        }
        return entity;
    }

    private static void EventHandling(PersonHiredSucceeded ph, ref Person? entity)
    {
        var birthHydrate = new DateOnly(ph.Birth.Year, ph.Birth.Month, ph.Birth.Day);
        entity = Person.Hydrate(ph.AggregateId, ph.GenderId, ph.LastName, ph.FirstName, birthHydrate);
    }

    private static void EventHandling(PersonFiredSucceeded pf, ref Person? entity)
    {
        if (entity is null) throw new Exception("Event Error, cannot default default");
    }

    private static void EventHandling(PersonChangedGender pcg, ref Person? entity)
    {
        if (entity is null) throw new Exception("Event Error, cannot change gender on default");
        entity.UpdateGenderIdentification(pcg.GenderId);
    }

    private static void EventHandling(PersonChangedBirth pcb, ref Person? entity)
    {
        if (entity is null) throw new Exception("Event Error, cannot change birth on default");
        var birthUpdate = new DateOnly(pcb.Birth.Year, pcb.Birth.Month, pcb.Birth.Day);
        entity.UpdateBirth(birthUpdate);
    }

    private static void EventHandling(PersonChangedLastName pcl, ref Person? entity)
    {
        if (entity is null) throw new Exception("Event Error, cannot change last name on default");
        entity.ReplaceLastName(pcl.LastName);
    }

    private static void EventHandling(PersonChangedFirstName pcf, ref Person? entity)
    {
        if (entity is null) throw new Exception("Event Error, cannot change first name on default");
        entity.ReplaceFirstName(pcf.FirstName);
    }
}
