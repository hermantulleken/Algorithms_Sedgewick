namespace AlgorithmsSW.PriorityQueue;

using List;

public static class PriorityQueueExtensions
{
	public static bool IsEmpty<T>(this IPriorityQueue<T> queue) 
		=> queue.Count == 0;

	public static bool IsSingleton<T>(this IPriorityQueue<T> queue)
		=> queue.Count == 1;

	/// <summary>
	/// Converts the queue to an array, sorted by priority.
	/// </summary>
	/// <param name="queue">The queue to convert.</param>
	/// <typeparam name="T">The type of the elements in the queue.</typeparam>
	/// <returns>An array containing the elements in the queue, sorted by priority.</returns>
	public static T[] ToSortedArray<T>(this IPriorityQueue<T> queue)
	{
		var array = new T[queue.Count];

		for (int i = 0; i < array.Length; i++)
		{
			array[i] = queue.PopMin();
		}

		return array;
	}
	
	/// <summary>
	/// Converts the queue to a list, sorted by priority.
	/// </summary>
	/// <param name="queue">The queue to convert.</param>
	/// <typeparam name="T">The type of the elements in the queue.</typeparam>
	/// <returns>An array containing the elements in the queue, sorted by priority.</returns>
	public static IRandomAccessList<T> ToSortedList<T>(this IPriorityQueue<T> queue)
	{
		var list = new ResizeableArray<T>(queue.Count);

		while (!queue.IsEmpty())
		{
			list.Add(queue.PopMin());
		}
		
		return list;
	}
}
