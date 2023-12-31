namespace AlgorithmsSW.List;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Buffer;
using Support;
using static System.Diagnostics.Debug;
using static WhiteBoxTesting;

public static class ListExtensions
{
	private sealed class ListWrapper<T> : IRandomAccessList<T>
	{
		private readonly IList<T> list;

		public int Count => list.Count;

		public bool IsEmpty => list.Count == 0;

		public T this[int index]
		{
			get => list[index];
			set => list[index] = value;
		}

		public ListWrapper(IList<T> list)
		{
			this.list = list;
		}

		public static implicit operator ListWrapper<T>(T[] list) => new(list);

		public static implicit operator ListWrapper<T>(List<T> list) => new(list);

		public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

		public override string ToString() => list.Pretty();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}

	private sealed class ListWindow<T> : IRandomAccessList<T>
	{
		private readonly int offset;
		private readonly IRandomAccessList<T> list;

		public ListWindow(IRandomAccessList<T> list, int offset, int count)
		{
			if (offset + count > list.Count)
			{
				throw new ArgumentException("The window is out of bounds.");
			}
			
			this.list = list;
			this.offset = offset;
			Count = count;
		}

		public IEnumerator<T> GetEnumerator() => list.Skip(offset).Take(Count).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int Count { get; private set; }

		public T this[int index]
		{
			get => list[index + offset];
			set => list[index + offset] = value;
		}
	}

	public static IRandomAccessList<T> AsList<T>(this IEnumerable<T> source)
	{
		return source switch
		{
			IRandomAccessList<T> randomAccessList => randomAccessList,
			IList<T> list => new ListWrapper<T>(list),
			_ => new ListWrapper<T>(source.ToArray())
		};
	}
	
	public static void Fill<T>(this IRandomAccessList<T> list, T value) => list.FillRange(0, list.Count, value);
	
	public static void FillRange<T>(this IRandomAccessList<T> list, int start, int count, T value)
	{
		for (int i = start; i < start + count; i++)
		{
			list[i] = value;
		}
	}
	
	// Note: Extension method because it is not effieicnt, and will clutter up the LisnkedList class.
	[ExerciseReference(1, 3, 19)]
	public static void RemoveLast<T>(this LinkedList<T> list)
	{
		if (list.First.NextNode == null)
		{
			list.Clear();
		}
		
		foreach (var (node, nextNode) in list.Nodes.SlidingWindow2())
		{
			Assert(node != null);
			Assert(nextNode != null);
			
			if (nextNode.NextNode == null)
			{
				// We have the last node
				list.RemoveAfter(node);
			}
		}
	}
	
	public static void Fill<T>(this IList<T> list, T value) => list.FillRange(0, list.Count, value);
	
	public static void FillRange<T>(this IList<T> list, int start, int count, T value)
	{
		for (int i = start; i < start + count; i++)
		{
			list[i] = value;
		}
	}
	
	public static void Fill<T>(this IRandomAccessList<T> list, Func<T> valueGenerator) => list.FillRange(0, list.Count, valueGenerator);
	
	public static void FillRange<T>(this IRandomAccessList<T> list, int start, int count, Func<T> valueGenerator)
	{
		for (int i = start; i < start + count; i++)
		{
			list[i] = valueGenerator();
		}
	}
	
	public static void Fill<T>(this IList<T> list, Func<T> valueGenerator) => list.FillRange(0, list.Count, valueGenerator);
	
	public static void Fill<T>(this IList<T> list, Func<int, T> valueGenerator) => list.FillRange(0, list.Count, valueGenerator);
	
	public static void FillRange<T>(this IList<T> list, int start, int count, Func<T> valueGenerator)
	{
		for (int i = start; i < start + count; i++)
		{
			list[i] = valueGenerator();
		}
	}
	
	public static void FillRange<T>(this IList<T> list, int start, int count, Func<int, T> valueGenerator)
	{
		for (int i = start; i < start + count; i++)
		{
			list[i] = valueGenerator(i);
		}
	}
	
