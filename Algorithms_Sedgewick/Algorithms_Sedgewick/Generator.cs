namespace Algorithms_Sedgewick;

public static class Generator
{
	private static readonly Random Random = new();

	// Note: The methods in this class are infinite. Use Take to get a finite amount of elements. 
	public static IEnumerable<int> UniformRandomInt(int maxValue)
	{
		while (true)
		{
			yield return Random.Next(maxValue);
		}
		
		// ReSharper disable once IteratorNeverReturns
	}
}
