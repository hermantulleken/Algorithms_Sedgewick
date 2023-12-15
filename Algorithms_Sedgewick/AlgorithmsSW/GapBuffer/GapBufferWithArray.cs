namespace AlgorithmsSW.GapBuffer;

using System.Collections;
using List;

/// <summary>
/// Implements a gap buffer with an underlying array.
/// </summary>
public sealed class GapBufferWithArray<T> : IGapBuffer<T>, IRandomAccessList<T>
{
	// This is the same as the cursor index, but the user does not see it as a gap.
	private int gapStartIndex;
	private T[] items;
	private int rightBlockStartIndex;

	private int version;

	public int Capacity => items.Length;

	public int Count => items.Length - GapSize;

	public int CursorIndex => gapStartIndex;

	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets or sets the element at the specified index.
	/// </summary>
	/// <param name="index">The index of the element to get or set.</param>
	/// <returns>The element at the specified index.</returns>
	public T this[int index]
	{
		get
		{
			index.ThrowIfOutOfRange(0, Count);

			return 
				index < gapStartIndex
					? items[index] 
					: items[index + GapSize];
		}
		
		set
		{
			index.ThrowIfOutOfRange(0, Count);

			if (index < gapStartIndex)
			{
				items[index] = value;
			}
			else
			{
				items[index + GapSize] = value;
			}
		}
	}

	private int GapSize => rightBlockStartIndex - gapStartIndex;

	private bool IsFull => GapSize == 0;

	IGapBuffer<T> IGapBuffer<T>.@this => @this;

	IGapBuffer<T> @this => this;

	public GapBufferWithArray(int initialCapacity)
	{
		items = new T[initialCapacity];
		gapStartIndex = 0;
		rightBlockStartIndex = initialCapacity;
		version = 0;
	}

	public void AddAfter(T item)
	{
		if (IsFull)
		{
			Grow();
		}

		rightBlockStartIndex--;
		items[rightBlockStartIndex] = item;
		version++;
	}

	public void AddBefore(T item)
	{
		if (IsFull)
		{
			Grow();
		}

		items[gapStartIndex] = item;
		gapStartIndex++;
		version++;
	}

	public IEnumerator<T> GetEnumerator()
	{
		int versionAtStartOfIteration = version;
		
		for (int i = 0; i < gapStartIndex; i++)
		{
			version.ThrowIfVersionMismatch(versionAtStartOfIteration);
			
			yield return items[i];
		}

		for (int i = rightBlockStartIndex; i < items.Length; i++)
		{
			version.ThrowIfVersionMismatch(versionAtStartOfIteration);
			
			yield return items[i];
		}
	}

	public void MoveCursorBy(int offset)
	{
		void MoveCursorLeft()
		{
			gapStartIndex--;
			rightBlockStartIndex--;
			items[rightBlockStartIndex] = items[gapStartIndex];
			items[gapStartIndex] = default!;
		}

		void MoveCursorRight()
		{
			items[gapStartIndex] = items[rightBlockStartIndex];
			items[rightBlockStartIndex] = default!;
			gapStartIndex++;
			rightBlockStartIndex++;
		}

		@this.ValidateCursor(CursorIndex + offset);
		
		if (offset > 0)
		{
			for (int i = 0; i < offset; i++)
			{
				MoveCursorRight();
			}
		}
		else
		{
			for (int i = 0; i < -offset; i++)
			{
				MoveCursorLeft();
			}
		}
	}

	public T RemoveAfter()
	{
		if (rightBlockStartIndex == items.Length)
		{
			ThrowHelper.ThrowGapAtEnd();
		}
		
		var result = items[rightBlockStartIndex];
		items[rightBlockStartIndex] = default!;
		rightBlockStartIndex++;
		version++;
		
		return result;
	}

	public T RemoveBefore()
	{
		if (gapStartIndex == 0)
		{
			ThrowHelper.ThrowGapAtBeginning();
		}
		
		gapStartIndex--;
		T result = items[gapStartIndex];
		items[gapStartIndex] = default!;
		version++;
		
		return result;
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void Grow()
	{
		var newItems = new T[Capacity * 2];

		for (int i = 0; i < gapStartIndex; i++)
		{
			newItems[i] = items[i];
		}

		for (int i = rightBlockStartIndex; i < Capacity; i++)
		{
			newItems[i + Capacity] = items[i];
		}
		
		rightBlockStartIndex += Capacity;
		items = newItems; // This also changes the capacity
	}
}
