using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Set;

namespace Algorithms_Sedgewick;



public static class Generator
{
	private const string CountMustBeSmallerThanMax = "Count must be less than or equal to maxValue.";
	
	private static readonly Random Random = new();

	// Note: The methods in this class are infinite. Use Take to get a finite amount of elements. 
	public static IEnumerable<int> UniformRandomInt(int maxValue)
	{
		while (true)
		{
			yield return NextUniformRandomInt(maxValue);
		}
		
		// ReSharper disable once IteratorNeverReturns
	}
	
	public static int NextUniformRandomInt(int maxValue) => Random.Next(maxValue);
	
	public static int NextUniformRandomInt(int minValue, int maxValue) => Random.Next(maxValue - minValue) + minValue;

	public static IRandomAccessList<int> UniqueUniformRandomInt_WithShuffledList(int maxValue, int count)
	{
		if (count > maxValue)
		{
			throw new ArgumentException(CountMustBeSmallerThanMax);
		}
		
		var list = ListExtensions.Range(0, maxValue);
		list.Shuffle(count); // We do not need more than count values, so we can do an incomplete shuffle. 
		
		return list.Take(count);
	}
	
	public static IRandomAccessList<int> UniqueUniformRandomInt_WithSet(int maxValue, int count)
	{
		if (count > maxValue)
		{
			throw new ArgumentException(CountMustBeSmallerThanMax);
		}
		
		var set = new Set.HashSet<int>(count, Comparer<int>.Default);
		
		while (set.Count < count)
		{
			set.Add(NextUniformRandomInt(maxValue));
		}

		return set
			.ToList()
			.ToRandomAccessList();
	}
}
