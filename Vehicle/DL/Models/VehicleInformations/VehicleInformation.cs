using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.VehicleInformations;
public class VehicleInformation : IAggregateRoot
{
    private int _vehicleInformationId;
    private string _name;
    private byte _maxWheelAmount;
    private IdReference _licenseTypeRequired;
    //could have a producer model, might be its own domain. If added, no reason to let user create vehicle informations, only the Producer domain can trigger that
    internal int VehicleInformationId { get => _vehicleInformationId; private set => _vehicleInformationId = value; }
    internal string Name { get => _name; private set => _name = value; }
    internal IdReference LicenseTypeRequired { get => _licenseTypeRequired; private set => _licenseTypeRequired = value; }
    internal byte MaxWheelAmount { get => _maxWheelAmount; private set => _maxWheelAmount = value; }

    private VehicleInformation()
    { //it does not make sense to be able to update amount of wheeels after adding the entity, but what about name?
        //it might have to depend on if the data is user entered or from an external system, e.g. the vehicle produce
        //this really is a domain expert question. A company software that knows the people working there and what driving license they have, e.g. for driver renting??? 
    }

    internal VehicleInformation( string name, byte maxWheelAmount, IdReference licenseTypeRequired)
    {
        _vehicleInformationId = RandomValue.GetValue;
        _name = name;
        _licenseTypeRequired = licenseTypeRequired;
        _maxWheelAmount = maxWheelAmount;
    }

}
