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
	
	/// <summary>
	/// Creates a new read-only random access list that contains the same elements as this list.
	/// </summary>
	/// <returns>A new read-only random access list that contains the same elements as this list.</returns>
	public static IRandomAccessList<T> Copy<T>(this IEnumerable<T> list) => list.ToList().ToRandomAccessList();
	
	public static IRandomAccessList<T> ToRandomAccessList<T>(this IList<T> list) => new ListWrapper<T>(list);

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
