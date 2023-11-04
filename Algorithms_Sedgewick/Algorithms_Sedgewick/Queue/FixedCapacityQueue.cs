using System.Collections;
using Algorithms_Sedgewick.Object;
using Support;

namespace Algorithms_Sedgewick.Queue;

/// <summary>
/// A queue with a fixed capacity.
/// </summary>
/// <typeparam name="T">The type of the queue's items.</typeparam>
public sealed class FixedCapacityQueue<T> : IQueue<T>
{
	private readonly T?[] items;
	private int version = 0;
	private int head = 0;
	private int tail = 0;
	
	private static IdGenerator idGenerator = new();
	
	public int Id { get; } = idGenerator.GetNextId();

	public int Capacity { get; }

	public int Count { get; private set; }

	public bool IsEmpty => Count == 0;

	public bool IsFull => Count == Capacity;

	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items[head]!;
		}
	}

	public FixedCapacityQueue(int capacity)
	{
		items = capacity switch
		{
			< 0 => throw ThrowHelper.CapacityCannotBeNegativeException(capacity),
			0 => Array.Empty<T>(),
			_ => new T[capacity],
		};

		Capacity = capacity;
	}

	public void Clear()
	{
		for (int i = 0; i < Count; i++)
		{
			items[(head + i) % Capacity] = default;
		}

		Count = 0;
		head = 0;
		tail = 0;
	}

	public IEnumerator<T> GetEnumerator()
	{
		int versionAtStartOfIteration = version;

		for (int i = 0; i < Count; i++)
		{
			ValidateVersion(versionAtStartOfIteration);

			yield return items[(head + i) % Capacity]!;
		}
	}

	public T Dequeue()
	{
		ValidateNotEmpty();

		var result = items[head];
		items[head] = default; // Clear the reference
		head = (head + 1) % Capacity; // Move the head to the next position
		Count--;
		version++;

		return result!;
	}

	public void Enqueue(T item)
	{
		if (IsFull)
		{
			ThrowHelper.ThrowContainerFull();
		}

		items[tail] = item;
		tail = (tail + 1) % Capacity; // Move the tail to the next position
		Count++;
		version++;
	}

	public override string ToString() => this.Pretty();
	
	public override int GetHashCode() => Id.GetHashCode();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}

	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			ThrowHelper.ThrowIteratingOverModifiedContainer();
		}
	}
}
