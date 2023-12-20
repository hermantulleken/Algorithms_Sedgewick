namespace AlgorithmsSW;

using System.Diagnostics.CodeAnalysis;
using Buffer;
using List;
using Queue;
using Support;

using static System.Diagnostics.Debug;
using static AlgorithmsSW.List.ListExtensions;
using static Sort.Sort;
using static Support.WhiteBoxTesting;

/// <summary>
/// Provides algorithms for various tasks, especially for operating on lists. 
/// </summary>
public static class Algorithms
{
	private static readonly Random Random = new();
	
	/// <summary>
	/// Returns the supplied parameter. 
	/// </summary>
	/// <param name="value">The value to return.</param>
	/// <typeparam name="T">The type of the value to return.</typeparam>
	/// <remarks>This class is useful for cases where a function is required, but no transformation is needed.</remarks>
	public static T Identity<T>(T value) => value;

	/// <summary>
	/// Gives the indexes of elements in a list that satisfy the given predicate.
	/// </summary>
	/// <param name="list">The enumerable list to search.</param>
	/// <param name="predicate">A function to test each element for a condition.</param>
	/// <typeparam name="T">The type of the values in the list.</typeparam>
	public static IEnumerable<int> IndexWhere<T>(this IEnumerable<T> list, Func<T, bool> predicate)
	{
		int index = 0;

		foreach (var item in list)
		{
			if (predicate(item))
			{
				yield return index;
			}

			index++;
		}
	}

	/// <summary>
	/// Returns the number of elements in a sorted list that are less than the given value. 
	/// </summary>
	public static int BinaryRank<T>(this IReadonlyRandomAccessList<T> list, T item, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		comparer.ThrowIfNull();
		
		Assert(IsSorted(list, comparer));
		
		int left = 0;
		int right = list.Count - 1;

		while (left <= right)
		{
			int mid = left + (right - left) / 2;

			if (comparer.Less(list[mid], item))
			{
				left = mid + 1;
			}
			else
			{
				right = mid - 1;
			}
		}

		return left;
	}

	[Obsolete, PossiblyIncorrect]
	public static int BinaryRank__<T>(this IReadonlyRandomAccessList<T> list, T key, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		
		int start = 0;
		int end = list.Count - 1;
		
		while (start <= end)
		{
			int mid = (start + end) / 2;

			switch (comparer.Compare(key, list[mid]))
			{
				case < 0:
					end = mid - 1;
					break;
				case > 0:
					start = mid + 1;
					break;
				default:
					return mid;
			}
		}
		
		return start;
	}

	[Obsolete, PossiblyIncorrect]
	public static int BinarySearch(this int[] list, int key)
	{
		list.ThrowIfNull();

		if (list.Length == 0)
		{
			return -1;
		}

		int start = 0;
		int end = list.Length - 1;

		while (start <= end)
		{
			int mid = (start + end) / 2;

			if (key == list[mid])
			{
				return mid;
			}

			if (key < list[mid])
			{
				end = mid - 1;
			}
			else
			{
				start = mid + 1;
			}
		}

		return -1;	
	}

	[Obsolete, PossiblyIncorrect]
	public static int BinarySearch<T>(this IReadonlyRandomAccessList<T> list, T key, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		
		int start = 0;
		int end = list.Count - 1;
		
		while (start <= end)
		{
			int mid = (start + end) / 2;

			switch (comparer.Compare(key, list[mid]))
			{
				case < 0:
					end = mid - 1;
					break;
				case > 0:
					start = mid + 1;
					break;
				default:
					return mid;
			}
		}
		
		return -1;
	}

	/// <summary>
	/// Returns pairs of successive elements.
	/// </summary>
	/// <remarks>
	/// Will not return any elements if the list has 1 or 0 elements. The buffer will always contain two elements. 
	/// </remarks>
	public static IEnumerable<IPair<T>> Buffer2<T>(this IEnumerable<T> list)
	{
		// Would be nice if we could eliminate this new operation
		var buffer = new Capacity2Buffer<T>();
		
		foreach (var item in list)
		{
			buffer.Insert(item);
			
			if (buffer.Count == 2)
			{
				yield return buffer;
			}
		}
	}
	
