﻿using Common.SpecificationPattern;
using PeopleDomain.DL.CQRS.Commands;

namespace PeopleDomain.DL.Validation.GenderSpecifications;

internal class IsGenderVerbObjectSat : ISpecification<PermitGender>
{
    public bool IsSatisfiedBy(PermitGender candidate)
    {
        return IsSatisfiedBy(candidate.VerbObject);
    }

    private bool IsSatisfiedBy(string candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate);
    }
}