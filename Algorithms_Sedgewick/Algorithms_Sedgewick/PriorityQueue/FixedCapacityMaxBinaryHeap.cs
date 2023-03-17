namespace Algorithms_Sedgewick.PriorityQueue;

public sealed class FixedCapacityMaxBinaryHeap<T> where T : IComparable<T>
{
	private readonly struct InvertedComparable : IComparable<InvertedComparable>
	{
		public T Item { get; }

		public InvertedComparable(T item)
		{
			Item = item;
		}

		public int CompareTo(InvertedComparable other) => other.Item.CompareTo(Item);

		public override bool Equals(object obj)
			=> obj is InvertedComparable comparable && CompareTo(comparable) == 0;

		public override int GetHashCode() => Item.GetHashCode();
	}
	
	private readonly FixedCapacityMinBinaryHeap<InvertedComparable> minHeap;

	public int Count => minHeap.Count;

	public FixedCapacityMaxBinaryHeap(int capacity)
	{
		minHeap = new FixedCapacityMinBinaryHeap<InvertedComparable>(capacity);
	}

	public void Push(T item) => minHeap.Push(new InvertedComparable(item));
	public T PopMax() => minHeap.PopMin().Item;
	public T PeekMax => minHeap.PeekMin.Item;

	public override string ToString() => minHeap.ToString();

	//This op is O(n) 
	public T PopMin() => minHeap.PopMax().Item;
}