	public static IRandomAccessList<int> Range(int start, int count)
	{
		var list = new ResizeableArray<int>(count);
		for (int i = 0; i < count; i++)
		{
			list.Add(i + start);
		}

		return list;
	}
	
	/// <summary>
	/// Creates a new read-only random access list that contains the same elements as this list.
	/// </summary>
	/// <returns>A new read-only random access list that contains the same elements as this list.</returns>
	public static IRandomAccessList<T> ToRandomAccessList<T>(this IEnumerable<T> list) => list.ToList().ToRandomAccessList();
	
	public static IRandomAccessList<T> ToRandomAccessList<T>(this IList<T> list) => new ListWrapper<T>(list);

	public static void AddN<T>(this ResizeableArray<T> list, T item, int timesToAdd)
	{
		for (int i = 0; i < timesToAdd; i++)
		{
			list.Add(item);
		}
	}
	
	/// <summary>
	/// Adds the elements of the specified collection to the end of the list.
	/// </summary>
	/// <param name="list">The list to add the elements to.</param>
	/// <param name="newITems">The collection whose elements should be added to the end of the list.</param>
	/// <typeparam name="T">The type of elements in the list.</typeparam>
	public static void AddRange<T>(this ResizeableArray<T> list, IEnumerable<T> newITems)
	{
		foreach (var item in newITems)
		{
			list.Add(item);
		}
	}
	
	public static IRandomAccessList<T> Take<T>(this IRandomAccessList<T> list, int count)
	{
		return new ListWindow<T>(list, 0, count);
	}
	
