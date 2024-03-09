namespace AlgorithmsSW;

using List;

/// <summary>
/// Provides methods for generating random values.
/// </summary>
public static class Generator
{
	private const string CountMustBeSmallerThanMax = "Count must be less than or equal to maxValue.";
	
	private static readonly Random Random = new();

	// Note: The methods in this class are infinite. Use Take to get a finite amount of elements. 
#region GeneratorExample
	/// <summary>
	/// Returns an infinite sequence of random integers in the range [0, maxValue).
	/// </summary>
	/// <param name="maxValue">The exclusive upper bound of the random integers.</param>
	/// <remarks> Use <see cref="Enumerable.Take{TSource}(System.Collections.Generic.IEnumerable{TSource},int)"/> to
	/// get a finite amount of elements. </remarks>
	public static IEnumerable<int> UniformRandomInt(int maxValue)
	{
		while (true)
		{
			yield return NextUniformRandomInt(maxValue);
		}
		
		// ReSharper disable once IteratorNeverReturns
	}
#endregion

	public static IEnumerable<double> UniformRandomDouble(double maxValue) => UniformRandomDouble(0, maxValue);
	
	public static IEnumerable<double> UniformRandomDouble(double minValue, double maxValue)
	{
		while (true)
		{
			yield return NextUniformRandomDouble(minValue, maxValue);
		}
		
		// ReSharper disable once IteratorNeverReturns
	}

	/// <summary>
	/// Generates a random integer in the range [0, maxValue).
	/// </summary>
	/// <param name="maxValue">The exclusive upper bound of the random integer.</param>
	public static int NextUniformRandomInt(int maxValue) => Random.Next(maxValue);
	
	/// <summary>
	/// Generates a random integer in the range [minValue, maxValue).
	/// </summary>
	/// <param name="minValue">The inclusive lower bound of the random integer.</param>
	/// <param name="maxValue">The exclusive upper bound of the random integer.</param>
	public static int NextUniformRandomInt(int minValue, int maxValue) => Random.Next(maxValue - minValue) + minValue;

	/// <summary>
	/// Generates a set of random integers in the range [0, maxValue) where each integer is unique.
	/// </summary>
	/// <param name="maxValue">The exclusive upper bound of the random integer.</param>
	/// <param name="count">The number of random integers to generate.</param>
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
	
	/// <summary>
	/// Generates a set of random integers in the range [0, maxValue) where each integer is unique.
	/// </summary>
	/// <param name="maxValue">The exclusive upper bound of the random integer.</param>
	/// <param name="count">The number of random integers to generate.</param>
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
	
	private static double NextUniformRandomDouble(double minValue, double maxValue) 
		=> Random.NextDouble() * (maxValue - minValue) + minValue;
}
