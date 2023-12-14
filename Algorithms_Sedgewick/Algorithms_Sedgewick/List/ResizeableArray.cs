namespace Algorithms_Sedgewick.List;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Support;
using static System.Diagnostics.Debug;

/// <summary>
/// Contains constants for the <see cref="ResizeableArray{T}"/> class.
/// </summary>
public static class ResizeableArray
{
	internal const int DefaultCapacity = 16;
	internal const int HalfMaxCapacity = MaxCapacity >> 1;
	internal const int MaxCapacity = int.MaxValue;
	
	internal static class Builder
	{
		/*
			This is used so we can use collection expressions. See the attributes of ResizeableArray<T>.
		*/
		internal static ResizeableArray<T> Create<T>(ReadOnlySpan<T> values) => new(values);
	}
}

/// <summary>
/// Represents a generic list of items.
/// </summary>
/// <param name="capacity">The initial capacity of the list.</param>
/// <typeparam name="T">The type of items contained in the list.</typeparam>
/// <exception cref="ArgumentException"><paramref name="capacity"/> is negative.</exception>
[SuppressMessage(
	"StyleCop.CSharp.MaintainabilityRules", 
	"SA1402:File may only contain a single type", 
	Justification = "Generic and Simple version goes together.")]
[CollectionBuilder(typeof(ResizeableArray.Builder), nameof(ResizeableArray.Builder.Create))]
public sealed class ResizeableArray<T>(int capacity = ResizeableArray.DefaultCapacity)
	: IRandomAccessList<T>
{
	/*
		This array may have null elements when
			1. this class is below capacity and T is a reference type, 
				so the last values are all null. These nulls are never 
				exposed to the caller.
			2. T is nullable, and so null values could have been inserted.  
	*/
	private T[] items = capacity switch
	{
		< 0 => throw ThrowHelper.CapacityCannotBeNegativeException(capacity),
		0 => Array.Empty<T>(),
		_ => new T[capacity],
	};

	private int version = 0;
	
	/// <summary>
	/// Gets the capacity for this instance, that is the maximum number of items it can hold without resizing.
	/// </summary>
	public int Capacity { get; private set; } = capacity;

	/// <inheritdoc />
	public int Count { get; private set; }

	/// <inheritdoc/>
	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets a value indicating whether the <see cref="ResizeableArray{T}"/> is full, that is <see cref="Count"/> equals <see cref="Capacity"/>.
	/// </summary>
	public bool IsFull => Count == Capacity;

	public static ResizeableArray<int> Empty = [];

	/// <summary>
	/// Gets or sets the item at the specified index.
	/// </summary>
	/// <param name="index">The index of the item to get or set.</param>
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

	internal ResizeableArray(ReadOnlySpan<T> elements)
		: this(elements.Length)
	{
		AddRange(elements);
	}

	/// <summary>
	/// Adds an item to the end of the <see cref="ResizeableArray{T}"/>.
	/// </summary>
	/// <pqram name="item">The item to add to the <see cref="ResizeableArray{T}"/>.</pqram>
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

	public bool Remove(T item, IEqualityComparer<T> comparer)
	{
		for (int i = 0; i < Count; i++)
		{
			if (comparer.Equals(items[i], item))
			{
				RemoveAt(i);
				return true;
			}
		}

		return false;
	}
	
	public bool Remove(T item)
	{
		for (int i = 0; i < Count; i++)
		{
			if (Equals(items[i], item))
			{
				RemoveAt(i);
				return true;
			}
		}

		return false;
	}


	/// <summary>
	/// Sets the <see cref="Count"/> to the specified value, growing the <see cref="ResizeableArray{T}"/> if necessary.
	/// </summary>
	/// <param name="newCount">The new value for <see cref="Count"/>.</param>
	/*
		Why not put this in Count's setter?
		Because Count is a property, that is supposed to execute quickly, but setting the size in this way could
		trigger a UpdateCapacity, which is not a quick operation.
	*/
	public void SetCount(int newCount)
	{
		Assert(newCount < ResizeableArray.MaxCapacity);
		
		if (newCount > capacity)
		{
			/*
				I am not sure if this should not rather be newCount * 2 to avoid further resizes for some time. 
				It would be nice if this was exactly the same as adding individual items to the list, but that would be
				too tricky. 
			*/
			UpdateCapacity(newCount);
		}

		if (newCount < Count)
		{
			RemoveLastAlreadyChecked(Count - newCount);
		}
		else
		{
			Count = newCount;
			version++;
		}
	}

	/// <summary>
	/// Removes all items from the <see cref="ResizeableArray{T}"/>.
	/// </summary>
	public void Clear()
	{
		if (IsEmpty)
		{
			return;
		}

		RemoveLastAlreadyChecked(Count);
	}

	/// <summary>
	/// Deletes the item at the specified index.
	/// </summary>
	/// <param name="index">The index of the item to delete.</param>
	/// <returns>The deleted item.</returns>
	public T RemoveAt(int index = 0)
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

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator()
	{
		int versionAtStartOfIteration = version;
		
		for (int i = 0; i < Count; i++)
		{
			ValidateVersion(versionAtStartOfIteration);
			
			yield return items[i];
		}
	}

	/// <summary>
	/// Inserts an item at the beginning of the <see cref="ResizeableArray{T}"/>.
	/// </summary>
	/// <param name="item">The item to insert.</param>
	/// <param name="index">The index at which to insert the item.</param>
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
	
	/// <summary>
	/// Removes the last item from the <see cref="ResizeableArray{T}"/>.
	/// </summary>
	/// <returns> The removed item. </returns>
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

	/// <summary>
	/// Removes the last <paramref name="n"/> items from the <see cref="ResizeableArray{T}"/>.
	/// </summary>
	/// <param name="n">The number of items to remove.</param>
	public void RemoveLast(int n) // Ignores negative values
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

	/// <inheritdoc />
	public override string ToString() => this.Pretty();
	
	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void AddRange(ReadOnlySpan<T> elements)
	{
		for (int i = 0; i < elements.Length; i++)
		{
			items[i] = elements[i];
		}

		Count = elements.Length;
	}
	
	private void Grow()
	{
		int newCapacity = Capacity switch
		{
			ResizeableArray.MaxCapacity => throw ThrowHelper.ContainerIsAtMaximumCapacityException,
			< ResizeableArray.HalfMaxCapacity => 2 * Capacity,
			_ => ResizeableArray.MaxCapacity,
		};
		
		UpdateCapacity(newCapacity);
	}

	private void UpdateCapacity(int newCapacity)
	{
		Capacity = newCapacity;
		var newItems = new T[Capacity];
		Array.Copy(items, newItems, Count);
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
