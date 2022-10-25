using Common.CQRS.Commands;

namespace VehicleDomain.DL.Models.People.CQRS.Commands;
public class AddPersonNoLicenseFromSystem : ICommand
{
    public int Id { get; private set; }
    public DateTime Birth { get; private set; }
}

public class AddPersonWithLicenseFromUser : ICommand
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