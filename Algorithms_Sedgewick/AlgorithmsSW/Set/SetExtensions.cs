namespace AlgorithmsSW.Set;

/// <summary>
/// Provides extension methods for set operations like union, intersection,
/// difference, and symmetric difference.
/// </summary>
/// <remarks>
/// Many of these methods rely on iterating over the elements of the set, which
/// for many sets are not efficient. 
/// </remarks>
public static class SetExtensions
{
	/// <summary>
	/// Produces the union of two sets.
	/// </summary>
	/// <param name="set1">The first set.</param>
	/// <param name="set2">The second set.</param>
	/// <returns>A new set containing all elements that are in either <paramref name="set1"/> or
	/// <paramref name="set2"/>.</returns>
	public static ISet<T> Union<T>(this ISet<T> set1, ISet<T> set2)
	{
		var union = new HashSet<T>(set1.Comparer);
		
		foreach (var item in set1)
		{
			union.Add(item);
		}
		
		foreach (var item in set2)
		{
			union.Add(item);
		}

		return union;
	}
	
	/// <summary>
	/// Produces the intersection of two sets.
	/// </summary>
	/// <param name="set1">The first set.</param>
	/// <param name="set2">The second set.</param>
	/// <returns>A new set containing all elements that are in both <paramref name="set1"/> and
	/// <paramref name="set2"/>.</returns>
	public static ISet<T> Intersection<T>(ISet<T> set1, ISet<T> set2)
	{
		var intersection = new HashSet<T>(set1.Comparer);
		
		foreach (var item in set1)
		{
			if (set2.Contains(item))
			{
				intersection.Add(item);
			}
		}

		return intersection;
	}
	
	/// <summary>
	/// Produces the difference of two sets.
	/// </summary>
	/// <param name="set1">The first set (minuend).</param>
	/// <param name="set2">The second set (subtrahend).</param>
	/// <returns>A new set containing all elements that are in <paramref name="set1"/> but not in
	/// <paramref name="set2"/>.</returns>

	public static ISet<T> Difference<T>(this ISet<T> set1, ISet<T> set2)
	{
		var except = new HashSet<T>(set1.Comparer);
		
		foreach (var item in set1)
		{
			if (!set2.Contains(item))
			{
				except.Add(item);
			}
		}

		return except;
	}
	
	/// <summary>
	/// Produces the symmetric difference of two sets.
	/// </summary>
	/// <param name="set1">The first set.</param>
	/// <param name="set2">The second set.</param>
	/// <returns>A new set containing elements that are in either <paramref name="set1"/> or <paramref name="set2"/>,
	/// but not in both.</returns>
	public static ISet<T> SymmetricDifference<T>(this ISet<T> set1, ISet<T> set2)
	{
		var symmetricExcept = new HashSet<T>(set1.Comparer);
		
		foreach (var item in set1)
		{
			if (!set2.Contains(item))
			{
				symmetricExcept.Add(item);
			}
		}
		
		foreach (var item in set2)
		{
			if (!set1.Contains(item))
			{
				symmetricExcept.Add(item);
			}
		}

		return symmetricExcept;
	}
}
