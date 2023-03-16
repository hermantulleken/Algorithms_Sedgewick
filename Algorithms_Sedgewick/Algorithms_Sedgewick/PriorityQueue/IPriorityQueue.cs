using Algorithms_Sedgewick.Stack;
using Support;

namespace Algorithms_Sedgewick.PriorityQueue;

public interface IPriorityQueue<T> where T : IComparable<T>
{
	public int Count { get; }
	public T PeekMin { get; }
	
	public T PopMin();
	public void Push(T item);
}

public static class PriorityQueueExtensions
{
	public static bool IsEmpty<T>(this IPriorityQueue<T> queue) where T : IComparable<T>
		=> queue.Count == 0;
	
	public static bool IsSingleton<T>(this IPriorityQueue<T> queue) where T : IComparable<T>
		=> queue.Count == 1;
}


// 2.4.21
public class StackWithPriorityQueue<T> 
{
	private readonly struct PriorityNode<T> : IComparable<PriorityNode<T>>
	{
		public readonly T Item;
		private readonly int priority;

		public PriorityNode(T item, int priority)
		{
			Item = item;
			this.priority = priority;
		}

		public int CompareTo(PriorityNode<T> other) => priority.CompareTo(other.priority);
	}
	
	private const int Capacity = 1000;
	private readonly FixedCapacityMinBinaryHeap<PriorityNode<T>> queue = new(Capacity);
	private int counter = Capacity;

	public int Count => queue.Count;

	public T Peek => queue.PeekMin.Item;
	public void Push(T item)
	{
		queue.Push(new PriorityNode<T>(item, counter));
		counter--;//For queue, use ++, for random queue use a random value instead of counter
	}

	public T Pop()
	{
		var min = queue.PopMin().Item;
		counter++;//For queue, use --
		return min;
	}
}
