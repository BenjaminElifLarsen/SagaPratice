using Common.CQRS.Queries;

namespace VehicleDomain.DL.Models.Operators.CQRS.Queries.ReadModels;
internal class LicenseTypeIdValidation : BaseReadModel
{
    public int Id { get; private set; }

    public LicenseTypeIdValidation(int id)
    {
        Id = id;
    }
}
