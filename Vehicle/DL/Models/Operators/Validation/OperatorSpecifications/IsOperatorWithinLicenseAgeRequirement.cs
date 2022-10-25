using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.People.Validation;

namespace VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;
internal class IsOperatorWithinLicenseAgeRequirement : ISpecification<AddPersonWithLicenseFromUser>
{
    private readonly OperatorValidationData _ageRequirements;

    public IsOperatorWithinLicenseAgeRequirement(OperatorValidationData LicenseAgeRequirements)
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
        if (birth > now)
        {
            throw new Exception("Birth is after current moment.");
        }

        var age = now.Year - birth.Year - 1 +
            (now.Month > birth.Month ||
            now.Month == birth.Month && now.Day >= birth.Day ? 1 : 0);

        foreach (var requirement in _ageRequirements.LicenseTypes)
        {
            if (age < requirement.YearRequirement)
            {
                return false;
            }
        }
        return true;
    }
}
