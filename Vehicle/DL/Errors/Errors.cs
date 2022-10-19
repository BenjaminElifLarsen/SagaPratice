namespace Vehicle.DL.Errors;
internal enum VehicleErrors
{
}

internal enum VehicleInformationErrors
{
}

internal enum LicenseTypeErrors
{
    IdAlreadyInUse = 0b1,
    InvalidType = 0b10,
    InvalidRenewPeriod = 0b100,
}

internal enum LicenseErrors
{
    InvalidArquired = 0b1,
    InvalidLastRenewed = 0b10,
}

internal enum PersonErrors
{
    IdNotSet = 0b1,
}