	public static ResizeableArray<ResizeableArray<T>> PowerSet<T>(IEnumerable<T> set)
	{
		ResizeableArray<ResizeableArray<T>> allSubsets = [[]];

		foreach (var element in set)
		{
			var newSubsets = new ResizeableArray<ResizeableArray<T>>();

			foreach (var subset in allSubsets)
			{
				var newSubset = new ResizeableArray<T>();
				newSubset.AddRange(subset);
				newSubset.Add(element);
				newSubsets.Add(newSubset);
			}

			allSubsets.AddRange(newSubsets);
		}

		return allSubsets;
	}
	
	public static float Median(float[] list) => Median(list, 0, list.Length - 1);
		
	private static float Median(float[] list, int start, int end)
	{
		if (start >= end)
		{
			return list[start];
		}
			
		while (true)
		{
			int centerIndex = (start + end) / 2;

			list.SwapAt(start, centerIndex);
			int pivotIndex = start;

			for (int i = start + 1; i <= end; i++)
			{
				if (!(list[i] < list[pivotIndex]))
                {
                    continue;
                }
				
				// Move the item to the right of the pivot
				list.SwapAt(i, pivotIndex + 1);

				// Swap item with pivot
				list.SwapAt(pivotIndex + 1, pivotIndex);

				// Now the pivot is here
				pivotIndex++;
			}

			if (pivotIndex < centerIndex)
			{
				start = pivotIndex + 1;
			}
			else if (pivotIndex > centerIndex)
			{
				end = pivotIndex - 1;
			}
			else
			{
				return list[pivotIndex];
			}
		}
	}

	public static int FindIndexOfMax<T>(this IReadonlyRandomAccessList<T> list) 
		where T : IComparable<T>
		=> list.FindIndexOfMax(0, list.Count);

	public static int FindIndexOfMax<T>(this IReadonlyRandomAccessList<T> list, int start, int end) 
		where T : IComparable<T>
	{
		int length = end - start;
		switch (length)
		{
			case 0:
				ThrowHelper.ThrowContainerEmpty();
				break;
			case 1:
				return 0;
		}

		var max = list[start];
		int maxIndex = start;
		
		for (int i = start + 1; i < end; i++)
		{
			if (Less(max, list[i]))
			{
				maxIndex = i;
				max = list[i];
			}
		}

		return maxIndex;
	}

	public static int FindIndexOfMax<T>(this T[] list) 
		where T : IComparable<T>
		=> list.FindIndexOfMax(0, list.Length);

	public static int FindIndexOfMax<T>(this T[] list, int start, int end)
		where T : IComparable<T>
	{
		int length = end - start;
		switch (length)
		{
			case 0:
				ThrowHelper.ThrowContainerEmpty();
				break;
			case 1:
				return 0;
		}

		var max = list[start];
		int maxIndex = start;
		
		for (int i = start + 1; i < end; i++)
		{
			if (Less(max, list[i]))
			{
				maxIndex = i;
				max = list[i];
			}
		}

		return maxIndex;
	}
	
	public static int FindIndexOfMax<T>(this T[] list, int start, int end, IComparer<T> comparer)
	{
		int length = end - start;
		switch (length)
		{
			case 0:
				ThrowHelper.ThrowContainerEmpty();
				break;
			case 1:
				return 0;
		}

		var max = list[start];
		int maxIndex = start;
		
		for (int i = start + 1; i < end; i++)
		{
			if (comparer.Less(max, list[i]))
			{
				maxIndex = i;
				max = list[i];
			}
		}

		return maxIndex;
	}

	public static int FindIndexOfMin<T>(this IReadonlyRandomAccessList<T> list) 
		where T : IComparable<T>
		=> list.FindIndexOfMin(0, list.Count);

	public static int FindIndexOfMin<T>(this IReadonlyRandomAccessList<T> list, int start, int end) 
		where T : IComparable<T>
	{
		int length = end - start;
		
		switch (length)
		{
			case 0:
				ThrowHelper.ThrowContainerEmpty();
				break;
			case 1:
				return 0;
		}

		var min = list[start];
		int minIndex = start;
		
		for (int i = start + 1; i < end; i++)
		{
			if (Less(list[i], min))
			{
				minIndex = i;
				min = list[i];
			}
		}

		return minIndex;
	}
	
