//using Common.SpecificationPattern;
//using VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
//using l = VehicleDomain.DL.Models.Operators.CQRS.Commands.License; // Without this one, License would point to the model in the People folder.
//using lv = VehicleDomain.DL.Models.Operators.Validation.PersonCreationLicenseValidationData.LicenseValidationData;

//namespace VehicleDomain.DL.Models.Operators.Validation.LicenseSpecifications;

//internal class IsLicenseLicenseTypeValid : ISpecification<l>
//{
//    private readonly IEnumerable<LicenseTypeIdValidation> _permittedIds;
//    public IsLicenseLicenseTypeValid(IEnumerable<LicenseTypeIdValidation> permittedIds)
//    {
//        _permittedIds = permittedIds;
//    }

//    public bool IsSatisfiedBy(l candidate)
//    {
//        return _permittedIds.Any(x => x.Id == candidate.LicenseTypeId);
//    }
//}
