using Common.Other;
using Common.ResultPattern;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.LicenseTypes.CQRS.Commands;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.Operators.CQRS.Commands;
using VehicleDomain.DL.Models.Operators.CQRS.Queries;
using VehicleDomain.DL.Models.Operators.Events;
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
using VehicleDomain.IPL.Services;

namespace VehicleDomain.AL.Handlers.Command;
internal class VehicleCommandHandler : IVehicleCommandHandler
{
    private readonly IVehicleFactory _vehicleFactory;
    private readonly IOperatorFactory _operatorFactory;
    private readonly ILicenseTypeFactory _licenseTypeFactory;
    private readonly IVehicleInformationFactory _vehicleInformationFactory;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleCommandHandler(IOperatorFactory operatorFactory,
        ILicenseTypeFactory licenseTypeFactory,
        IVehicleInformationFactory vehicleInformationFactory,
        IVehicleFactory vehicleFactory,
        IUnitOfWork unitOfWork)
    {
        _vehicleFactory = vehicleFactory;
        _operatorFactory = operatorFactory;
        _licenseTypeFactory = licenseTypeFactory;
        _vehicleInformationFactory = vehicleInformationFactory;
        _unitOfWork = unitOfWork;
    }

    public Result Handle(ValidateDriverLicenseStatus command)
    { //make a valdiation check, after this (if successfull) the user should use a query to get the result
        //this method could be called by a system designed to automatically ensure everyones licenses are up to date.
        var @operator = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        var license = @operator.GetLicense(command.TypeId);
        if (license is null)
        {
            return new InvalidResultNoData($"No license with type of {command.TypeId} was found.");
        }
        var oldValue = license.Expired;
        license.CheckIfExpired();
        if (oldValue != license.Expired)
        {
            _unitOfWork.OperatorRepository.Update(@operator);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddOperatorNoLicenseFromSystem command)
    {
        if (!_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Operator already exist.");
        }
        var result = _operatorFactory.CreateOperator(command);
        if (result is InvalidResult<Operator>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.OperatorRepository.Create(result.Data);
        try
        {
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddOperatorWithLicenseFromUser command)
    {//will need to trigger an event, PersonCreated
        if (!_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.Id).Result)
        {
            return new InvalidResultNoData("Operator already exist.");
        }
        var licenseTypeAges = _unitOfWork.LicenseTypeRepository.AllAsync(new LicenseTypeAgeQuery()).Result;
        var licenseTypeIds = _unitOfWork.LicenseTypeRepository.AllAsync(new LicenseTypeIdQuery()).Result;
        OperatorValidationData data = new(licenseTypeAges);
        var dictionary = new Dictionary<int, PersonCreationLicenseValidationData.LicenseValidationData>();
        foreach (var licenseType in licenseTypeAges)
        {
            dictionary.Add(licenseType.Id, new PersonCreationLicenseValidationData.LicenseValidationData(licenseType));
        }
        PersonCreationLicenseValidationData licenseData = new(dictionary, licenseTypeIds);
        var result = _operatorFactory.CreateOperator(command, data, licenseData);
        if (result is InvalidResult<Operator>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.OperatorRepository.Create(result.Data);
        try
        {
            _unitOfWork.Save();
        }
        catch (Exception e)
        {
            return new InvalidResultNoData(e.Message);
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AddLicenseToOperator command)
    {
        if (_unitOfWork.LicenseTypeRepository.IsIdUniqueAsync(command.LicenseType).Result)
        {
            return new InvalidResultNoData($"LicenseType with id of {command.LicenseType} is invalid.");
        }
        var @operator = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (@operator is null)
        {
            return new InvalidResultNoData("Operator was not found.");
        }
        if (@operator.AddLicense(new(command.LicenseType), command.Arquired))
        {
            return new InvalidResultNoData($"A license with type of {command.LicenseType} is already present.");
        }
        _unitOfWork.OperatorRepository.Update(@operator);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishLicenseTypeFromUser command)
    {
        if (!_unitOfWork.LicenseTypeRepository.IsTypeUniqueAsync(command.Type).Result)
        {
            return new InvalidResultNoData($"License type with of id {command.Type} already exist.");
        }
        var result = _licenseTypeFactory.CreateLicenseType(command); //consider moving all this stuff into a domain model (or multiple domain models)
        if (result is InvalidResult<LicenseType>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.LicenseTypeRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromSystem command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete();
            entity.AddDomainEvent(new OperatorRemoved(entity));
            _unitOfWork.OperatorRepository.Update(entity);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromUser command)
    { //trigger OperatorRemoved which removes them for all vehicles and their licenses from their respective license types
        throw new NotImplementedException();
    }

    public Result Handle(ObsoleteLicenseTypeFromUser command)
    { //transmit event, ObsoletedLicenseType, that trigger expiring driving licenses that people may have that use this specific type id.
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.Id).Result;
        if (entity is not null)
        {
            entity.Delete(command.MomentOfDeletion);
            _unitOfWork.LicenseTypeRepository.Update(entity);
            _unitOfWork.Save();
        }
        return new SuccessResultNoData();
    }

    public Result Handle(AlterLicenseType command)
    {
        throw new NotImplementedException();
    }

    public Result Handle(AddVehicleInformationFromSystem command)
    {
        if (!_unitOfWork.VehicleInformationRepository.IsNameUniqueAsync(command.VehicleName).Result)
        {
            return new InvalidResultNoData("Vehicle information is not unique.");
        }
        var data = _unitOfWork.LicenseTypeRepository.AllAsync(new LicenseTypeForVehicleInformationValidationQuery()).Result;
        var valdationData = new VehicleInformationValidationData(data);
        var result = _vehicleInformationFactory.CreateVehicleInformation(command, valdationData);
        if (result is InvalidResult<VehicleInformation>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleInformationRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(BuyVehicleWithNoOperator command)
    { //trigger event VehicleAdded
        var vehicleInformations = _unitOfWork.VehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var validationData = new VehicleValidationData(vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, validationData);
        if (result is InvalidResult<Vehicle>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(BuyVehicleWithOperators command)
    { //trigger event VehicleAdded. Need to information operator that they can use this vehicle.
        var operators = _unitOfWork.OperatorRepository.AllAsync(new OperatorIdQuery()).Result;
        var vehicleInformations = _unitOfWork.VehicleInformationRepository.AllAsync(new VehicleInformationIdQuery()).Result;
        var valiationData = new VehicleValidationWithOperatorsData(operators, vehicleInformations);
        var result = _vehicleFactory.CreateVehicle(command, valiationData);
        if (result is InvalidResult<Vehicle>)
        {
            return new InvalidResultNoData(result.Errors);
        }
        _unitOfWork.VehicleRepository.Create(result.Data);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(AddDistanceToVehicleDistance command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        BinaryFlag flag = entity.AddToDistanceMoved(command.DistanceToAdd);
        if (!flag)
        {
            return new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(ResetVehicleMovedDistance command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.Id).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        BinaryFlag flag = entity.OverwriteDistanceMoved(command.NewDistance);
        if (!flag)
        {
            return new InvalidResultNoData(VehicleErrorConversion.Convert(flag).ToArray());
        }
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(EstablishRelationBetweenOperatorAndVehicle command)
    { //raise some event that generates commands for the two handlers below
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public Result Handle(AddOperatorToVehicle command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.AddOperator(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(AddVehicleToOperator command)
    { //should this save or should the handler for EstablishRelationBetweenOperatorAndVehicle?
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.AddVehicle(new(command.VehicleId));
        _unitOfWork.OperatorRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveRelationBetweenOperatorAndVehicle command)
    {
        if (_unitOfWork.OperatorRepository.IsIdUniqueAsync(command.OperatorId).Result)
        {
            return new InvalidResultNoData($"");
        }
        if (!_unitOfWork.VehicleRepository.DoesVehicleExist(command.VehicleId).Result)
        {
            return new InvalidResultNoData($"");
        }
        throw new NotImplementedException();
    }

    public Result Handle(RemoveOperatorFromVehicle command)
    { 
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.RemoveOperator(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveVehicleFromOperator command)
    {
        var entity = _unitOfWork.OperatorRepository.GetForOperationAsync(command.OperatorId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        entity.RemoveVehicle(new(command.VehicleId));
        _unitOfWork.OperatorRepository.Update(entity);
        throw new NotImplementedException();
    }

    public Result Handle(StartOperatingVehicle command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StartOperating(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(StopOperatingVehicle command)
    {
        var entity = _unitOfWork.VehicleRepository.GetForOperationAsync(command.VehicleId).Result;
        if (entity is null)
        {
            return new InvalidResultNoData($"Not found.");
        }
        if (!entity.IsOperatorPermitted(new(command.OperatorId)))
        {
            return new InvalidResultNoData($"The operator with id {command.OperatorId} is not permitted to operate vehicle.");
        }
        entity.StopOperating(new(command.OperatorId));
        _unitOfWork.VehicleRepository.Update(entity);
        _unitOfWork.Save();
        return new SuccessResultNoData();
    }

    public Result Handle(RemoveOperatorFromLicenseType command)
    {
        var entity = _unitOfWork.LicenseTypeRepository.GetForOperationAsync(command.LicenseTypeId).Result;
        if (entity is null)
        {
            //error most likely as this should only be called by an evnet.
        }
        entity.RemoveOperator(new(command.OperatorId));
        _unitOfWork.LicenseTypeRepository.Update(entity);
        return new SuccessResultNoData();
    }
}