	public static int FindIndexOfMin<T>(this IReadonlyRandomAccessList<T> list, IComparer<T> comparer) 
		=> list.FindIndexOfMin(0, list.Count, comparer);

	public static int FindIndexOfMin<T>(this IReadonlyRandomAccessList<T> list, int start, int end, IComparer<T> comparer) 
	{
		int length = end - start;
		
		switch (length)
		{
			case 0:
				ThrowHelper.ThrowContainerEmpty();
				break;
			case 1:
				return 0;
		}

		var min = list[start];
		int minIndex = start;
		
		for (int i = start + 1; i < end; i++)
		{
			if (comparer.Less(list[i], min))
			{
				minIndex = i;
				min = list[i];
			}
		}

		return minIndex;
	}

	public static int FindIndexOfMin<T>(this T[] list) 
		where T : IComparable<T>
		=> list.FindIndexOfMin(0, list.Length);

	public static int FindIndexOfMin<T>(this T[] list, int start, int end) 
		where T : IComparable<T>
	{
		int length = end - start;
		
		switch (length)
		{
			case 0:
				ThrowHelper.ThrowContainerEmpty();
				break;
			case 1:
				return 0;
		}

		var min = list[start];
		int minIndex = start;
		
		for (int i = start + 1; i < end; i++)
		{
			if (Less(list[i], min))
			{
				minIndex = i;
				min = list[i];
			}
		}

		return minIndex;
	}

	/// <summary>
	/// Finds the index in a sorted list at which an item can be inserted so that all the
	/// elements to the left are smaller or equal, and all the elements to the right are larger
	/// than the item.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the list. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="sortedList">The sorted list to search for the item.</param>
	/// <param name="item">The item to find the index for.</param>
	/// <param name="comparer">The <see cref="IComparer{T}"/> to use for comparisons.</param>
	/// <returns>The index at which the item can be inserted while maintaining the sorted order.</returns>
	/// <remarks>
	/// If the item is smaller than all the items in the list, the method returns <c>0</c>.
	/// If the item is greater than all the items in the list, the method returns <c>sortedList.Count</c>.
	/// </remarks>
	/*
		We return the last (and not first or any other) index that preserves the order of the list if the item is inserted
		so that the number of elements we need to move when inserting the item is minimal. 
		
		Uses binary search. 
		
		TODO: May not work correctly if equal keys are present.
	*/
	[Obsolete, PossiblyIncorrect]
	public static int FindInsertionIndex<T>(
		this IReadonlyRandomAccessList<T> sortedList, T item, IComparer<T> comparer)
	{
		sortedList.ThrowIfNull();
		comparer.ThrowIfNull();
		
		Assert(IsSorted(sortedList, comparer));
		
		// Define a binary search algorithm to find the insertion point for the item.
		int Find(int start, int end)
		{
			while (end > start + 1)
			{
				int mid = (start + end) / 2;
		
				Assert(start != mid); // because end > start + 1
				Assert(mid != end);

				if (comparer.Less(item, sortedList[mid]))
				{
					end = mid;
				}
				else 
				{
					start = mid;
				}
			}
			
			// When there are only two elements left, return the index of the correct element.
			Assert(end == start + 1);
			
			return comparer.Less(item, sortedList[start]) ? start : end;
		}

		// Handle special cases where the list is empty or the item is outside the bounds of the list.
		return sortedList.IsEmpty 
			? 0 
			: comparer.Less(item, sortedList[0]) 
				? 0 
				: comparer.Less(sortedList[^1], item) 
					? sortedList.Count 
					: Find(0, sortedList.Count);
	}
	
