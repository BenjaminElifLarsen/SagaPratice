using Common.BinaryFlags;
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
		flag += new IsLicenseTypeAgeRequirementValid(10).IsSatisfiedBy(_licenseType) ? 0 : LicenseTypeErrors.InvalidAgeRequirement;
		flag += new IsLicenseTypeTypeValid().IsSatisfiedBy(_licenseType) ? 0 : LicenseTypeErrors.InvalidType;
		flag += new IsLicenseTypeRenewPeriodValid(2).IsSatisfiedBy(_licenseType) ? 0 : LicenseTypeErrors.InvalidRenewPeriod;
		return flag;
	}
}


internal class LicenseTypeChangeInformationValidator : IValidate
{
	private readonly AlterLicenseType _licenseType;
	public LicenseTypeChangeInformationValidator(AlterLicenseType licenseType)
	{
		_licenseType = licenseType;
	}

    public BinaryFlag Validate()
    {
		BinaryFlag flag = new();
        flag += new IsLicenseTypeAgeRequirementValid(10).IsSatisfiedBy(_licenseType) ? 0 : LicenseTypeErrors.InvalidAgeRequirement;
        flag += new IsLicenseTypeTypeValid().IsSatisfiedBy(_licenseType) ? 0 : LicenseTypeErrors.InvalidType;
        flag += new IsLicenseTypeRenewPeriodValid(2).IsSatisfiedBy(_licenseType) ? 0 : LicenseTypeErrors.InvalidRenewPeriod;
        return flag;
    }
}
