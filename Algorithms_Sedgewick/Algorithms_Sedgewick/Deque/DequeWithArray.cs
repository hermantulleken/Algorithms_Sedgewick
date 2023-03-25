using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Support;

namespace Algorithms_Sedgewick.Deque;

public class DequeWithArray<T> : IDeque<T>
{
	private T[] items;
	private int leftIndex;
	private int rightIndex;

	public int Count { get; private set; }

	public T PeekLeft
	{
		get
		{
			if (Count == 0)
			{
				throw new InvalidOperationException("Deque is empty");
			}

			return items[leftIndex];
		}
	}

	public T PeekRight
	{
		get
		{
			if (Count == 0)
			{
				throw new InvalidOperationException("Deque is empty");
			}

			return items[rightIndex];
		}
	}

	public DequeWithArray() => Clear();

	[MemberNotNull(nameof(items))]
	public void Clear()
	{
		items = new T[4];
		leftIndex = items.Length / 2;
		rightIndex = items.Length / 2 - 1;
		Count = 0;
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (int i = leftIndex; i <= rightIndex; i++)
		{
			yield return items[i];
		}
	}

	public T PopLeft()
	{
		if (Count == 0)
		{
			throw new InvalidOperationException("Deque is empty");
		}

		T item = items[leftIndex];
		items[leftIndex] = default!;
		leftIndex++;
		Count--;

		ShrinkIfNecessary();

		return item;
	}
	
	public T PopRight()
	{
		if (Count == 0)
		{
			throw new InvalidOperationException("Deque is empty");
		}

		T item = items[rightIndex];
		items[rightIndex] = default!;
		rightIndex--;
		Count--;

		ShrinkIfNecessary();

		return item;
	}

	public void PushLeft(T item)
	{
		if (Count == items.Length)
		{
			Resize(items.Length * 2);
		}

		leftIndex--;
		items[leftIndex] = item;
		Count++;
	}

	public void PushRight(T item)
	{
		if (Count == items.Length)
		{
			Resize(items.Length * 2);
		}

		rightIndex++;
		items[rightIndex] = item;
		Count++;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void Resize(int capacity)
	{
		var resized = new T[capacity];
		int newLeftIndex = (capacity - Count) / 2;
		int newRightIndex = newLeftIndex + Count - 1;

		for (int i = leftIndex, j = newLeftIndex; i <= rightIndex; i++, j++)
		{
			resized[j] = items[i];
		}

		items = resized;
		leftIndex = newLeftIndex;
		rightIndex = newRightIndex;
	}
	
	[Conditional(Diagnostics.ShrinkDynamicContainers)]
	private void ShrinkIfNecessary()
	{
		if (Count > 0 && Count <= items.Length / 4)
		{
			Resize(items.Length / 2);
		}
	}
}