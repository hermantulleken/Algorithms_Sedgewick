namespace Algorithms_Sedgewick.Buffer;

using System.Collections;
using Queue;

/// <summary>
/// Represents a buffer with a fixed capacity.
/// </summary>
/// <typeparam name="T">The type of elements this buffer can hold.</typeparam>1
/// <param name="capacity">The capacity of this the new instance.</param>
public sealed class BufferWithQueue<T>(int capacity)
	: IBuffer<T>
{
	private readonly IQueue<T> queue = new QueueWithLinkedList<T>();

	/// <inheritdoc />
	public int Capacity { get; } = capacity;

	/// <inheritdoc />
	public int Count => queue.Count;

	/// <inheritdoc />
	public T First => queue.First();

	/// <inheritdoc />
	public T Last => queue.Peek;
	
	/// <inheritdoc />
	public void Clear() => queue.Clear();

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => queue.GetEnumerator();

	/// <inheritdoc />
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
