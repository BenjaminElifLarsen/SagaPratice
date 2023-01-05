using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries.ReadModels;
internal record LicenseTypeValidation : BaseReadModel
{
    public Guid Id { get; private set; }

    public LicenseTypeValidation(Guid id)
    {
        Id = id;
    }
}
