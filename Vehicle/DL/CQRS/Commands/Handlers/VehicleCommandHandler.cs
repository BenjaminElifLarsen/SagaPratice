using Common.Other;
using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Queries;
using VehicleDomain.DL.Models.Operators.Validation;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Commands;
using VehicleDomain.DL.Models.VehicleInformations.CQRS.Queries;
using VehicleDomain.DL.Models.VehicleInformations.Validation;
using VehicleDomain.DL.Models.Vehicles;
using VehicleDomain.DL.Models.Vehicles.CQRS.Commands;
using VehicleDomain.DL.Models.Vehicles.CQRS.Queries;
using VehicleDomain.DL.Models.Vehicles.Validation;
using VehicleDomain.DL.Models.Vehicles.Validation.Errors;

namespace VehicleDomain.DL.CQRS.Commands.Handlers;
internal class VehicleCommandHandler : IVehicleCommandHandler
{
    private readonly IVehicleFactory _vehicleFactory;
    private readonly IVehicleRepository _vehicleRepository;

    private readonly IOperatorFactory _operatorFactory;
    private readonly IOperatorRepository _operatorRepository;

    private readonly ILicenseTypeFactory _licenseTypeFactory;
    private readonly ILicenseTypeRepository _licenseTypeRepository;

    private readonly IVehicleInformationFactory _vehicleInformationFactory;
    private readonly IVehicleInformationRepository _vehicleInformationRepository;

    public VehicleCommandHandler(IOperatorFactory operatorFactory, IOperatorRepository operatorRepository, 
        ILicenseTypeFactory licenseTypeFactory, ILicenseTypeRepository licenseTypeRepository, 
        IVehicleInformationFactory vehicleInformationFactory, IVehicleInformationRepository vehicleInformationRepository,
        IVehicleFactory vehicleFactory, IVehicleRepository vehicleRepository)
    {
        _vehicleFactory = vehicleFactory;
        _vehicleRepository = vehicleRepository;
        _operatorFactory = operatorFactory;
        _operatorRepository = operatorRepository;
        _licenseTypeFactory = licenseTypeFactory;
        _licenseTypeRepository = licenseTypeRepository;
        _vehicleInformationFactory = vehicleInformationFactory;
        _vehicleInformationRepository = vehicleInformationRepository;
    }


