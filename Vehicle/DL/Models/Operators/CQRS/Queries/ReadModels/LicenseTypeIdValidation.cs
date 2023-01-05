using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
internal record LicenseTypeIdValidation : BaseReadModel
{
    public Guid Id { get; private set; }

    public LicenseTypeIdValidation(Guid id)
    {
        Id = id;
    }
}
