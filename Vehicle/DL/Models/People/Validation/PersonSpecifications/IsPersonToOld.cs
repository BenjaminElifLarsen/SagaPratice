using Common.SpecificationPattern;
using VehicleDomain.DL.Models.People.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.Validation.PersonSpecifications;
internal class IsPersonToOld : ISpecification<AddPersonWithLicenseFromUser>
{
    private readonly byte _age;
    public IsPersonToOld(byte age)
    {
        _age = age;
    }

    public bool IsSatisfiedBy(AddPersonWithLicenseFromUser candidate)
    {
        var now = DateTime.Now;
        var personAge = (now.Year - candidate.Birth.Year - 1) +
        (((now.Month > candidate.Birth.Month) ||
        ((now.Month == candidate.Birth.Month) && (now.Day >= candidate.Birth.Day))) ? 1 : 0);
        return personAge < _age;
    }
}