    public Result Handle(ValidateDriverLicenseStatus command)
    { //make a valdiation check, after this (if successfull) the user should use a query to get the result
        //this method could be called by a system designed to automatically ensure everyones licenses are up to date.
        var @operator = _operatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if(@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        var license = @operator.GetLicense(command.TypeId);
        if(license is null)
        {
            return new InvalidResultNoData($"No license with type of {command.TypeId} was found.");
        }
        var oldValue = license.Expired;
        license.CheckIfExpired();
        if(oldValue != license.Expired)
        {
            _operatorRepository.Update(@operator);
            _operatorRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddOperatorNoLicenseFromSystem command)
    { //will need to trigger an event, PersonCreated
        if (!_operatorRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Operator already exist.");
        }
        var result = _operatorFactory.CreateOperator(command);
        if(result is InvalidResult<Operator>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _operatorRepository.Create(result.Data);
        try
        {
            _operatorRepository.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddOperatorWithLicenseFromUser command)
    {//will need to trigger an event, PersonCreated
        if (!_operatorRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Operator already exist.");
        }
        var licenseTypeAges = _licenseTypeRepository.AllAsync(new LicenseTypeAgeQuery()).Result;
        var licenseTypeIds = _licenseTypeRepository.AllAsync(new LicenseTypeIdQuery()).Result;
        OperatorValidationData data = new(licenseTypeAges);
        var dictionary = new Dictionary<int, PersonCreationLicenseValidationData.LicenseValidationData>();
        foreach(var licenseType in licenseTypeAges)
        {
            dictionary.Add(licenseType.Id, new PersonCreationLicenseValidationData.LicenseValidationData(licenseType));
        }
        PersonCreationLicenseValidationData licenseData = new(dictionary, licenseTypeIds);
        var result = _operatorFactory.CreateOperator(command, data, licenseData);
        if (result is InvalidResult<Operator>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _operatorRepository.Create(result.Data);
        try
        {
            _operatorRepository.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddLicenseToOperator command)
    {
        if (_licenseTypeRepository.IsIdUniqueAsync(command.LicenseType).Result)
        {
            return new InvalidResultNoData($"LicenseType with id of {command.LicenseType} is invalid.");
        }
        var @operator = _operatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if(@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        if(@operator.AddLicense(new(command.LicenseType), command.Arquired))
        {
            return new InvalidResultNoData($"A license with type of {command.LicenseType} is already present.");
        }
        _operatorRepository.Update(@operator);
        _operatorRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishLicenseTypeFromUser command)
    {
        if (!_licenseTypeRepository.IsTypeUniqueAsync(command.Type).Result)
        {
            return new InvalidResultNoData($"License type with of id {command.Type} already exist.");
        }
        var result = _licenseTypeFactory.CreateLicenseType(command); //consider moving all this stuff into a domain model (or multiple domain models)
        if(result is InvalidResult<LicenseType>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _licenseTypeRepository.Create(result.Data);
        _licenseTypeRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromSystem command)
    { //trigger OperatorRemoved which removes them for all vehicles and their licenses from their respective license types
        var entity = _operatorRepository.GetForOperationAsync(command.Id).Result;
        if(entity is not null)
        {
            //trigger event
            entity.Delete();
            /*
             * how to handle soft delete regarding vehicles and licensetypes
             * after all the operator id should be 'removed' as the operator has been softdeleted, but if the operator is restored should the ids in licensetypes and vehicles do that too?
             * Could remove the operator id from licenseType and vehicle and if operator is restored recreate the relations. Then again, should a restored operator still have the same access?
             */
            _operatorRepository.Update(entity);
            _operatorRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromUser command)
    { //trigger OperatorRemoved which removes them for all vehicles and their licenses from their respective license types
        throw new NotImplementedException();
    }

    public Result Handle(ObsoleteLicenseTypeFromUser command)
    { //transmit event, ObsoletedLicenseType, that trigger expiring driving licenses that people may have that use this specific type id.
        var entity = _licenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if(entity is not null)
        {
            entity.Delete(command.MomentOfDeletion);
            _licenseTypeRepository.Update(entity);
            _licenseTypeRepository.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AlterLicenseType command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(AddVehicleInformationFromSystem command)
    {
        if (!_vehicleInformationRepository.IsNameUniqueAsync(command.VehicleName).Result)
        {
            return new InvalidResultNoData("Vehicle information is not unique.");
        }
        var data = _licenseTypeRepository.AllAsync(new LicenseTypeForVehicleInformationValidationQuery()).Result;
        var valdationData = new VehicleInformationValidationData(data);
        var result = _vehicleInformationFactory.CreateVehicleInformation(command, valdationData);
        if(result is InvalidResult<VehicleInformation>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _vehicleInformationRepository.Create(result.Data);
        _vehicleInformationRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(BuyVehicleWithNoOperator command)
    { //trigger event VehicleAdded
        var vehicleInformations = _vehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var validationData = new VehicleValidationData(vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, validationData);
        if (result is InvalidResult<Vehicle>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _vehicleRepository.Create(result.Data);
        _vehicleRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(BuyVehicleWithOperators command)
    { //trigger event VehicleAdded. Need to information operator that they can use this vehicle.
        var operators = _operatorRepository.AllAsync(new OperatorIdQuery()).Result;
        var vehicleInformations = _vehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var valiationData = new VehicleValidationWithOperatorsData(operators, vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, valiationData);
        if(result is InvalidResult<Vehicle>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _vehicleRepository.Create(result.Data);
        _vehicleRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddDistanceToVehicleDistance command)
    {
        var entity = _vehicleRepository.GetForOperationAsync(command.Id).Result;
        if(entity is null)
        {
            return new InvalidResultNoData($"");
        }
        BinaryFlag flag = entity.AddToDistanceMoved(command.DistanceToAdd);
        if (flag != 0)
        {
            return new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _vehicleRepository.Update(entity);
        _vehicleRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(ResetVehicleMovedDistance command)
    {
        var entity = _vehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        BinaryFlag flag = entity.OverwriteDistanceMoved(command.NewDistance);
        if (flag != 0)
        {
            return new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _vehicleRepository.Update(entity);
        _vehicleRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishRelationBetweenOperatorAndVehicle command)
    { //raise some event that generates commands for the two handlers below
        if (_operatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        if (!_vehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public Result Handle(AddOperatorToVehicle command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (_operatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        var entity = _vehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if(entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.AddOperator(new(command.OperatorId));
        _vehicleRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(AddVehicleToOperator command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (!_vehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        var entity = _operatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if(entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.AddVehicle(new(command.VehicleId));
        _operatorRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveRelationBetweenOperatorAndVehicle command)
    {
        if (_operatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        if (!_vehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public Result Handle(RemoveOperatorFromVehicle command)
    { //should this save or should the handler for RemoveRelationBetweenOperatorAndVehicle?
        var entity = _vehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.RemoveOperator(new(command.OperatorId));
        _vehicleRepository.Update(entity);
        throw new NotImplementedException();
    }

    public Result Handle(RemoveVehicleFromOperator command)
    { //should this save or should the handler for RemoveRelationBetweenOperatorAndVehicle?
        var entity = _operatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.RemoveVehicle(new(command.VehicleId));
        _operatorRepository.Update(entity);
        throw new NotImplementedException();
    }

    public Result Handle(StartOperatingVehicle command)
    {
        var entity = _vehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if(entity is null)
        {
            return new InvalidResultNoData($"");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StartOperating(new(command.OperatorId));
        _vehicleRepository.Update(entity);
        _vehicleRepository.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(StopOperatingVehicle command)
    {
        var entity = _vehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StopOperating(new(command.OperatorId));
        _vehicleRepository.Update(entity);
        _vehicleRepository.Save();
        return new SuccessResultNoData();
    }
}
