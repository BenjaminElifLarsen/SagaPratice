﻿using Common.ResultPattern;
using PeopleDomain.AL.Services.Genders.Queries;
using PeopleDomain.AL.Services.Genders.Queries.GetDetails;

namespace PeopleDomain.AL.Services.Genders;
public partial class GenderService
{
    public async Task<Result<GenderDetails>> GetGenderDetailsAsync(int id)
    {
        var details = await _genderRepository.GetAsync(id, new GenderDetailsQuery());
        if(details is null)
        {
            return new NotFoundResult<GenderDetails>("Not found.");
        }
        return new SuccessResult<GenderDetails>(details);
    }
}