	/// <summary>
	/// Generates a sequence of sliding windows from the provided sequence.
	/// Each sliding window is of a specified size and contains a segment of the input sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in the input sequence.</typeparam>
	/// <param name="list">The input sequence from which sliding windows are generated.</param>
	/// <param name="windowSize">The size of each sliding window. Must be a positive integer.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> where each element is an <see cref="IEnumerable{T}"/> representing a sliding
	/// window of the specified size over the input sequence.
	/// </returns>
	/// <remarks>
	/// If the window size is greater than the number of elements in the input sequence, no windows are generated.
	/// This method uses deferred execution and streams the windows as they are generated.
	/// </remarks>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// var numbers = new List<int> { 0, 1, 2, 3, 4, 5 };
	/// foreach (var window in numbers.SlidingWindow(3))
	/// {
	///     Console.WriteLine(string.Join(", ", window));
	/// }
	/// // Output:
	/// // 0, 1, 2
	/// // 1, 2, 3
	/// // 2, 3, 4
	/// // 3, 4, 5
	/// ]]>
	/// </code>
	/// </example>
	public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> list, int windowSize)
	{
		var buffer = new RingBuffer<T>(windowSize);

		foreach (var item in list)
		{
			buffer.Insert(item);

			if (buffer.IsFull)
			{
				yield return buffer;
			}
		}
	}
	
	/// <summary>
	/// Generates a sequence of circular sliding windows from the provided sequence. Each sliding window is of a
	/// specified size and contains a segment of the input sequence. This method wraps around the sequence to include
	/// earlier elements in the window as it reaches the end of the
	/// sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements in the input sequence.</typeparam>
	/// <param name="list">The input sequence from which circular sliding windows are generated.</param>
	/// <param name="windowSize">The size of each sliding window. Must be a positive integer.</param>
	/// <returns>
	/// An <see cref="IEnumerable{T}"/> where each element is an <see cref="IEnumerable{T}"/> representing a circular
	/// sliding
	/// window of the specified size over the input sequence.
	/// </returns>
	/// <remarks>
	/// The circular sliding window wraps around the end of the sequence. If the window size is greater than the number
	/// of elements in the input sequence, it will continue to loop through the sequence until the window is filled.
	/// This method uses deferred execution and streams the windows as they are generated.
	/// </remarks>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// var numbers = new List<int> { 0, 1, 2, 3, 4, 5 };
	/// foreach (var window in numbers.CircularSlidingWindow(3))
	/// {
	///     Console.WriteLine(string.Join(", ", window));
	/// }
	/// // Output:
	/// // 0, 1, 2
	/// // 1, 2, 3
	/// // 2, 3, 4
	/// // 3, 4, 5
	/// // 4, 5, 0
	/// // 5, 0, 1
	/// ]]>
	/// </code>
	/// </example>
	public static IEnumerable<IEnumerable<T>> CircularSlidingWindow<T>(this IEnumerable<T> list, int windowSize)
	{
		var buffer = new RingBuffer<T>(windowSize);

		foreach (var item in list)
		{
			buffer.Insert(item);

			if (buffer.IsFull)
			{
				yield return buffer;
			}
		}

		foreach (var item in list.Take(windowSize - 1))
		{
			buffer.Insert(item);
			
			Assert(buffer.IsFull);
			
			yield return buffer;
		}
	}
	
	/// <summary>
	/// Provides a variant of <see cref="SlidingWindow{T}"/>, optimized for a window size of 2.
	/// </summary>
	/// <typeparam name="T">The type of elements in the input sequence.</typeparam>
	/// <param name="list">The input sequence from which sliding windows are generated.</param>
	public static IEnumerable<(T? first, T? last)> SlidingWindow2<T>(this IEnumerable<T?> list)
	{
		var buffer = new OptimizedCapacity2Buffer<T>();

		foreach (var item in list)
		{
			buffer.Insert(item);

			if (buffer.IsFull)
			{
				yield return (buffer.First, buffer.Last);
			}
		}
	}
	
	/// <summary>
	/// Provides a variant of <see cref="CircularSlidingWindow{T}"/>, optimized for a window size of 2.
	/// </summary>
	/// <typeparam name="T">The type of elements in the input sequence.</typeparam>
	/// <param name="list">The input sequence from which sliding windows are generated.</param>
	public static IEnumerable<IEnumerable<T?>> CircularSlidingWindow2<T>(this IEnumerable<T?> list)
	{
		var buffer = new OptimizedCapacity2Buffer<T>();

		foreach (var item in list)
		{
			buffer.Insert(item);

			if (buffer.IsFull)
			{
				yield return buffer;
			}
		}
		
		buffer.Insert(list.First());
		Assert(buffer.IsFull);
		yield return buffer;
	}
	
	// TODO Do we really need this null check here? 
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool Less<T>(T v, T w) 
		where T : IComparable<T>
	{
		__AddCompareTo();
		return v.CompareTo(w) < 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessAt<T>(T[] list, int i, int j) 
		where T : IComparable<T>
		=>
			Less(list[i], list[j]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessAt<T>(IReadonlyRandomAccessList<T> list, int i, int j) 
		where T : IComparable<T>
		=>
			Less(list[i], list[j]);
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessAt<T>(this T[] list, int i, int j, IComparer<T> comparer) 
		=> comparer.Less(list[i], list[j]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessAt<T>(IComparer<T> comparer, IReadonlyRandomAccessList<T> list, int i, int j) 
		=> comparer.Less(list[i], list[j]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessOrEqual<T>(T v, T w) 
		where T : IComparable<T>
	{
		__AddCompareTo();
		
		return v.CompareTo(w) <= 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessOrEqualAt<T>(this IReadonlyRandomAccessList<T> list, int i, int j) 
		where T : IComparable<T> 
		=> LessOrEqual(list[i], list[j]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void MoveAt<T>(this IRandomAccessList<T?> list, int sourceIndex, int destinationIndex)
	{
		list[destinationIndex] = list[sourceIndex];
		list[sourceIndex] = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void MoveAt<T>(this T?[] list, int sourceIndex, int destinationIndex)
	{
		list[destinationIndex] = list[sourceIndex];
		list[sourceIndex] = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void SwapAt<T>(this IRandomAccessList<T> list, int i, int j)
	{
		__AddSwap();
		(list[i], list[j]) = (list[j], list[i]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void SwapAt<T>(this T[] list, int i, int j)
	{
		__AddSwap();
		(list[i], list[j]) = (list[j], list[i]);
	}
}
