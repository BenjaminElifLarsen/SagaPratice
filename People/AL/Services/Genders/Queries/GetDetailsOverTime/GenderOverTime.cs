using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.AL.Services.Genders.Queries.GetDetailsOverTime;
public sealed record GenderOverTime : ISingleProjection<GenderOverTime>, IMultiProjection<GenderOverTime>
{
    public Guid Id { get; private set; }
    public string Term { get; private set; }
    public DateTime Recognised { get; private set; }
    //public int TestAmountOfPeopleRemovedFromGender { get; private set; }
    //public int TestAmountOfPeopleAddedToGender { get; private set; }
    public IList<GenderDataPoint> DataPoints { get; private set; }
    private GenderOverTime()
    {

    }

    public GenderOverTime(Guid id, string subject, string @object, long recognised)
    {
        Id = id;
        Term = subject + "|" + @object;
        DataPoints = new List<GenderDataPoint>();
        Recognised = new(recognised);
    }

    public static GenderOverTime? SingleProjection(IEnumerable<DomainEvent> events)
    { //maybe an extension method that can convert the above list to a given model via a 'projection' (similar to the hydrate method) method
        GenderOverTime? state = null;
        foreach (var e in events)
        {
            switch (e.EventType)
            {
                case nameof(GenderRecognisedSucceeded):
                    var gr = e as GenderRecognisedSucceeded;
                    state = new(gr.AggregateId, gr.Subject, gr.Object, gr.TimeStampRecorded);
                    break;

                case nameof(GenderUnrecognisedSucceeded):
                    state = null;
                    break;

                case nameof(PersonAddedToGenderSucceeded):
                    state.DataPoints.Add(new(e.TimeStampRecorded, true));
                    break;

                case nameof(PersonRemovedFromGenderSucceeded):
                    state.DataPoints.Add(new(e.TimeStampRecorded, false));
                    break;

                default:
                    if (e.AggregateType != nameof(Gender))
                        throw new Exception("Wrong events");
                    break;
            }
        }
        return state;
    }

    public static IEnumerable<GenderOverTime> MultiProjection(IEnumerable<DomainEvent> events)
    {
        Dictionary<Guid, GenderOverTime?> states = new();
        foreach (var e in events)
        {
            var found = states.TryGetValue(e.AggregateId, out GenderOverTime? state);

            switch (e.EventType)
            {
                case nameof(GenderRecognisedSucceeded):
                    var gr = e as GenderRecognisedSucceeded;
                    state = new(gr.AggregateId, gr.Subject, gr.Object, gr.TimeStampRecorded);
                    break;

                case nameof(GenderUnrecognisedSucceeded):
                    state = null;
                    states.Remove(e.AggregateId);
                    break;

                case nameof(PersonAddedToGenderSucceeded):
                    state.DataPoints.Add(new(e.TimeStampRecorded, true));
                    break;

                case nameof(PersonRemovedFromGenderSucceeded):
                    state.DataPoints.Add(new(e.TimeStampRecorded, false));
                    break;

                default:
                    if (e.AggregateType != nameof(Gender))
                        throw new Exception("Wrong events");
                    break;
            }
            if (!found)
            {
                states.Add(e.AggregateId, state);
            }
        }

        return states.Values.Where(x => x is not null);
    }

    public record GenderDataPoint
    {
        public DateTime Recorded { get; private set; }
        public bool Added { get; private set; }

        public GenderDataPoint(long recorded, bool added)
        {
            Recorded = new(recorded);
            Added = added;
        }
    }
}
