namespace VehicleDomain.DL.Models;

internal static class RandomValue // Just here for the mock id generation.
{
	private static readonly Random _random = new(int.MaxValue);

	public static int GetValue => _random.Next();
}