using l = VehicleDomain.DL.CQRS.Commands.License;

namespace VehicleDomain.DL.Models.People.Validation;

internal class LicenseErrorConversion : IErrorConversion<l>
{
    public static IEnumerable<string> Convert(int binaryFlag, l license)
    {
        List<string> errors = new();
        return errors;
    }
}




public interface IErrorConversion //when proper tested and they work, move them over to the Common project. 
{
    public abstract static IEnumerable<string> Convert(int binaryFlag);
}

public interface IErrorConversion<T> where T : class
{
    public abstract static IEnumerable<string> Convert(int binaryFlag, T entity);
}