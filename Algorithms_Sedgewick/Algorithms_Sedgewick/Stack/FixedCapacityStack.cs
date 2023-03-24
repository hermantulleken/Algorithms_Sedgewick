using System.Collections;
using Support;

namespace Algorithms_Sedgewick.Stack;

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
	public bool IsEmpty => Count == 0;
	public bool IsFull => Count == Capacity;

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
			< 0 => throw ThrowHelper.CapacityCannotBeNegativeException(capacity),
			0 => Array.Empty<T>(),
			_ => new T[capacity]
		};

		Capacity = capacity;
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

	public T Pop()
	{
		ValidateNotEmpty();

		Count--;
		var result = items[Count];
		items[Count] = default; //Don't hold on to references we don't need, i.e. don't let items loiter.
		version++;
			
		return result;
	}

	public void Push(T item)
	{
		if (IsFull)
		{
			ThrowHelper.ThrowContainerFull();
		}

		items[Count] = item;
		Count++;
		version++;
	}

	public override string ToString() => this.Pretty();

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
