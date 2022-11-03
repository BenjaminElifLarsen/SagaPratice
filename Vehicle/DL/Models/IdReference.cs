using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models;
/// <summary>
/// Used to prevent accessing one aggregate root via another aggregate.
/// </summary>
public record IdReference : ValueObject
{
	/*
	 * In ORMs like EF Core the domain models that usages this record,
	 * it would be possible to set up to create the database relationships.
	 */

    public int Id { get; private set; }

	public IdReference(int id)
	{
		Id = id;
	}
}


internal static class RandomValue // Just here for the mock id generation.
{
	private static readonly Random _random = new(int.MaxValue);

	public static int GetValue => _random.Next();
}