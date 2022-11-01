﻿using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
internal static class Seeder
{
    public static void MockSeedData(MockVehicleContext vehicleContext)
    {
        LicenseType train = new("Train", 4, 20);
        LicenseType bus = new("Bus", 2, 21);
        LicenseType car = new("Car", 10, 18);
        LicenseType moterBike = new("Moterbike", 1, 16);
        if (!vehicleContext.LicenseTypes.Any())
        {
            vehicleContext.LicenseTypes.Add(train);
            vehicleContext.LicenseTypes.Add(bus);
            vehicleContext.LicenseTypes.Add(car);
            vehicleContext.LicenseTypes.Add(moterBike);
        }

        VehicleInformation carInfo1 = new("Quick Yellow Car", 3, new(car.LicenseTypeId));
        VehicleInformation carInfo2 = new("Slow Car", 4, new(car.LicenseTypeId));
        VehicleInformation busInfo1 = new("CO2 Generating City Bus", 13, new(bus.LicenseTypeId));
        if (!vehicleContext.VehicleInformations.Any())
        {
            vehicleContext.VehicleInformations.Add(carInfo1);
            vehicleContext.VehicleInformations.Add(carInfo2);
            vehicleContext.VehicleInformations.Add(busInfo1);
        }

        Vehicle veh1 = new(new(200, 5, 21), new(carInfo1.VehicleInformationId), 5);
        Vehicle veh2 = new(DateTime.Now, new(busInfo1.VehicleInformationId));
        if (!vehicleContext.Vehicles.Any())
        {
            vehicleContext.Vehicles.Add(veh1);
            vehicleContext.Vehicles.Add(veh2);
        }

        Operator owner = new(1, new(1956, 1, 2));
        if (!vehicleContext.People.Any())
        {
            owner.AddLicense(new(car.LicenseTypeId), new(2019, 5, 13));
            owner.AddVehicle(new(veh1.VehicleId));
            owner.AddVehicle(new(veh2.VehicleId));
            vehicleContext.People.Add(owner);
        }

    }
}
