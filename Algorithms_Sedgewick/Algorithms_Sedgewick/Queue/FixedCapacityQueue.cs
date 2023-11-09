using System.Collections;
using Algorithms_Sedgewick.Object;
using Support;

namespace Algorithms_Sedgewick.Queue;

/// <summary>
/// A queue with a fixed capacity.
/// </summary>
/// <typeparam name="T">The type of the queue's items.</typeparam>
/// <exception cref="ArgumentException"><paramref name="capacity"/> is negative.</exception>
public sealed class FixedCapacityQueue<T>(int capacity)
	: IQueue<T>
{
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();
	
	private readonly T?[] items = capacity switch
	{
		< 0 => throw ThrowHelper.CapacityCannotBeNegativeException(capacity),
		0 => Array.Empty<T>(),
		_ => new T[capacity],
	};
	
	private int version = 0;
	private int head = 0;
	private int tail = 0;

	/// <summary>
	/// Gets the capacity for this instance.
	/// </summary>
	public int Capacity { get; } = capacity;

	/// <inheritdoc />
	public int Count { get; private set; }

	/// <inheritdoc />
	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets a value indicating whether the queue is full, that is <see cref="Count"/> equals <see cref="Capacity"/>.
	/// </summary>
	public bool IsFull => Count == Capacity;
	
	/// <inheritdoc />
	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items[head]!;
		}
	}
	
	private int Id { get; } = IdGenerator.GetNextId();
	
	/// <inheritdoc />
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

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator()
	{
		int versionAtStartOfIteration = version;

		for (int i = 0; i < Count; i++)
		{
			ValidateVersion(versionAtStartOfIteration);

			yield return items[(head + i) % Capacity]!;
		}
	}

	/// <inheritdoc />
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

	/// <inheritdoc />
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

	/// <inheritdoc />
	public override string ToString() => this.Pretty();

	/// <inheritdoc />
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
