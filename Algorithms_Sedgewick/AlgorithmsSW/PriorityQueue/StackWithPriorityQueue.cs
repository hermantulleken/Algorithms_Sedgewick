namespace AlgorithmsSW.PriorityQueue;

[ExerciseReference(2, 4, 21)]
public class StackWithPriorityQueue<T>(IComparer<T> comparer)
{
	private readonly struct PriorityNode
	{
		public readonly T Item;
		private readonly int priority;

		public PriorityNode(T item, int priority)
		{
			Item = item;
			this.priority = priority;
		}
	}
	
	private class PriorityNodeComparer : IComparer<PriorityNode>
	{
		private readonly IComparer<T> comparer;

		public PriorityNodeComparer(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		public int Compare(PriorityNode x, PriorityNode y) =>  comparer.Compare(x.Item, y.Item);
	}

	private const int Capacity = 1000;
	private readonly FixedCapacityMinBinaryHeap<PriorityNode> queue = new(Capacity, new PriorityNodeComparer(comparer));
	private int counter = Capacity;

	public int Count => queue.Count;

	public T Peek => queue.PeekMin.Item;

	public T Pop()
	{
		var min = queue.PopMin().Item;
		counter++; // For queue, use --
		return min;
	}

	public void Push(T item)
	{
		queue.Push(new PriorityNode(item, counter));
		counter--; // For queue, use ++, for random queue use a random value instead of counter
	}
}
