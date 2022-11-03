using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models.Vehicles;
internal record SerielNumber : ValueObject
{
    private string _serialNumber;
    public string SerialNumber { get => _serialNumber; private set => _serialNumber = value; }

    public SerielNumber(string  serialNumber)
    {
        _serialNumber = serialNumber;
    }
}
