namespace Algorithms_Sedgewick.PriorityQueue;

// 2.4.29
// Note: This version does not quit have the performance characteristics desired. 
// Getting all operations to be O (log n) may be quite tricky. 
public class MinMaxPriorityQueue<T> where T : IComparable<T>
{
	private readonly FixedCapacityMaxBinaryHeap<T> largestElements = new(1000);
	private readonly FixedCapacityMinBinaryHeap<T> smallestElements = new(1000);
	public T PeekMax => largestElements.PeekMax;

	public T PeekMin => smallestElements.PeekMin;

	public T PopMax()
	{
		var max = largestElements.PopMax();
		Rebalance();
		return max;
	}

	public T PopMin()
	{
		var min = smallestElements.PopMin();
		Rebalance();
		return min;
	}

	public void Push(T item)
	{
		smallestElements.Push(item);
		Rebalance();
	}

	/*
		This op is O(n). In practice performance may not be so bad and can be improved
		if we changed the push logic to be smarter about which list to push onto.
		
		One idea is to cache the last item transferred in this rebalancing - that
		should be close to the median, and select the smallest or biggest items list 
		depending on whether new items are smaller or larger than the media. 
	*/
	private void Rebalance()
	{
		if (smallestElements.Count > largestElements.Count + 1)
		{
			largestElements.Push(smallestElements.PopMax());
		}
		else if(largestElements.Count > smallestElements.Count + 1)
		{ //Can happen because of pop too many from largestElements
			smallestElements.Push(largestElements.PopMin());
		}
	}
}
