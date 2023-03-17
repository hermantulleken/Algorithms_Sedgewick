using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Queue;
using static System.Diagnostics.Debug;
using static Algorithms_Sedgewick.Sort.Sort;

namespace Algorithms_Sedgewick;

public static class AlgorithmExtensions
{
	private static readonly Random Random = new ();

	public static int FindIndexOfMin<T>(this IReadonlyRandomAccessList<T> list) where T : IComparable<T>
		=> list.FindIndexOfMin(0, list.Count);
	public static int FindIndexOfMin<T>(this IReadonlyRandomAccessList<T> list, int start, int end) where T : IComparable<T>
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

	public static int FindIndexOfMax<T>(this IReadonlyRandomAccessList<T> list) where T : IComparable<T>
		=> list.FindIndexOfMax(0, list.Count);
	
	public static int FindIndexOfMax<T>(this IReadonlyRandomAccessList<T> list, int start, int end) where T : IComparable<T>
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
	
	public static int FindIndexOfMin<T>(this T[] list) where T : IComparable<T>
		=> list.FindIndexOfMin(0, list.Length);
	public static int FindIndexOfMin<T>(this T[] list, int start, int end) where T : IComparable<T>
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

	public static int FindIndexOfMax<T>(this T[] list) where T : IComparable<T>
		=> list.FindIndexOfMax(0, list.Length);
	
	public static int FindIndexOfMax<T>(this T[] list, int start, int end) where T : IComparable<T>
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

	/// <summary>
	/// Finds the index in a sorted list at which an item can be inserted so that all the
	/// elements to the left are smaller or equal, and all the elements to the right are larger
	/// than the item.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the list. Must implement <see cref="IComparable{T}"/>.</typeparam>
	/// <param name="sortedList">The sorted list to search for the item.</param>
	/// <param name="item">The item to find the index for.</param>
	/// <returns>The index at which the item can be inserted while maintaining the sorted order.</returns>
	/// <remarks>
	/// If the item is smaller than all the items in the list, the method returns <c>0</c>.
	/// If the item is greater than all the items in the list, the method returns <c>sortedList.Count</c>.
	/// </remarks>
	/*
		We return the last (and not first or any other) index that preserves the order of the list if the item is inserted
		so that the number of elements we need to move when inserting the item is minimal. 
	*/
	public static int FindInsertionIndex<T>(this IReadonlyRandomAccessList<T> sortedList, T item) where T : IComparable<T>
	{
	    // Ensure that the input list is sorted.
	    Assert(IsSorted(sortedList));
	    
	    // Define a binary search algorithm to find the insertion point for the item.
	    int Find(int start, int end)
	    {
	        while (end > start + 1)
	        {
	            int mid = (start + end) / 2;
	    
	            Assert(start != mid); //because end > start + 1
	            Assert(mid != end);

	            // If the item is less than the midpoint, search the left half of the list.
	            if (Less(item, sortedList[mid]))
	            {
	                end = mid;
	            }
	            // Otherwise, search the right half of the list.
	            else
	            {
	                start = mid;
	            }
	        }
	        
	        // When there are only two elements left, return the index of the correct element.
	        Assert(end == start + 1);
	        return Less(item, sortedList[start]) ? start : end;
	    }

	    // Handle special cases where the list is empty or the item is outside the bounds of the list.
	    return sortedList.IsEmpty 
	        ? 0 
	        : Less(item, sortedList[0]) 
	            ? 0 
	            : Less(sortedList[^1], item) 
	                ? sortedList.Count 
	                : Find(0, sortedList.Count);
	    
	}

	public static T First<T>(this IReadonlyRandomAccessList<T> source)
	{
		source
			.ThrowIfNull()
			.ThrowIfEmpty();

		return source[0];
	}

	public static void InsertSorted<T>(this ResizeableArray<T> list, T item) where T : IComparable<T>
	{
		Assert(IsSorted(list));
		
		int insertionIndex = list.FindInsertionIndex(item);
		list.InsertAt(item, insertionIndex);
	}

	public static void InsertSorted<T>(this List.LinkedList<T> list, T item) where T : IComparable<T>
	{
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

	public static IEnumerable<List.LinkedList<T>.Node> NodesBeforeThat<T>(this List.LinkedList<T> list, Func<List.LinkedList<T>.Node, bool> predicate)
	{
		if (list.IsEmpty || list.Count == 1)
		{
			return Array.Empty<List.LinkedList<T>.Node>();
		}
		
		return list.Nodes
			.SkipLast(1)
			.Where(node => predicate(node.NextNode));
	}

	public static List.LinkedList<T>.Node Nth<T>(this List.LinkedList<T> list, int n) => list.Nodes.Skip(n).First();

	public static void RemoveIf<T>(this List.LinkedList<T> list, Func<List.LinkedList<T>.Node, bool> predicate)
	{
		if (list.IsEmpty)
		{
			return;
		}

		var current = list.First;

		while (current != null && predicate(current))
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

	public static List.LinkedList<T>.Node RemoveNth<T>(this List.LinkedList<T> list, int n)
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
	/// Shuffles a list so that each permutation is equally likely
	/// </summary>
	// Fisher-Yates shuffle from https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
	public static void Shuffle<T>(this IReadonlyRandomAccessList<T> list)
	{
		for (int i = list.Count - 1; i > 0; i--)
		{
			int j = Random.Next(i + 1); //Common mistake: to use i or list.Count here instead of i + 1
			SwapAt(list, i, j);
		}
	}
}
