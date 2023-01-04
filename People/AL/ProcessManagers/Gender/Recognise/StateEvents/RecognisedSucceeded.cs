using Common.Events.System;

namespace PeopleDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
public sealed record RecognisedSucceeded : SystemEvent
{
    public int GenderId { get; set; }

    public RecognisedSucceeded(/*int Id,*/ Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        //GenderId = Id;
    }
}
