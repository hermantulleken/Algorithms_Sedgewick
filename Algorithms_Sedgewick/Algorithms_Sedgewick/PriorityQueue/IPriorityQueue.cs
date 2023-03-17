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
