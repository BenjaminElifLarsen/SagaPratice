using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.Operators.CQRS.Commands;
public class AddOperatorNoLicenseFromSystem : ICommand
{
    public int Id { get; private set; }
    public DateTime Birth { get; private set; }
}

public class AddOperatorWithLicenseFromUser : ICommand
{
    public int Id { get; private set; }
    public DateTime Birth { get; private set; }
    public IEnumerable<License> Licenses { get; private set; }
}


public class License
{
    /// <summary>
    /// License type id
    /// </summary>
    public int LicenseTypeId { get; private set; }
    public DateTime Arquired { get; private set; }
}