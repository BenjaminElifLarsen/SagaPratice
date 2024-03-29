﻿using Common.UnitOfWork;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Services;
public interface IUnitOfWork : IBaseUnitOfWork, IBaseEventUnitOfWork
{
    public ILicenseTypeRepository LicenseTypeRepository { get; }
    public IOperatorRepository OperatorRepository { get; }
    public IVehicleInformationRepository VehicleInformationRepository { get; }
    public IVehicleRepository VehicleRepository { get; }
}