	public static LinkedList<KeyValuePair<TKey, TValue>>.Node FindInsertionNodeUnsafe<TKey, TValue>(
		this LinkedList<KeyValuePair<TKey, TValue>> sortedList, TKey key, IComparer<TKey> comparer)
	{
		Assert(!sortedList.IsEmpty);
		
		// Note: if item < firstItem we need to add it at the front
		// item >= firstItem
		Assert(comparer.LessOrEqual(sortedList.First.Item.Key, key));

		var node = sortedList.First;
		
		while (true)
		{
			var next = node.NextNode;

			if (next == null)
			{
				return node;
			}

			if (comparer.Less(key, next.Item.Key))
			{
				return node;
			}

			node = next;
		}

		Assert(false); // Unreachable
		return sortedList.Last;
	}

	public static LinkedList<T>.Node FindInsertionNodeUnsafe<T>(this LinkedList<T> sortedList, T item, IComparer<T> comparer)
	{
		Assert(!sortedList.IsEmpty);
		
		// Note: if item < firstItem we need to add it at the front
		// item >= firstItem
		Assert(comparer.LessOrEqual(sortedList.First.Item, item));

		var node = sortedList.First;
		
		while (true)
		{
			var next = node.NextNode;

			if (next == null)
			{
				return node;
			}

			if (comparer.Less(item, next.Item))
			{
				return node;
			}

			node = next;
		}

		Assert(false); // Unreachable
		return sortedList.Last;
	}
	
	public static LinkedList<T>.Node FindInsertionNode<T>(this LinkedList<T> sortedList, T item, IComparer<T> comparer)
	{
		sortedList.ThrowIfNull();
		comparer.ThrowIfNull();

		return FindInsertionNodeUnsafe(sortedList, item, comparer);
	}

	public static T First<T>(this IReadonlyRandomAccessList<T> source)
	{
		source
			.ThrowIfNull()
			.ThrowIfEmpty();

		return source[0];
	}

	public static void InsertSorted<T>(this ResizeableArray<T> list, T item) 
		where T : IComparable<T>
	{
		InsertSorted(list, item, Comparer<T>.Default);
	}

	// TODO: Test the output
	public static int InsertSorted<T>(this ResizeableArray<T> list, T item, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		item.ThrowIfNull();
		comparer.ThrowIfNull();
		
		ArgumentNullException.ThrowIfNull(list);
		ArgumentNullException.ThrowIfNull(item);
		ArgumentNullException.ThrowIfNull(comparer);
		
		Assert(IsSorted(list, comparer));
		
		int insertionIndex = list.FindInsertionIndex(item, comparer);
		list.InsertAt(item, insertionIndex);
		return insertionIndex;
	}

	public static void InsertSorted<T>(this LinkedList<T> list, T item) 
		where T : IComparable<T>
	{
		list.ThrowIfNull();
		if (list.Count == 0)
		{
			list.InsertAtFront(item);
			return;
		}
		
		var node = list.First;

		if (Less(item, node.Item))
		{
			list.InsertAtFront(item);
			return;
		}

		while (node.NextNode != null)
		{
			if (Less(item, node.NextNode.Item))
			{
				list.InsertAfter(node, item);
				return;
			}

			node = node.NextNode;
		}
		
		Assert(node == list.Last);
		list.InsertAtBack(item);
	}
	
	public static void InsertSorted<T>(this LinkedList<T> list, T item, IComparer<T> comparer) 
	{
		list.ThrowIfNull();
		if (list.Count == 0)
		{
			list.InsertAtFront(item);
			return;
		}
		
		var node = list.First;

		if (comparer.Less(item, node.Item))
		{
			list.InsertAtFront(item);
			return;
		}

		while (node.NextNode != null)
		{
			if (comparer.Less(item, node.NextNode.Item))
			{
				list.InsertAfter(node, item);
				return;
			}

			node = node.NextNode;
		}
		
		Assert(node == list.Last);
		list.InsertAtBack(item);
	}

