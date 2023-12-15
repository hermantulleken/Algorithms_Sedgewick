namespace AlgorithmsSW.PriorityQueue;

public sealed class FixedCapacityMaxBinaryHeap<T> 
{
	private readonly FixedCapacityMinBinaryHeap<T> minHeap;

	public int Count => minHeap.Count;
	
	public T PeekMax => minHeap.PeekMin;

	public FixedCapacityMaxBinaryHeap(int capacity, IComparer<T> comparer)
	{
		minHeap = new FixedCapacityMinBinaryHeap<T>(capacity, comparer.Invert());
	}

	public T PopMax() => minHeap.PopMin();

	// This op is O(n) 
	public T PopMin() => minHeap.PopMax();

	public void Push(T item) => minHeap.Push(item);

	public override string ToString() => minHeap.ToString();
}
