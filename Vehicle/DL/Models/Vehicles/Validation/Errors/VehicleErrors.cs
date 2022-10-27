namespace VehicleDomain.DL.Models.Vehicles.Validation.Errors;
internal enum VehicleErrors
{
    InvalidAmountOfOperator = 0b1,
    InvalidOperator = 0b10,
    InvalidVehicleInformation = 0b100,
    InvalidDistance = 0b1000,
    InvalidProductionDate = 0b10000,
}
