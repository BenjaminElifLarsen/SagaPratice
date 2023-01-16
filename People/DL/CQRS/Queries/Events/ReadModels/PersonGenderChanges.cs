using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.CQRS.Queries.Events.ReadModels;
public sealed record PersonGenderChanges : IProjection
{
    private IList<Gender> _genders;
    public Guid Id { get; private set; }
    public IEnumerable<Gender> GendersOverTime => _genders;
    public Guid CurrentGender;

    private PersonGenderChanges()
    {

    }

    private PersonGenderChanges(Guid id)
    {
        Id = id;
        _genders = new List<Gender>();
    }

    public void AddGender(Guid genderId, long ticks)
    {
        _genders.Add(new(genderId, ticks));
        CurrentGender = genderId;
    }

    public static PersonGenderChanges? Projection(IEnumerable<DomainEvent> events)
    {
        PersonGenderChanges? state = null;
        foreach (var e in events)
        {
            switch (e.EventType)
            {
                case nameof(PersonHiredSucceeded):
                    var ph = e as PersonHiredSucceeded;
                    state = new(ph.AggregateId);
                    state.AddGender(ph.GenderId, ph.TimeStampRecorded);
                    break;

                case nameof(PersonChangedGender):
                    state.AddGender((e as PersonChangedGender).GenderId, e.TimeStampRecorded);
                    break;

                default:
                    if (e.AggregateType != nameof(Person))
                        throw new Exception("Wrong events");
                    break;
                    //throw new Exception("Wrong events");
            }
        }

        return state;
    }

    public class Gender
    {
        public Guid Id { get; private set; }
        public DateTime ChangeTime { get; private set; }

        public Gender(Guid id, long ticks)
        {
            Id = id;
            ChangeTime = new(ticks);
        }
    }
}
