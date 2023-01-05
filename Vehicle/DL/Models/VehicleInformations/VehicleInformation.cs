using Common.Events.Domain;
using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.VehicleInformations;
public class VehicleInformation : IAggregateRoot
{
    private Guid _id;
    private string _name;
    private byte _maxWheelAmount;
    private IdReference _licenseTypeRequired;
    private HashSet<IdReference> _vehicles;
    private readonly HashSet<DomainEvent> _events;
    //could have a producer model, might be its own domain. If added, no reason to let user create vehicle informations, only the Producer domain can trigger that
    internal string Name { get => _name; private set => _name = value; }
    internal Guid LicenseTypeRequired { get => _licenseTypeRequired; private set => _licenseTypeRequired = value; }
    internal byte MaxWheelAmount { get => _maxWheelAmount; private set => _maxWheelAmount = value; }
    internal IEnumerable<Guid> Vehicles => _vehicles.Select(x => x.Id);

    public Guid Id { get => _id; private set => _id = value; }
    public IEnumerable<DomainEvent> Events => _events;

    private VehicleInformation()
    { //it does not make sense to be able to update amount of wheeels after adding the entity, but what about name?
        //it might have to depend on if the data is user entered or from an external system, e.g. the vehicle produce
        //this really is a domain expert question. A company's internal software that knows the people working there and what driving license they have, e.g. for driver renting??? 
        _events = new();
    }

    internal VehicleInformation(string name, byte maxWheelAmount, Guid licenseTypeRequired)
    {
        _id = Guid.NewGuid();
        _name = name;
        _licenseTypeRequired = new(licenseTypeRequired);
        _maxWheelAmount = maxWheelAmount;
        _vehicles = new();
        _events = new();
    }

    internal bool RegistrateVehicle(Guid vehicle)
    {
        return _vehicles.Add(vehicle);
    }

    internal bool UnregistrateVehicle(Guid vehicle)
    {
        return _vehicles.Remove(vehicle);
    }

    public void AddDomainEvent(DomainEvent eventItem)
    {
        if (_id == eventItem.AggregateId) //should cause an expection if this fails
            _events.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        if (_id == eventItem.AggregateId) //should cause an expection if this fails
            _events.Remove(eventItem);
    }
}
