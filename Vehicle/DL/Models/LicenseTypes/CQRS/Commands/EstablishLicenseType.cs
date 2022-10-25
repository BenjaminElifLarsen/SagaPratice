using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
internal class EstablishLicenseTypeFromUser : ICommand
{
    public string Type { get; private set; }
    public byte RenewPeriod { get; private set; }
    public byte AgeRequirement { get; private set; }

    public EstablishLicenseTypeFromUser(string type, byte renewPeriod, byte ageRequirement)
    {
        Type = type;
        RenewPeriod = renewPeriod;
        AgeRequirement = ageRequirement;
    }
}
