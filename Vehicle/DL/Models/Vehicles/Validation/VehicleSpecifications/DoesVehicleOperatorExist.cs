using Common.SpecificationPattern;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries.ReadModels;

namespace VehicleDomain.DL.Models.Vehicles.Validation.VehicleSpecifications;
internal class DoesVehicleOperatorExist : ISpecification<BuyVehicleWithOperators>, ISpecification<AddOperatorToVehicle>
{
    private readonly IEnumerable<OperatorIdValidation> _operators;

    public DoesVehicleOperatorExist(VehicleValidationWithOperatorsData validationData)
    {
        _operators = validationData.Operators;
    }

    public bool IsSatisfiedBy(AddOperatorToVehicle candidate)
    {
        return IsSatisfiedBy(candidate.OperatorId);
    }

    public bool IsSatisfiedBy(BuyVehicleWithOperators candidate)
    {
        return candidate.Operators.All(IsSatisfiedBy);
    }

    private bool IsSatisfiedBy(int candidte)
    {
        return _operators.Any(x => x.Id == candidte);
    }
}
