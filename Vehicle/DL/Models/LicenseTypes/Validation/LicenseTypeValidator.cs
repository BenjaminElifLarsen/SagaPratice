using Common.Other;
using Common.SpecificationPattern;
using VehicleDomain.DL.Errors;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.LicenseTypes.Validation.Specifications;

namespace VehicleDomain.DL.Models.LicenseTypes.Validation;
internal class LicenseTypeValidator : IValidate
{
	private readonly EstablishLicenseTypeFromUser _licenseType;
	public LicenseTypeValidator(EstablishLicenseTypeFromUser licenseType)
	{
		_licenseType = licenseType;
	}

	public BinaryFlag Validate()
	{
		BinaryFlag flag = new(); 
		if(new IsLicenseTypeAgeRequirementValid(10).IsSatisfiedBy(_licenseType))
			flag.AddFlag((int)LicenseTypeErrors.InvalidAgeRequirement);
		if (new IsLicenseTypeTypeValid().IsSatisfiedBy(_licenseType))
			flag.AddFlag((int)LicenseTypeErrors.InvalidType);
		if (new IsLicenseTypeRenewPeriodValid(2).IsSatisfiedBy(_licenseType))
			flag.AddFlag((int)LicenseTypeErrors.InvalidRenewPeriod);
		return flag;
	}
}
