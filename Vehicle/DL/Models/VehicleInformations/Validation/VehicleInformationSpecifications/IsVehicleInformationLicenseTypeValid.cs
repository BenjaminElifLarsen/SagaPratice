using Common.SpecificationPattern;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;

namespace VehicleDomain.DL.Models.VehicleInformations.Validation.VehicleInformationSpecifications;
internal class IsVehicleInformationLicenseTypeValid : ISpecification<AddVehicleInformationFromSystem>
{
    public IsVehicleInformationLicenseTypeValid(VehicleInformationValidationData validationData)
    {

    }

    public bool IsSatisfiedBy(AddVehicleInformationFromSystem candidate)
    {
        throw new NotImplementedException();
    }
}
