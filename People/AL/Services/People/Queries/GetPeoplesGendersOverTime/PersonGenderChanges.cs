using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.AL.Services.People.Queries.GetPeoplesGendersOverTime;
public sealed record PersonGenderChanges : ISingleProjection<PersonGenderChanges>, IMultiProjection<PersonGenderChanges>
{
    private IList<Gender> _genders;
    public Guid Id { get; private set; }
    public IEnumerable<Gender> GendersOverTime => _genders;
    public Guid CurrentGender { get; private set; }

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

    public static PersonGenderChanges? SingleProjection(IEnumerable<DomainEvent> events)
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

                case nameof(PersonFiredSucceeded):
                    var pf = e as PersonFiredSucceeded;
                    var deleteDate = new DateTime(pf.DeletedFrom.Year, pf.DeletedFrom.Month, pf.DeletedFrom.Day);
                    if (DateTime.Now >= deleteDate)
                    {
                        state = null;
                    }
                    break;

                default:
                    if (e.AggregateType != nameof(Person))
                        throw new Exception("Wrong events");
                    break;
            }
        }
        return state;
    }

    public static IEnumerable<PersonGenderChanges> MultiProjection(IEnumerable<DomainEvent> events)
    {
        Dictionary<Guid, PersonGenderChanges?> states = new();
        foreach (var e in events)
        {
            var found = states.TryGetValue(e.AggregateId, out PersonGenderChanges? state);
            switch (e.EventType)
            {
                case nameof(PersonHiredSucceeded):
                    var ph = e as PersonHiredSucceeded;
                    state = new(ph.AggregateId);
                    state.AddGender(ph.GenderId, ph.TimeStampRecorded);
                    states.Add(e.AggregateId, state); //if moving this (and the one in PersonFiredSucceeded) out it is possible to convrt the switch case to a method for DRY
                    break;

                case nameof(PersonChangedGender):
                    state.AddGender((e as PersonChangedGender).GenderId, e.TimeStampRecorded);
                    break;

                case nameof(PersonFiredSucceeded):
                    var pf = e as PersonFiredSucceeded;
                    var deleteDate = pf.DeletedFrom.ToDateTime(new TimeOnly());
                    if (DateTime.Now >= deleteDate)
                    {
                        state = null;
                        states.Remove(e.AggregateId);
                    }
                    break;

                default:
                    if (e.AggregateType != nameof(Person))
                        throw new Exception("Wrong events");
                    break;
            }
        }
        return states.Values.Where(x => x is not null);
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
