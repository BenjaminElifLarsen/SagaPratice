﻿using Common.Events.Domain;
using PeopleDomain.DL.Models;

namespace PeopleDomain.DL.Events.Domain;
public class PersonPersonalInformationChangedSuccessed : IDomainEventSuccess<PersonPersonalInformationChangedSuccessedData>
{
    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public PersonPersonalInformationChangedSuccessedData Data { get; private set; }

public PersonPersonalInformationChangedSuccessed(Person aggregate, Guid correlationId, Guid causationId, bool firstNameChanged, bool lastNameChanged, bool birthChanged, bool genderChanged)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Data = new(firstNameChanged, lastNameChanged, birthChanged, genderChanged);
    }
}

public class PersonPersonalInformationChangedSuccessedData
{
    public bool FirstNameWasChanged { get; private set; }
    public bool LastNameWasChanged { get; private set; }
    public bool BirthWasChanged { get; private set; }
    public bool GenderWasChanged { get; private set; }

    public PersonPersonalInformationChangedSuccessedData(bool firstName, bool lastName, bool birth, bool gender)
    {
        FirstNameWasChanged = firstName;
        LastNameWasChanged = lastName;
        BirthWasChanged = birth;
        GenderWasChanged = gender;
    }
}

public class PersonPersonalInformationChangedFailed : IDomainEventFail
{
    public IEnumerable<string> Errors { get; private set; }

    public string AggregateType { get; private set; }

    public int AggregateId { get; private set; }

    public string EventType { get; private set; }

    public Guid EventId { get; private set; }

    public long TimeStampRecorded { get; private set; }

    public Guid CorrelationId { get; private set; }

    public Guid CausationId { get; private set; }

    public PersonPersonalInformationChangedFailed(Person aggregate, IEnumerable<string> errors, Guid correlationId, Guid causationId)
    {
        AggregateType = aggregate.GetType().Name;
        AggregateId = aggregate.PersonId;
        EventType = GetType().Name;
        EventId = Guid.NewGuid();
        TimeStampRecorded = DateTime.Now.Ticks;
        CorrelationId = correlationId;
        CausationId = causationId;
        Errors = errors;
    }
}
