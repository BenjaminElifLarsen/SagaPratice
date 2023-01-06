using Common.Events.Domain;
using Common.Events.System;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonRemovedFromGenderFailed : SystemEvent
{
	public IEnumerable<string> Errors { get; private set; }
	public PersonRemovedFromGenderFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId)
		: base(correlationId, causationId)
	{
		Errors = errors;
	}
}
