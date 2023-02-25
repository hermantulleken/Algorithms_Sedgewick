using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Support;

namespace Algorithms_Sedgewick.List;

public sealed class ResizeableArray<T> : IRandomAccessList<T>
{
	[NotNull]
	private T[] items;
	private int version;

	public int Capacity { get; private set; }
	public int Count { get; private set; }
	public bool IsFull => Count == Capacity;
	public bool IsEmpty => Count == 0;

	public T this[int index]
	{
		get
		{
			ValidateIndex(index);

			return items[index];
		}

		set
		{
			ValidateIndex(index);
			items[index] = value;
			version++;
		}
	}

	public IRandomAccessList<T> Copy()
	{
		var copy = new ResizeableArray<T>(Capacity);

		for (int i = 0; i < Count; i++)
		{
			copy[i] = this[i];
		}

		return copy;
	}

	public ResizeableArray(int capacity = ResizeableArray.DefaultCapacity)
	{
		items = capacity switch
		{
			< 0 => throw new ArgumentException(ContainerErrorMessages.CapacityCannotBeNegative, nameof(capacity)),
			0 => Array.Empty<T>(),
			_ => new T[capacity]
		};

		version = 0;
		Capacity = capacity;
	}

	public void Add(T item)
	{
		if (IsFull)
		{
			Grow();
		}
			
		items[Count] = item;
		Count++;
		version++;
	}

	public T RemoveLast()
	{
		if (IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}

		Count--;
		var result = items[Count];
		items[Count] = default; //Don't hold on to references we don't need.
		version++;
			
		return result;
			
	}

	public override string ToString() => Formatter.Pretty(this);

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
		
	private void Grow()
	{
		switch (Capacity)
		{
			case ResizeableArray.MaxCapacity:
				throw new Exception(ContainerErrorMessages.TheContainerIsAtMaximumCapacity);
			case < ResizeableArray.HalfMaxCapacity:
				Capacity *= 2;
				break;
			default:
				Capacity = ResizeableArray.MaxCapacity;
				break;
		}

		var newItems = new T[Capacity];

		for (int i = 0; i < items.Length; i++)
		{
			newItems[i] = items[i];
		}

		items = newItems;
	}
		
	private void ValidateIndex(int index)
	{
		if (index < 0 || index >= Count)
		{
			throw new IndexOutOfRangeException();
		}
	}

	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			throw new InvalidOperationException(ContainerErrorMessages.IteratingOverModifiedList);
		}
	}
	
	public void Clear()
	{
		for (int i = 0; i < Count; i++)
		{
			items[i] = default;
		}
		
		Count = 0;
		version++;
	}
}

public static class ResizeableArray
{
	internal const int DefaultCapacity = 16;
	internal const int MaxCapacity = int.MaxValue;
	internal const int HalfMaxCapacity = MaxCapacity >> 1;
}
