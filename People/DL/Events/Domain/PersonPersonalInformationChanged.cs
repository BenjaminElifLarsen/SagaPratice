using Common.Events.Domain;
using Common.Events.System;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonPersonalInformationChangedSuccessed : DomainEvent
{ //mayhaps it should be a system event and then let the other events be the stored (so PersonGenderChanged etc.)
    public bool FirstNameWasChanged { get; private set; }
    public bool LastNameWasChanged { get; private set; }
    public bool BirthWasChanged { get; private set; }
    public bool GenderWasChanged { get; private set; }

    public PersonPersonalInformationChangedSuccessed(Person aggregate, bool firstNameChanged, bool lastNameChanged, bool birthChanged, bool genderChanged, Guid correlationId, Guid causationId)
        : base(aggregate, correlationId, causationId)
    {
        FirstNameWasChanged = firstNameChanged;
        LastNameWasChanged = lastNameChanged;
        BirthWasChanged = birthChanged;
        GenderWasChanged = genderChanged;    
    }
}

public sealed record PersonPersonalInformationChangedFailed : SystemEvent
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public Guid AggregateId { get; private set; }

    public PersonPersonalInformationChangedFailed(Person aggregate, IEnumerable<string> errors, Guid correlationId, Guid causationId) 
        : base(correlationId, causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.Id;
        Errors = errors;
    }

    public PersonPersonalInformationChangedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        AggregateType = typeof(Person).Name;
        AggregateId = Guid.Empty;
        Errors = errors;
    }
}
