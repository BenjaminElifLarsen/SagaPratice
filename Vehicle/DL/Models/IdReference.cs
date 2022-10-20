using Common.RepositoryPattern;

namespace VehicleDomain.DL.Models;
/// <summary>
/// Used to prevent accessing one aggregate root via another aggregate.
/// </summary>
internal record IdReference : ValueObject
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