	[Obsolete, PossiblyIncorrect]
	public static int InterpolationSearch(this int[] list, int key)
	{
		list.ThrowIfNull();
		
		Assert(IsSorted(list.ToRandomAccessList()), $"{nameof(list)} is not sorted.");
		
		if (list.Length == 0 || key < list[0] || list[^1] <= key)
		{
			return -1;
		}
		
		int startIndex = 0;
		int endIndex = list.Length - 1;

		while (true)
		{
			__AddPass();
			
			if (endIndex < startIndex || key < list[startIndex] || key > list[endIndex])
			{
				return -1;
			}
			
			int length = endIndex - startIndex + 1;
			float first = list[startIndex];

			if (length == 1)
			{
				return key == list[startIndex] ? startIndex : -1;
			}

			float last = list[endIndex];
			float fraction = (key - first) / (last - first);
			float approximateMid = ((endIndex - startIndex) * fraction) + startIndex;
			int mindIndex = (int)MathF.Floor(approximateMid);
			
			if (key == list[mindIndex])
			{
				return mindIndex;
			}

			int newStart = startIndex;
			int newEnd = endIndex;
			
			if (key < list[mindIndex])
			{
				newEnd = mindIndex - 1;
			}
			else
			{
				newStart = mindIndex + 1;
			}
			
			// Assert(list[newStart] <= key);
			// Assert(key <= list[newEnd]);

			startIndex = newStart;
			endIndex = newEnd;
		}
	}

	public static IEnumerable<T> JosephusSequence<T>(this IEnumerable<T> list, int m)
	{
		var queue = new QueueWithLinkedList<T>();

		int i = 0;
		foreach (var item in list)
		{
			if (i % m == 0)
			{
				yield return item;
			}
			else
			{
				queue.Enqueue(item);
			}

			i++;
		}

		while (queue.Count > 1)
		{
			var item = queue.Dequeue();

			if (i % m == 0)
			{
				yield return item;
			}
			else
			{
				queue.Enqueue(item);
			}

			i++;
		}

		if (queue.Count == 1)
		{
			yield return queue.Dequeue();
		}
	}

	public static T Last<T>(this IReadonlyRandomAccessList<T> source)
	{
		source
			.ThrowIfNull()
			.ThrowIfEmpty();
		
		return source[^1];
	}

	public static IEnumerable<LinkedList<T>.Node> NodesBeforeThat<T>(this LinkedList<T> list, Func<LinkedList<T>.Node, bool> predicate)
	{
		if (list.IsEmpty || list.Count == 1)
		{
			return Array.Empty<LinkedList<T>.Node>();
		}
		
		return list.Nodes
			.SkipLast(1)
			.Where(node => predicate(node.NextNode!));
	}

	/// <summary>
	/// Returns the nth node in a list.
	/// </summary>
	/// <param name="list">The list to return the element from.</param>
	/// <param name="n">The index of the element to return. 0 Will return the first element.</param>
	/// <typeparam name="T">The type of items in the list.</typeparam>
	public static LinkedList<T>.Node Nth<T>(this LinkedList<T> list, int n) => list.Nodes.Skip(n).First();

	/// <summary>
	/// Remove elements from a list that satisfy a predicate.
	/// </summary>
	/// <param name="list">The list to process.</param>
	/// <param name="predicate">A predicate items to be removed should satisfy.</param>
	/// <typeparam name="T">The type of items in the list.</typeparam>
	public static void RemoveIf<T>(this LinkedList<T> list, Func<LinkedList<T>.Node, bool> predicate)
	{
		if (list.IsEmpty)
		{
			return;
		}

		var current = list.First;

		while (predicate(current))
		{
			list.RemoveFromFront();
			current = list.First;
		}

		if (list.IsEmpty)
		{
			return;
		}

		Assert(current != null);
		
		while (current.NextNode != null)
		{
			if (predicate(current.NextNode))
			{
				list.RemoveAfter(current);
			}
			
			current = current.NextNode;
		}
	}

	public static LinkedList<T>.Node RemoveNth<T>(this LinkedList<T> list, int n)
	{
		if (n < 0 || n >= list.Count)
		{
			throw new ArgumentException(null, nameof(n));
		}
		
		if (n == 0)
		{
			return list.RemoveFromFront();
		}

		var previous = list.Nth(n - 1);
		return list.RemoveAfter(previous);
	}

