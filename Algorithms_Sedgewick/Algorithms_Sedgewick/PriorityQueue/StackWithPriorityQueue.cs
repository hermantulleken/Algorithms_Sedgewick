namespace Algorithms_Sedgewick.PriorityQueue;

// 2.4.21
public class StackWithPriorityQueue<T>
{
	private readonly struct PriorityNode : IComparable<PriorityNode>
	{
		public readonly T Item;
		private readonly int priority;

		public PriorityNode(T item, int priority)
		{
			Item = item;
			this.priority = priority;
		}

		public int CompareTo(PriorityNode other) => priority.CompareTo(other.priority);
	}

	private const int Capacity = 1000;
	private readonly FixedCapacityMinBinaryHeap<PriorityNode> queue = new(Capacity);
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
