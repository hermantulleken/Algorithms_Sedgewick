using System.Diagnostics;

namespace Algorithms_Sedgewick.PriorityQueue;

/// <summary>
/// Container that supports efficient insertions and retrieval and removal of the median element in the container.  
/// </summary>
// Ex. 2.4.30
//This implementation uses a wrapper max heap that may npt be so performant. 
public sealed class MedianDoubleHeap<T> where T : IComparable<T>
{
	

	private readonly FixedCapacityMaxBinaryHeap<T> smallestHalf;
	private readonly FixedCapacityMinBinaryHeap<T> biggestHalf;

	public MedianDoubleHeap(int capacity)
	{
		biggestHalf = new FixedCapacityMinBinaryHeap<T>(capacity);
		smallestHalf = new FixedCapacityMaxBinaryHeap<T>(capacity);
	}

	public T PeekMedian => smallestHalf.PeekMax;

	public void Push(T item)
	{
		if (Sort.Less(item, PeekMedian))
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

	public void AssertHeapSizeInvariant()
	{
		Debug.Assert(smallestHalf.Count == biggestHalf.Count || smallestHalf.Count == biggestHalf.Count + 1);
	}
}
