using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class DoesVehicleOperatorExist : ISpecification<AddVehicleWithOperators>, ISpecification<AddOperatorToVehicle>
{
    private readonly IEnumerable<IdReference> _operators;

    public DoesVehicleOperatorExist(VehicleValidationData validationData)
    {
        _operators = validationData.Operators;
    }

    public bool IsSatisfiedBy(AddOperatorToVehicle candidate)
    {
        return IsSatisfiedBy(candidate.OperatorId);
    }

    public bool IsSatisfiedBy(AddVehicleWithOperators candidate)
    {
        return candidate.Operators.All(IsSatisfiedBy);
    }

    private bool IsSatisfiedBy(int candidte)
    {
        return _operators.Any(x => x.Id == candidte);
    }
}
