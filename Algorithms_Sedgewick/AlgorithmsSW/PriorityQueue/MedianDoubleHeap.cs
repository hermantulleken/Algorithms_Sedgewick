using System.Diagnostics;
using AlgorithmsSW.List;

namespace AlgorithmsSW.PriorityQueue;

using Support;

/// <summary>
/// Container that supports efficient insertions and retrieval and removal of the median element in the container.  
/// </summary>
[ExerciseReference(2, 4, 30)]
// This implementation uses a wrapper max heap that may npt be so performant. 
public sealed class MedianDoubleHeap<T>
{
	private readonly FixedCapacityMinBinaryHeap<T> biggestHalf;
	private readonly FixedCapacityMaxBinaryHeap<T> smallestHalf;
	private readonly IComparer<T> comparer;

	public T PeekMedian => smallestHalf.PeekMax;

	public MedianDoubleHeap(int capacity, IComparer<T> comparer)
	{
		this.comparer = comparer;
		biggestHalf = new FixedCapacityMinBinaryHeap<T>(capacity, comparer);
		smallestHalf = new FixedCapacityMaxBinaryHeap<T>(capacity, comparer);
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
		if (comparer.Less(item, PeekMedian))
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
