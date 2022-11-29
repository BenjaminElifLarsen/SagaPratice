using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
//using l = VehicleDomain.DL.Models.Operators.CQRS.Commands.License; // Without this one, License would point to the model in the People folder.

namespace VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;
internal class IsLicenseLicenseTypeSet : ISpecification<AddLicenseToOperator>/*, ISpecification<l>*/
{
    public bool IsSatisfiedBy(AddLicenseToOperator candidate)
    {
        return candidate.LicenseType != 0;
    }

    //public bool IsSatisfiedBy(l candidate)
    //{
    //    return candidate.LicenseTypeId != 0;
    //}
}
