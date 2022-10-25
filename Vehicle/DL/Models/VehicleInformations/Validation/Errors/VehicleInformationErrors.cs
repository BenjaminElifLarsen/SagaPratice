namespace VehicleDomain.DL.Models.VehicleInformations.Validation.Errors;
internal enum VehicleInformationErrors
{
    //LicenseTypeNotSat = 0b1,
    InvalidLicenseType = 0b10,
    InvalidName = 0b100,
    InvalidWheelAmount = 0b1000,
}
