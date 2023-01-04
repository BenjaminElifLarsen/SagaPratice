using Common.Events.Domain;
using Common.Events.System;
using Common.ProcessManager;
using Common.ResultPattern;
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
    private readonly IEnumerable<IProcessManager> _processManagers;

    public ILicenseTypeRepository LicenseTypeRepository => _licenseTypeRepository;

    public IOperatorRepository OperatorRepository => _operatorRepository;

    public IVehicleInformationRepository VehicleInformationRepository => _vehicleInformationRepository;

    public IVehicleRepository VehicleRepository => _vehicleRepository;

    public UnitOfWork(ILicenseTypeRepository licenseTypeRepository, IOperatorRepository operatorRepository, IVehicleInformationRepository vehicleInformationRepository, IVehicleRepository vehicleRepository, IVehicleDomainEventBus eventBus, IVehicleContext context, IEnumerable<IProcessManager> processManagers)
    {
        _licenseTypeRepository = licenseTypeRepository;
        _operatorRepository = operatorRepository;
        _vehicleInformationRepository = vehicleInformationRepository;
        _vehicleRepository = vehicleRepository;
        _eventBus = eventBus;
        _context = context;
        _processManagers = processManagers;
    }
    private void Save(ProcesserFinished @event)
    {
        if (@event.Result is SuccessResultNoData)
        { 
            _context.Save(); 
        } 
    } 

    public void Save()
    {
        var pm = _processManagers.SingleOrDefault(x => x.CorrelationId != default);
        if (pm is not null)
        {
            pm?.RegistrateHandler(Save);
        }
        ProcessEvents();
        if (pm is null)
        {
            _context.Save();
        }
    }

    public void AddSystemEvent(SystemEvent @event)
    {
        _context.Add(@event);
    }

    public void ProcessEvents()
    {
        do
        {
            var eventsArray = _context.SystemEvents.ToArray();
            foreach (var @event in eventsArray)
            {
                _eventBus.Publish(@event);
                _context.Remove(@event);
            }
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
        
    }
}
