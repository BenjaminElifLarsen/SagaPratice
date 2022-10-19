using Common.RepositoryPattern;

namespace Vehicle.DL.Models;
/// <summary>
/// Used to prevent accessing one aggregate root via another aggregate root.
/// </summary>
internal record IdReference : ValueObject
{
    public int Id { get; private set; }

	public IdReference(int id)
	{
		Id = id;
	}
}
