namespace Algorithms_Sedgewick.PriorityQueue;

public static class PriorityQueueExtensions
{
	public static bool IsEmpty<T>(this IPriorityQueue<T> queue) where T : IComparable<T>
		=> queue.Count == 0;

	public static bool IsSingleton<T>(this IPriorityQueue<T> queue) where T : IComparable<T>
		=> queue.Count == 1;
}