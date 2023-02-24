using System.Collections;

namespace Algorithms_Sedgewick;

/// <summary>
/// A stack with a fixed capacity;
/// </summary>
/// <typeparam name="T">The type of the stack's items.</typeparam>
/// <remarks>From p. 132.</remarks>
public sealed class FixedCapacityStack<T> : IStack<T>
{
	private readonly T[] items;
	private int version = 0;

	public int Capacity { get; }
	public int Count { get; private set; }
	public bool IsFull => Count == Capacity;
	public bool IsEmpty => Count == 0;

	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items[^1];
		}
	}

	public FixedCapacityStack(int capacity)
	{
		items = capacity switch
		{
			< 0 => throw new ArgumentException(ContainerErrorMessages.CapacityCannotBeNegative, nameof(capacity)),
			0 => Array.Empty<T>(),
			_ => new T[capacity]
		};

		Capacity = capacity;
	}

	public void Push(T item)
	{
		if (IsFull)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerFull);
		}

		items[Count] = item;
		Count++;
		version++;
	}

	public T Pop()
	{
		ValidateNotEmpty();

		Count--;
		var result = items[Count];
		items[Count] = default; //Don't hold on to references we don't need, i.e. don't let items loiter.
		version++;
			
		return result;
	}

	public void Clear()
	{
		for (int i = 0; i < Count; i++)
		{
			items[i] = default;
		}

		Count = 0;
	}

	public IEnumerator<T> GetEnumerator()
	{
		int versionAtStartOfIteration = version;
			
		for (int i = 0; i < Count; i++)
		{
			ValidateVersion(versionAtStartOfIteration);
				
			yield return items[i];
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			throw new InvalidOperationException(ContainerErrorMessages.IteratingOverModifiedList);
		}
	}

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}
	}
}