	/// <summary>
	/// Returns the number of elements in a sorted list that are less than the given value. 
	/// </summary>
	public static int SequentialRank<T>(this IReadonlyRandomAccessList<T> list, T item, IComparer<T> comparer)
	{
		Assert(IsSorted(list, comparer));
		
		for (int i = 0; i < list.Count; i++)
		{
			if (comparer.LessOrEqual(item, list[i]))
			{
				return i;
			}
		}
		
		return list.Count;
	}

	/// <summary>
	/// Returns the number of elements in a sorted list that are less than the given value. 
	/// </summary>
	public static int SequentialRank<T>(this LinkedList<T> list, [DisallowNull] T item, IComparer<T> comparer)
	{
		list.ThrowIfNull();
		item.ThrowIfNull();
		comparer.ThrowIfNull();
		
		Assert(IsSorted(list.ToResizableArray(list.Count), comparer));

		int i = 0;
		
		foreach (var listItem in list)
		{
			if (comparer.LessOrEqual(item, listItem))
			{
				return i;
			}

			i++;
		}

		return i;
	}

	[Obsolete, PossiblyIncorrect]
	public static int SequentialSearch(this int[] list, int key)
	{
		list.ThrowIfNull();
		
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i] == key)
			{
				return i;
			}
		}

		return -1;
	}

	/// <summary>
	/// Shuffles a list so that each permutation is equally likely.
	/// </summary>
	// Fisher-Yates shuffle from https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
	public static void Shuffle<T>(this IRandomAccessList<T> list)
	{
		list.ThrowIfNull();
		
		for (int i = list.Count - 1; i > 0; i--)
		{
			int j = Random.Next(i + 1); // Common mistake: to use i or list.Count here instead of i + 1
			list.SwapAt(i, j);
		}
	}
	
	/// <summary>
	/// This is a variant of the Fisher-Yates shuffle that shuffles only the first <paramref name="count"/> elements of
	/// the list.
	/// </summary>
	public static void Shuffle<T>(this IRandomAccessList<T> list, int count)
	{
		list.ThrowIfNull();
		int listCount = list.Count;

		if (count > listCount - 1)
		{
			count = listCount - 1;
		}

		for (int i = 0; i < count; i++)
		{
			int j = Generator.NextUniformRandomInt(i, listCount);
			list.SwapAt(i, j);
		}
	}

	/// <summary>
	/// Sorts a list and removes any duplicates. 
	/// </summary>
	/// <param name="list">The list to sort.</param>
	/// <typeparam name="T">The type of items in the list.</typeparam>
	// Ex. 2.5.4
	public static void SortAndRemoveDuplicates<T>(this ResizeableArray<T> list) 
		where T : IComparable<T>
	{
		bool EqualAt(int i, int j) => list[i].CompareTo(list[j]) == 0;
		
		list.ThrowIfNull();
		
		if (list.Count <= 1)
		{
			return;
		}
		
		ShellSortWithPrattSequence(list);

		int i = 0;
		int j = 1;
		
		while (j < list.Count)
		{
			if (EqualAt(i, j))
			{
				j++;
			}
			else
			{
				list[i + 1] = list[j];
				i++;
				j++;
			}
		}
			
		list.RemoveLast(list.Count - i - 1);
	}

	/// <summary>
	/// Sorts a list and removes duplicates by using a buffer. 
	/// </summary>
	/// <param name="list">The list to sort.</param>
	/// <typeparam name="T">The type of the items in the list.</typeparam>
	public static void SortAndRemoveDuplicatesWithBuffer<T>(this ResizeableArray<T> list) 
		where T : IComparable<T>
	{
		bool Equal(T left, T right) => left.CompareTo(right) == 0;
		list.ThrowIfNull();
		
		if (list.Count <= 1)
		{
			return;
		}
		
		ShellSortWithPrattSequence(list);
		
		var sorted = list
			.Buffer2()
			.Where(buffer => !Equal(buffer.First!, buffer.Last!)) // Buffer only returns when both elements are not null
			.Select(buffer => buffer.First!)
			.ToArray();

		// TODO: Add Copy method for arrays
		for (int i = 0; i < sorted.Length; i++)
		{
			list[i] = sorted[i];
		}
		
		list.RemoveLast(list.Count - sorted.Length);
	}
}
