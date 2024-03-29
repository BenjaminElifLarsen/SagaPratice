﻿using Common.Events.State;

namespace PersonDomain.AL.ProcessManagers.Gender.Recognise.StateEvents;
public sealed record RecognisedSucceeded : StateEvent
{
    public int GenderId { get; set; }

    public RecognisedSucceeded(/*int Id,*/ Guid correlationId, Guid causationId) : base(correlationId, causationId)
    {
        //GenderId = Id;
    }
}
