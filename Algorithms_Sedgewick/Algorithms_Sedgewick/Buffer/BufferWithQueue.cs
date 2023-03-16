using System.Collections;
using Algorithms_Sedgewick.Queue;

namespace Algorithms_Sedgewick.Buffer;

public sealed class BufferWithQueue<T> : IBuffer<T>
{
	private readonly IQueue<T> queue;

	public int Count => queue.Count;
	public int Capacity { get; }
	public T First => queue.First();
	public T Last => queue.Peek;

	public BufferWithQueue(int capacity)
	{
		Capacity = capacity;
		queue = new QueueWithLinkedList<T>();
	}

	public void Insert(T item)
	{
		if (queue.Count == Capacity)
		{
			queue.Dequeue();
		}
        
		queue.Enqueue(item);
	}

	public void Clear() => queue.Clear();
	public IEnumerator<T> GetEnumerator() => queue.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
