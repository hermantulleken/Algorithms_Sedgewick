﻿using System.Diagnostics;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.PriorityQueue;

/// <summary>
/// Container that supports efficient insertions and retrieval and removal of the median element in the container.  
/// </summary>
// Ex. 2.4.30
// This implementation uses a wrapper max heap that may npt be so performant. 
public sealed class MedianDoubleHeap<T> 
	where T : IComparable<T>
{
	private readonly FixedCapacityMinBinaryHeap<T> biggestHalf;
	private readonly FixedCapacityMaxBinaryHeap<T> smallestHalf;

	public T PeekMedian => smallestHalf.PeekMax;

	public MedianDoubleHeap(int capacity)
	{
		biggestHalf = new FixedCapacityMinBinaryHeap<T>(capacity);
		smallestHalf = new FixedCapacityMaxBinaryHeap<T>(capacity);
	}

	public void AssertHeapSizeInvariant()
	{
		Debug.Assert(smallestHalf.Count == biggestHalf.Count || smallestHalf.Count == biggestHalf.Count + 1);
	}

	public T PopMedian()
	{
		var median = smallestHalf.PopMax();

		if (smallestHalf.Count < biggestHalf.Count)
		{
			smallestHalf.Push(biggestHalf.PopMin());
		}
		
		AssertHeapSizeInvariant();

		return median;
	}

	public void Push(T item)
	{
		if (ListExtensions.Less(item, PeekMedian))
		{
			smallestHalf.Push(item);
		}
		else
		{
			biggestHalf.Push(item);
		}

		if (smallestHalf.Count > biggestHalf.Count + 1)
		{
			biggestHalf.Push(smallestHalf.PopMax());
		}
		
		AssertHeapSizeInvariant();
	}
}
