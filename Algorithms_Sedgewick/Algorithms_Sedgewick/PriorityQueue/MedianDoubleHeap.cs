using System.Diagnostics;

namespace Algorithms_Sedgewick.PriorityQueue;

/// <summary>
/// Container that supports efficient insertions and retrieval and removal of the median element in the container.  
/// </summary>
// Ex. 2.4.30
//This implementation uses a wrapper max heap that may npt be so performant. 
public sealed class MedianDoubleHeap<T> where T : IComparable<T>
{
	private readonly struct InvertedComparable : IComparable<InvertedComparable>
	{
		public T Item { get; }

		public InvertedComparable(T item)
		{
			Item = item;
		}

		public int CompareTo(InvertedComparable other) => other.Item.CompareTo(Item);
	}

	public sealed class MaxHeap
	{
		private readonly FixedCapacityMinBinaryHeap<InvertedComparable> minHeap;

		public int Count => minHeap.Count;

		public MaxHeap(int capacity)
		{
			minHeap = new FixedCapacityMinBinaryHeap<InvertedComparable>(capacity);
		}

		public void Push(T item) => minHeap.Push(new InvertedComparable(item));
		public T PopMax() => minHeap.PopMin().Item;
		public T Peek => minHeap.PeekMin.Item;

		public override string ToString() => minHeap.ToString();
	}

	private readonly MaxHeap smallestHalf;
	private readonly FixedCapacityMinBinaryHeap<T> biggestHalf;

	public MedianDoubleHeap(int capacity)
	{
		biggestHalf = new FixedCapacityMinBinaryHeap<T>(capacity);
		smallestHalf = new MaxHeap(capacity);
	}

	public T PeekMedian => smallestHalf.Peek;

	public void Push(T item)
	{
		if (Sort.Sort.Less(item, PeekMedian))
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
