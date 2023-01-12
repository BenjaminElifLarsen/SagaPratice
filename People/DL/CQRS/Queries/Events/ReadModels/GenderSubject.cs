using Common.CQRS.Queries;
using Common.Events.Domain;
using PersonDomain.DL.Events.Domain;

namespace PersonDomain.DL.CQRS.Queries.Events.ReadModels;
public sealed record GenderSubject : BaseReadModel
{
    public string Subject { get; private set; }
	public int TestAmountOfPeopleRemovedFromGender { get; private set; }
	public GenderSubject(string subject)
	{
		Subject = subject;
	}

	private GenderSubject()
	{

	}

	public static GenderSubject? Projection(IEnumerable<DomainEvent> events)
	{
		GenderSubject? state = null;
		foreach(var e in events) 
		{
			switch (e.EventType)
			{
				case nameof(GenderRecognisedSucceeded) :
					state = new((e as GenderRecognisedSucceeded).Subject);
					break;

				case nameof(GenderUnrecognisedSucceeded):
					state = null;
					break;

				case nameof(PersonAddedToGenderSucceeded): break;

				case nameof(PersonRemovedFromGenderSucceeded):
					state.TestAmountOfPeopleRemovedFromGender++;
					break;

				default:
					throw new Exception("Wrong events");
					//break;//have the other gender event, hitting this one is an expection
			}
		}
		return state;
	} 
}
