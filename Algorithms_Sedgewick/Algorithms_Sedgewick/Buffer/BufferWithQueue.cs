namespace Algorithms_Sedgewick.Buffer;

using System.Collections;
using Queue;

public sealed class BufferWithQueue<T> : IBuffer<T>
{
	private readonly IQueue<T> queue;

	public int Capacity { get; }

	public int Count => queue.Count;

	public T First => queue.First();

	public T Last => queue.Peek;

	public BufferWithQueue(int capacity)
	{
		Capacity = capacity;
		queue = new QueueWithLinkedList<T>();
	}

	public void Clear() => queue.Clear();

	public IEnumerator<T> GetEnumerator() => queue.GetEnumerator();

	public void Insert(T item)
	{
		if (queue.Count == Capacity)
		{
			queue.Dequeue();
		}
        
		queue.Enqueue(item);
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
