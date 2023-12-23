namespace AlgorithmsSW;

using List;
using Stack;

public static class CollectionExtensions
{
	public static TStack Copy<TStack, T>(this TStack stack) 
		where TStack : IStack<T>, new()
	{
		var copy = new TStack();

		foreach (var item in stack)
		{
			copy.Push(item);
		}

		return copy;
	}

	public static ResizeableArray<T> Copy<T>(this ResizeableArray<T> array)
	{
		var newArray = new ResizeableArray<T>(array.Capacity);
		
		foreach (var item in array)
		{
			newArray.Add(item);
		}

		return newArray;
	}
	
	public static ResizeableArray<T> ToResizableArray<T>(this IEnumerable<T> items, int capacity)
	{
		var array = new ResizeableArray<T>(capacity);
			
		foreach (var item in items)
		{
			array.Add(item);
		}

		return array;
	}
	
	public static Set.ISet<T> ToSet<T>(this IEnumerable<T> items, IComparer<T> comparer)
	{
		var set = DataStructures.Set(comparer);
			
		foreach (var item in items)
		{
			set.Add(item);
		}

		return set;
	}

	public static bool Contains<T>(this ResizeableArray<T> list, IEqualityComparer<T> comparer, T item) 
		=> list.Any(t => comparer.Equals(t, item));

	/// <summary>
	/// Groups the elements of a sequence into fixed-size chunks.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="size"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static IEnumerable<IEnumerable<T>> Group<T>(this IEnumerable<T> source, int size)
	{
		if (size <= 0)
		{
			throw new ArgumentException("Size must be greater than 0", nameof(size));
		}

		using var enumerator = source.GetEnumerator();
		
		while (enumerator.MoveNext())
		{
			yield return GetNextGroup(enumerator, size);
		}
	}

	private static IEnumerable<T> GetNextGroup<T>(IEnumerator<T> enumerator, int size)
	{
		do
		{
			yield return enumerator.Current;
		}
		while (--size > 0 && enumerator.MoveNext());
	}
	
	public static IEnumerable<(T, T)> GenerateAllPairs<T>(this IEnumerable<T> source)
	{
		var enumerable = source as T[] ?? source.ToArray();

		for (int i = 0; i < enumerable.Length; i++)
		{
			for (int j = i + 1; j < enumerable.Length; j++)
			{
				yield return (enumerable[i], enumerable[j]);
			}
		}
	}

	public static IEnumerable<(T, T)> GenerateDistinctPairs<T>(this IEnumerable<T> source)
	{
		var enumerable = source as T[] ?? source.ToArray();

		for (int i = 0; i < enumerable.Length; i++)
		{
			for (int j = i + 1; j < enumerable.Length; j++)
			{
				if (!EqualityComparer<T>.Default.Equals(enumerable[i], enumerable[j]))
				{
					yield return (enumerable[i], enumerable[j]);
				}
			}
		}
	}
	

}
