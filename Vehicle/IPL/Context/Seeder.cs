using BaseRepository;
using VehicleDomain.DL.Models.LicenseTypes;
using VehicleDomain.DL.Models.Operators;
using VehicleDomain.DL.Models.VehicleInformations;
using VehicleDomain.DL.Models.Vehicles;

namespace VehicleDomain.IPL.Context;
internal static class Seeder
{
    public static void MockSeedData(IVehicleContext vehicleContext)
    {
        LicenseType train = new("Train", 4, 20);
        LicenseType bus = new("Bus", 2, 21);
        LicenseType car = new("Car", 10, 18);
        LicenseType moterBike = new("Moterbike", 1, 16);
        if (!(vehicleContext as IContextData<LicenseType>).GetAll.Any())
        {
            vehicleContext.Add(train);
            vehicleContext.Add(bus);
            vehicleContext.Add(car);
            vehicleContext.Add(moterBike);
        }

        VehicleInformation carInfo1 = new("Quick Yellow Car", 3, new(car.LicenseTypeId));
        VehicleInformation carInfo2 = new("Slow Car", 4, new(car.LicenseTypeId));
        VehicleInformation busInfo1 = new("CO2 Generating City Bus", 13, new(bus.LicenseTypeId));
        if (!(vehicleContext as IContextData<VehicleInformation>).GetAll.Any())
        {
            vehicleContext.Add(carInfo1);
            vehicleContext.Add(carInfo2);
            vehicleContext.Add(busInfo1);
            car.AddVehicleInformation(new(carInfo1.VehicleInformationId));
            car.AddVehicleInformation(new(carInfo2.VehicleInformationId));
            bus.AddVehicleInformation(new(busInfo1.VehicleInformationId));
        }

        Vehicle veh1 = new(new(200, 5, 21), new(carInfo1.VehicleInformationId), new("XM71"), 5);
        Vehicle veh2 = new(DateTime.Now, new(busInfo1.VehicleInformationId), new("???2G3"));
        if (!(vehicleContext as IContextData<Vehicle>).GetAll.Any())
        {
            vehicleContext.Add(veh1);
            vehicleContext.Add(veh2);
            carInfo1.RegistrateVehicle(new(veh1.VehicleId));
            busInfo1.RegistrateVehicle(new(veh2.VehicleId));
        }

        Operator op = new(1, new(1956, 1, 2));
        if (!(vehicleContext as IContextData<Operator>).GetAll.Any())
        {
            op.AddLicense(new(car.LicenseTypeId), new(2019, 5, 13));
            op.AddLicense(new(train.LicenseTypeId), new(2000, 3, 24));
            op.RenewLicense(train.LicenseTypeId, new(2006, 2, 1));
            op.AddVehicle(new(veh1.VehicleId));
            op.AddVehicle(new(veh2.VehicleId));
            vehicleContext.Add(op);
            veh1.AddOperator(new(op.OperatorId));
            veh2.AddOperator(new(op.OperatorId));
            car.AddOperator(new(1));
            train.AddOperator(new(1));
        }

        vehicleContext.Save();
    }
}
