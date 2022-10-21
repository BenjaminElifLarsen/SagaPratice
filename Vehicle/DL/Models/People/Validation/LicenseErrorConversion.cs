namespace VehicleDomain.DL.Models.People.Validation;
internal static class LicenseErrorConversion
{
    public static IEnumerable<string> Convert(int binaryFlag)
    {
        List<string> errors = new();
        return errors;
    }
}




public interface ErrorConversion
{
    public abstract static IEnumerable<string> Convert(int binaryFlag);
}

public interface ErrorConversion<T> where T : class
{
    public abstract static IEnumerable<string> Convert(int binaryFlag, T entity);
}