namespace Algorithms_Sedgewick.List;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Support;
using static System.Diagnostics.Debug;

public static class ResizeableArray
{
	internal const int DefaultCapacity = 16;
	internal const int HalfMaxCapacity = MaxCapacity >> 1;
	internal const int MaxCapacity = int.MaxValue;
}

[SuppressMessage(
	"StyleCop.CSharp.MaintainabilityRules", 
	"SA1402:File may only contain a single type", 
	Justification = "Generic and Simple version goes together.")]
public sealed class ResizeableArray<T> : IReadonlyRandomAccessList<T>
{
	/*
		This array may have null elements when
			1. this class is below capacity and T is a reference type, 
				so the last values are all null. These nulls are never 
				exposed to the caller.
			2. T is nullable, and so null values could have been inserted.  
	*/
	private T[] items;

	private int version;

	public int Capacity { get; private set; }

	/// <inheritdoc />
	public int Count { get; private set; }

	/// <inheritdoc/>
	public bool IsEmpty => Count == 0;

	public bool IsFull => Count == Capacity;

	/// <inheritdoc />
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

	public ResizeableArray(int capacity = ResizeableArray.DefaultCapacity)
	{
		items = capacity switch
		{
			< 0 => throw ThrowHelper.CapacityCannotBeNegativeException(capacity),
			0 => Array.Empty<T>(),
			_ => new T[capacity],
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

	public void Clear()
	{
		if (IsEmpty)
		{
			return;
		}

		RemoveLastAlreadyChecked(Count);
	}

	/// <inheritdoc/>
	public IReadonlyRandomAccessList<T> Copy()
	{
		var copy = new ResizeableArray<T>(Capacity)
		{
			Count = Count,
		};

		for (int i = 0; i < Count; i++)
		{
			copy[i] = this[i];
		}

		return copy;
	}
	
	public T DeleteAt(int index = 0)
	{
		ValidateIndex(index);
		
		var firstItem = items[index];
		version++;
		Count--;
		
		for (int i = index; i < Count; i++)
		{
			items[i] = items[i + 1];
		}

		items[Count] = default!;
		
		return firstItem;
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

	public void InsertAt(T item, int index = 0)
	{
		if (IsFull)
		{
			Grow();
		}

		for (int i = Count; i > index; i--)
		{
			items[i] = items[i - 1];
		}

		items[index] = item;
		version++;
		Count++;
	}
	
	public T RemoveLast()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		Count--;
		var result = items[Count];
		items[Count] = default!; // Don't hold on to references we don't need.
		version++;
			
		return result;
	}

	// Ignores negative values
	public void RemoveLast(int n)
	{
		if (n <= 0)
		{
			return;
		}
		
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		RemoveLastAlreadyChecked(n > Count ? Count : n);
	}

	public override string ToString() => this.Pretty();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void Grow()
	{
		switch (Capacity)
		{
			case ResizeableArray.MaxCapacity:
				ThrowHelper.ThrowTheContainerIsAtMaximumCapacity();
				break;
			
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

	private void RemoveLastAlreadyChecked(int n)
	{
		Assert(n > 0);
		Assert(n <= Count);
		
		for (int i = Count - n; i < Count; i++)
		{
			items[i] = default!; // Don't hold on to references we don't need.
		}

		Count -= n;
		version++;
	}

	private void ValidateIndex(int index, [CallerArgumentExpression("index")] string? indexArgName = null)
	{
		if (index < 0 || index >= Count)
		{
			throw new ArgumentOutOfRangeException(indexArgName);
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
