﻿using Common.Events.Domain;
using Common.Events.Projection;
using PersonDomain.DL.Events.Domain;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.CQRS.Queries.Events.ReadModels;
public sealed record GenderSubject : ISingleProjection<GenderSubject>, IMultiProjection<GenderSubject>
{
    public string Subject { get; private set; }
	public int TestAmountOfPeopleRemovedFromGender { get; private set; }
	public int TestAmountOfPeopleAddedToGender { get; private set; }

	private GenderSubject()
	{

	}

	public GenderSubject(string subject)
	{
		Subject = subject;
	}

	public static GenderSubject? SingleProjection(IEnumerable<DomainEvent> events)
	{ //maybe an extension method that can convert the above list to a given model via a 'projection' (similar to the hydrate method) method
		GenderSubject? state = null;
		foreach(var e in events) 
		{
			switch (e.EventType)
			{
				case nameof(GenderRecognisedSucceeded):
					state = new((e as GenderRecognisedSucceeded).Subject);
					break;

				case nameof(GenderUnrecognisedSucceeded):
					state = null;
					break;

				case nameof(PersonAddedToGenderSucceeded):
                    state.TestAmountOfPeopleAddedToGender++; 
					break;

				case nameof(PersonRemovedFromGenderSucceeded):
					state.TestAmountOfPeopleRemovedFromGender++;
					break;

				default:
					if(e.AggregateType != nameof(Gender))
						throw new Exception("Wrong events");
					break;
			}
		}
		return state;
	} 

	public static IEnumerable<GenderSubject> MultiProjection(IEnumerable<DomainEvent> events)
	{
		Dictionary<Guid, GenderSubject?> states = new();
        foreach (var e in events)
        {
            var found = states.TryGetValue(e.AggregateId, out GenderSubject? state);

            switch (e.EventType)
            {
                case nameof(GenderRecognisedSucceeded):
                    state = new((e as GenderRecognisedSucceeded).Subject);
                    break;

                case nameof(GenderUnrecognisedSucceeded):
                    state = null;
                    break;

                case nameof(PersonAddedToGenderSucceeded):
                    state.TestAmountOfPeopleAddedToGender++;
                    break;

                case nameof(PersonRemovedFromGenderSucceeded):
                    state.TestAmountOfPeopleRemovedFromGender++;
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
}
