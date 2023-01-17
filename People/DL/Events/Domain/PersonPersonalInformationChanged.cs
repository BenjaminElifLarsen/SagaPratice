using Common.Events.Domain;
using Common.Events.System;
using PersonDomain.DL.Models;

namespace PersonDomain.DL.Events.Domain;
public sealed record PersonPersonalInformationChangedSuccessed : SystemEvent
{ //mayhaps it should be a system event and then let the other events be the stored (so PersonGenderChanged etc.)
    public bool FirstNameWasChanged { get; private set; }
    public bool LastNameWasChanged { get; private set; }
    public bool BirthWasChanged { get; private set; }
    public bool GenderWasChanged { get; private set; }

    public PersonPersonalInformationChangedSuccessed(bool firstNameChanged, bool lastNameChanged, bool birthChanged, bool genderChanged, Guid correlationId, Guid causationId)
        : base(correlationId, causationId)
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

    public PersonPersonalInformationChangedFailed(IEnumerable<string> errors, Guid correlationId, Guid causationId) 
        : base(correlationId, causationId)
    {
        Errors = errors;
    }

}
