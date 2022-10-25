using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class ValidateDriverLicenseStatus : ICommand
{
    public int OperatorId { get; private set; }
    public int TypeId { get; private set; }
}

public class ValidateOperatorLicenses : ICommand
{ //person id
    public int Id { get; private set; }
}
