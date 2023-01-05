using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
internal record LicenseTypeAgeValidation : BaseReadModel
{
    public Guid Id { get; private set; }
    public byte YearRequirement { get; private set; }

    public LicenseTypeAgeValidation(byte yearRequirement, Guid id)
    {
        YearRequirement = yearRequirement;
        Id = id;
    }
}
