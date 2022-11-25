using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class ValidateOperatorLicenseStatus : ICommand
{
    public int OperatorId { get; private set; }
    public int TypeId { get; private set; }
}

public class ValidateOperatorLicenses : ICommand
{ //person id
    public int Id { get; private set; }
}


public class ValidateLicenseAgeRequirementBecauseChange : ICommand
{
    public int LicenseTypeId { get; private set; }
    public byte AgeRequirement { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    public ValidateLicenseAgeRequirementBecauseChange(int licenseTypeId, byte ageRequirement, IEnumerable<int> operatorIds)
    {
        LicenseTypeId = licenseTypeId;
        AgeRequirement = ageRequirement;
        OperatorIds = operatorIds;
    }
}

public class ValidateLicenseRenewPeriodBecauseChange : ICommand
{
    public int LicenseTypeId { get; private set; }
    public byte RenewPeriod { get; private set; }
    public IEnumerable<int> OperatorIds { get; private set; }

    public ValidateLicenseRenewPeriodBecauseChange(int licenseTypeId, byte renewPeriod, IEnumerable<int> operatorIds)
    {
        LicenseTypeId = licenseTypeId;
        RenewPeriod = renewPeriod;
        OperatorIds = operatorIds;
    }
}