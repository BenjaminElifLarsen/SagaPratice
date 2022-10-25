using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.Validation.OperatorSpecifications;
internal class IsOperatorToOld : ISpecification<AddPersonWithLicenseFromUser>, ISpecification<DateTime>
{
    private readonly byte _age;
    public IsOperatorToOld(byte age)
    {
        _age = age;
    }

    public bool IsSatisfiedBy(AddPersonWithLicenseFromUser candidate)
    {
        return IsSatisfiedBy(candidate.Birth);
    }

    public bool IsSatisfiedBy(DateTime candidate)
    {
        var now = DateTime.Now;
        var personAge = now.Year - candidate.Year - 1 +
        (now.Month > candidate.Month ||
        now.Month == candidate.Month && now.Day >= candidate.Day ? 1 : 0);
        return personAge <= _age;
    }
}
