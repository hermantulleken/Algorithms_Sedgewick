namespace AlgorithmsSW;

using List;
using Stack;

/// <summary>
/// Provides extension methods for collections that do not belong to any specific collection type.
/// </summary>
/// <remarks> This includes extension methods for collections defined in this library, and collections in the standard
/// library. 
/// </remarks>
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

	public static ResizeableArray<T> ToResizableArray<T>(this ResizeableArray<T> array)
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
	
	// TODO: Add other collection types
	public static ResizeableArray<T> ToResizableArray<T>(this IEnumerable<T> items) =>
		items switch
		{
			IRandomAccessList<T> randomAccessList => randomAccessList.ToResizableArray(randomAccessList.Count),
			ICollection<T> collection => collection.ToResizableArray(collection.Count),
			_ => items.ToResizableArray(Collection.DefaultCapacity),
		};

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
	
	public static IEnumerable<(T item1, T item2)> GenerateAllPairs<T>(this IEnumerable<T> source)
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

	public static IEnumerable<(T item1, T item2)> GenerateDistinctPairs<T>(this IEnumerable<T> source)
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
	
	public static void CopyTo<T>(this IEnumerable<T> source, T[] array, int arrayIndex = 0)
	{
		source.ThrowIfNull();
		array.ThrowIfNull();
		arrayIndex.ThrowIfOutOfRange(0, array.Length);
		
		foreach (var item in source)
		{
			if (arrayIndex >= array.Length) 
			{
				throw new ArgumentException("The destination array was not long enough to copy all the items in the collection starting at the specified array index.");
			}
			
			array[arrayIndex++] = item;
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
}
