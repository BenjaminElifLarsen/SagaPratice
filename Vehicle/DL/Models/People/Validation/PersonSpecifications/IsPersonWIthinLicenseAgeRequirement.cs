﻿using Common.SpecificationPattern;
using VehicleDomain.DL.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.Validation.PersonSpecifications;
internal class IsPersonWIthinLicenseAgeRequirement : ISpecification<AddPersonWithLicenseFromUser>
{
    private readonly IEnumerable<byte> _ageRequirements;

    public IsPersonWIthinLicenseAgeRequirement(IEnumerable<byte> LicenseAgeRequirements)
    {
        _ageRequirements = LicenseAgeRequirements;
    }

    public bool IsSatisfiedBy(AddPersonWithLicenseFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    private bool IsSatisfiedBy(DateTime birth)
    {
        var now = DateTime.Now;
        if(birth > now)
        {
            throw new Exception("Birth is after current moment.");
        }

        var age = (now.Year - birth.Year - 1) +
            (((now.Month > birth.Month) ||
            ((now.Month == birth.Month) && (now.Day >= birth.Day))) ? 1 : 0);

        foreach(var requirement in _ageRequirements)
        {
            if(age < requirement)
            {
                return false;
            }
        }
        return true;
    }
}