﻿using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
//using l = VehicleDomain.DL.Models.Operators.CQRS.Commands.License; // Without this one, License would point to the model in the People folder.
using lv = VehicleDomain.DL.Models.Operators.Validation.PersonCreationLicenseValidationData.LicenseValidationData;

namespace VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;
internal class IsLicenseArquiredValid : ISpecification<AddLicenseToOperator>//, ISpecification<l>
{
    private readonly byte _yearRequirement;

    public IsLicenseArquiredValid(LicenseValidationData licenseType)
    {
        _yearRequirement = licenseType.LicenseYearRequirement.YearRequirement;
    }

    public IsLicenseArquiredValid(lv licenseType)
    {
        _yearRequirement = licenseType.AgeRequirement.YearRequirement;
    }

    public bool IsSatisfiedBy(AddLicenseToOperator candidate)
    {
        return IsSatisfiedBy(candidate.Arquired);
    }

    //public bool IsSatisfiedBy(l candidate)
    //{
    //    return IsSatisfiedBy(candidate.Arquired);
    //}

    public bool IsSatisfiedBy(DateTime candidate)
    {
        var now = DateTime.Now;
        var age = now.Year - candidate.Year - 1 +
        (now.Month > candidate.Month ||
        now.Month == candidate.Month && now.Day >= candidate.Day ? 1 : 0);
        return age < _yearRequirement;
    }
}
