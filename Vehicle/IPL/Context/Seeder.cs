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
        if (!vehicleContext.Set<LicenseType>().Any())
        {
            vehicleContext.Add(train);
            vehicleContext.Add(bus);
            vehicleContext.Add(car);
            vehicleContext.Add(moterBike);
        }

        VehicleInformation carInfo1 = new("Quick Yellow Car", 3, car.Id);
        VehicleInformation carInfo2 = new("Slow Car", 4, car.Id);
        VehicleInformation busInfo1 = new("CO2 Generating City Bus", 13, bus.Id);
        if (!vehicleContext.Set<VehicleInformation>().Any())
        {
            vehicleContext.Add(carInfo1);
            vehicleContext.Add(carInfo2);
            vehicleContext.Add(busInfo1);
            car.AddVehicleInformation(carInfo1.Id);
            car.AddVehicleInformation(carInfo2.Id);
            bus.AddVehicleInformation(busInfo1.Id);
        }

        Vehicle veh1 = new(new(200, 5, 21), carInfo1.Id, new("XM71"), 5);
        Vehicle veh2 = new(DateTime.Now, busInfo1.Id, new("???2G3"));
        if (!vehicleContext.Set<Vehicle>().Any())
        {
            vehicleContext.Add(veh1);
            vehicleContext.Add(veh2);
            carInfo1.RegistrateVehicle(veh1.Id);
            busInfo1.RegistrateVehicle(veh2.Id);
        }

        Operator op = new(Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), new(1956, 1, 2));
        if (!vehicleContext.Set<Operator>().Any())
        {
            op.AddLicense(car.Id, new(2018, 5, 13));
            op.AddLicense(train.Id, new(2000, 3, 24));
            op.RenewLicense(train.Id, new(2006, 2, 1));
            op.AddVehicle(veh1.Id);
            op.AddVehicle(veh2.Id);
            vehicleContext.Add(op);
            veh1.AddOperator(op.Id);
            veh2.AddOperator(op.Id);
            car.AddOperator(op.Id);
            train.AddOperator(op.Id);
        }

        vehicleContext.Save();
    }
}
