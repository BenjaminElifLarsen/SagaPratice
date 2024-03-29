﻿using Common.ResultPattern;
using PersonDomain.AL.Services.People.Queries;
using PersonDomain.AL.Services.People.Queries.GetDetails;
using PersonDomain.DL.CQRS.Queries.Events;

namespace PersonDomain.AL.Services.People;
public partial class PersonService
{
    public async Task<Result<PersonDetails>> GetPersonDetailsAsync(Guid id)
    {
        var details = await _unitOfWork.PersonRepository.GetAsync(id, new PersonDetailsQuery());;
        if(details is null)
        {
            return new NotFoundResult<PersonDetails>("Not found.");
        }
        return new SuccessResult<PersonDetails>(details);
    }
}