using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
public class EstablishLicenseTypeFromUser : ICommand
{
    public string Type { get; private set; }
    public byte RenewPeriod { get; private set; }
    public byte AgeRequirement { get; private set; }

    public Guid CommandId { get; private set; }

    public Guid CorrelationId => CommandId;

    public Guid CausationId => CommandId;

    public EstablishLicenseTypeFromUser(string type, byte renewPeriod, byte ageRequirement)
    {
        Type = type;
        RenewPeriod = renewPeriod;
        AgeRequirement = ageRequirement;
        CommandId = Guid.NewGuid();
    }
}
