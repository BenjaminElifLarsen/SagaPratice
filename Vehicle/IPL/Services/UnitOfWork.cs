using Common.Events.Domain;
using VehicleDomain.AL.Busses.Event;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;
using VehicleDomain.IPL.Context;

namespace VehicleDomain.IPL.Services;
internal class UnitOfWork : IUnitOfWork
{
    private readonly ILicenseTypeRepository _licenseTypeRepository;
    private readonly IOperatorRepository _operatorRepository;
    private readonly IVehicleInformationRepository _vehicleInformationRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IVehicleDomainEventBus _eventBus;
    private readonly IVehicleContext _context;

    public ILicenseTypeRepository LicenseTypeRepository => _licenseTypeRepository;

    public IOperatorRepository OperatorRepository => _operatorRepository;

    public IVehicleInformationRepository VehicleInformationRepository => _vehicleInformationRepository;

    public IVehicleRepository VehicleRepository => _vehicleRepository;

    public UnitOfWork(ILicenseTypeRepository licenseTypeRepository, IOperatorRepository operatorRepository, IVehicleInformationRepository vehicleInformationRepository, IVehicleRepository vehicleRepository, IVehicleDomainEventBus eventBus, IVehicleContext context)
    {
        _licenseTypeRepository = licenseTypeRepository;
        _operatorRepository = operatorRepository;
        _vehicleInformationRepository = vehicleInformationRepository;
        _vehicleRepository = vehicleRepository;
        _eventBus = eventBus;
        _context = context;
    }

    public void Save()
    {
        var roots = _context.GetTracked.ToArray();
        for (int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
        {
            for (int n = 0; n < roots[i].Events.Count(); n++)
            {
                _eventBus.Publish(roots[i].Events.ToArray()[n]);
                roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
            }
        }
        _context.Save();
    }

    public void AddOrphanEvnet(IDomainEvent @event)
    {
        _context.Add(@event);
    }

    public void ProcessEvents()
    {
        do
        {
            var roots = _context.GetTracked.ToArray();
            for (int i = 0; i < roots.Length; i++) //if wanting to multithread this, there is Parallel. Might be more useful for the integrate event bus
            {
                for (int n = 0; n < roots[i].Events.Count(); n++) //does not work correctly as n goes up and event count goes down
                {
                    _eventBus.Publish(roots[i].Events.ToArray()[n]); //the add and update method in the repository should add them to the event store
                    roots[i].RemoveDomainEvent(roots[i].Events.ToArray()[n]);
                }
            }
        } while (_context.GetTracked.SelectMany(x => x.Events).Any());
        var eventsArray = _context.OrphanEvents.ToArray();
        foreach (var @event in eventsArray)
        {
            _eventBus.Publish(@event);
            _context.Remove(@event);
        }
    }
}
