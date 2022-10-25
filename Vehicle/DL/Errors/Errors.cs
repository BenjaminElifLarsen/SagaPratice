namespace VehicleDomain.DL.Errors;

internal enum LicenseTypeErrors
{
    InvalidAgeRequirement = 0b1,
    InvalidType = 0b10,
    InvalidRenewPeriod = 0b100,
}

internal enum LicenseErrors
{
    InvalidArquired = 0b1,
    InvalidLastRenewed = 0b10,
    LicenseTypeNotSat = 0b100,
    InvalidLicenseType = 0b1000,
}

internal enum OperatorErrors
{
    IdNotSet = 0b1,
    InvalidAgeForLicense = 0b10,
    InvalidBirth = 0b100,
    NotWithinAgeRange = 0b1000,
}