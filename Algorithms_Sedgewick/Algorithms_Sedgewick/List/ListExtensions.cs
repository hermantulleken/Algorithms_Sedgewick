using System.Runtime.CompilerServices;
using static Support.WhiteBoxTesting;

namespace Algorithms_Sedgewick.List;

using System.Collections;
using System.Collections.Generic;
using Support;

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

	public static void Fill<T>(this IRandomAccessList<T> list, T value) => list.FillRange(0, list.Count, value);
	
	public static void FillRange<T>(this IRandomAccessList<T> list, int start, int count, T value)
	{
		for (int i = start; i < start + count; i++)
		{
			list[i] = value;
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
	public static IRandomAccessList<T> Copy<T>(this IEnumerable<T> list) => list.ToList().ToRandomAccessList();
	
	public static IRandomAccessList<T> ToRandomAccessList<T>(this IList<T> list) => new ListWrapper<T>(list);

	public static void AddN<T>(this ResizeableArray<T> list, T item, int timesToAdd)
	{
		for (int i = 0; i < timesToAdd; i++)
		{
			list.Add(item);
		}
	}
	
	public static IRandomAccessList<T> Take<T>(this IRandomAccessList<T> list, int count)
	{
		return new ListWindow<T>(list, 0, count);
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
	internal static bool LessOrEqual<T>(T v, T w) 
		where T : IComparable<T>
	{
		__AddCompareTo();
		
		return v.CompareTo(w) <= 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessOrEqualAt<T>(IReadonlyRandomAccessList<T> list, int i, int j) 
		where T : IComparable<T> 
		=> LessOrEqual(list[i], list[j]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void MoveAt<T>(IRandomAccessList<T?> list, int sourceIndex, int destinationIndex)
	{
		list[destinationIndex] = list[sourceIndex];
		list[sourceIndex] = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void MoveAt<T>(T?[] list, int sourceIndex, int destinationIndex)
	{
		list[destinationIndex] = list[sourceIndex];
		list[sourceIndex] = default;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void SwapAt<T>(IRandomAccessList<T> list, int i, int j)
	{
		__AddSwap();
		(list[i], list[j]) = (list[j], list[i]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void SwapAt<T>(T[] list, int i, int j)
	{
		__AddSwap();
		(list[i], list[j]) = (list[j], list[i]);
	}
}
