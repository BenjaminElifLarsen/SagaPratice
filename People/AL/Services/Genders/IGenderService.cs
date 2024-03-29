﻿using Common.Events.Save;
using Common.ResultPattern;
using PersonDomain.AL.Services.Genders.Queries.GetDetails;
using PersonDomain.AL.Services.Genders.Queries.GetDetailsOverTime;
using PersonDomain.AL.Services.Genders.Queries.GetList;
using PersonDomain.DL.CQRS.Commands;

namespace PersonDomain.AL.Services.Genders;
public interface IGenderService
{
    Task<Result<IEnumerable<GenderListItem>>> GetGenderListAsync();
    Task<Result<IEnumerable<GenderOverTime>>> GetGenderDataPointsOverTimeAsync();
    Task<Result<GenderDetails>> GetGenderDetailsAsync(Guid id);
    Task<Result> UnrecogniseGenderAsync(UnrecogniseGender command);
    Task<Result> RecogniseGenderAsync(RecogniseGender command);
    void Handle(ProcessingSucceeded @event);
    void Handle(ProcessingFailed @event);
}
