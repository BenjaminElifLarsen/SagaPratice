using Common.Events.Domain;
using Common.ResultPattern;
using PersonDomain.DL.CQRS.Commands;
using PersonDomain.DL.Errrors;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;
using PersonDomain.DL.Validation;

namespace PersonDomain.DL.Factories;
internal sealed class GenderFactory : IGenderFactory
{
    public Result<Gender> CreateGender(RecogniseGender gender, GenderValidationData validationData)
    { //consider better name
        List<string> errors = new();

        var flag = new GenderValidator(gender, validationData).Validate();
        if (!flag)
        {
            errors.AddRange(GenderErrorConversion.Convert(flag));
        }

        if (errors.Any())
        {
            return new InvalidResult<Gender>(errors.ToArray());
        }

        Gender entity = new(gender.VerbSubject, gender.VerbObject);
        return new SuccessResult<Gender>(entity);
    }

    public Gender HydrateGender(IEnumerable<DomainEvent> events)
    {
        if (events is null || !events.Any()) return null;

        Gender? entity = default;
        //should validate the events, correct order on version, no missing values over the versions, only one recognised event and at most one unrecognised event
        foreach(var e in events)
        {
            switch (e.EventType)
            {
                case nameof(GenderRecognisedSucceeded):
                    var gr = e as GenderRecognisedSucceeded;
                    entity = Gender.Hydrate(gr.AggregateId, gr.Subject, gr.Object);
                    break;

                case nameof(PersonAddedToGenderSucceeded):
                    if (entity is null) throw new Exception("Event Error, cannot add to default");
                    var pa = e as PersonAddedToGenderSucceeded;
                    entity.AddPerson(pa.PersonId);
                    break;

                case nameof(PersonRemovedFromGenderSucceeded):
                    if (entity is null) throw new Exception("Event Error, cannot remove from default");
                    var pr = e as PersonRemovedFromGenderSucceeded;
                    entity.RemovePerson(pr.PersonId);
                    break;

                case nameof(GenderUnrecognisedSucceeded):
                    if (entity is null) throw new Exception("Event Error, cannot default default");
                    entity = default;
                    break;

                default: throw new Exception("Unknown event");
            }
        }
        return entity;
    }
}
