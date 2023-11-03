namespace Algorithms_Sedgewick.Buffer;

using System.Collections;

/// <summary>
/// Represents a buffer with a fixed capacity of two items.
/// </summary>
/// <typeparam name="T">The type of items contained in the buffer.</typeparam>
public sealed class Capacity2Buffer<T> : IBuffer<T?>, IPair<T>
{
	private bool nextInsertIsItem1;
	private T? item1;
	private T? item2;

	/// <inheritdoc/>
	public int Capacity => 2;

	/// <inheritdoc/>
	public int Count { get; private set; }

	/// <summary>
	/// Gets the first item in this <see cref="Capacity2Buffer{T}"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">the container is empty.</exception>
	public T? First 
		=> (Count == 0) ? throw ThrowHelper.ContainerEmptyException : FirstUnsafe;

	/// <summary>
	/// Gets the last item in this <see cref="Capacity2Buffer{T}"/>.
	/// </summary>
	/// <exception cref="InvalidOperationException">the container is empty.</exception>
	public T? Last 
		=> (Count == 0) ? throw ThrowHelper.ContainerEmptyException : LastUnsafe;

	public bool HasValue => Count > 0;
	
	public bool HasPreviousValue => Count > 1;
	
	private T? FirstUnsafe 
		=> Count == 1 
			? item2 
			: (nextInsertIsItem1 ? item1 : item2);

	private T? LastUnsafe 
		=> nextInsertIsItem1 ? item2 : item1;

	/// <summary>
	/// Initializes a new instance of the <see cref="Capacity2Buffer{T}"/> class.
	/// </summary>
	public Capacity2Buffer() => Clear();

	public void Clear()
	{
		Count = 0;
		item1 = default;
		item2 = default;
		nextInsertIsItem1 = false; // true after first insertion
	}

	/// <inheritdoc/>
	public IEnumerator<T?> GetEnumerator()
	{
		if (Count > 0)
		{
			yield return FirstUnsafe;
		}

		if (Count > 1)
		{
			yield return LastUnsafe;
		}
	}

	/// <inheritdoc/>
	public void Insert(T? item)
	{
		if (nextInsertIsItem1)
		{
			item1 = item;
		}
		else
		{
			item2 = item;
		}
		
		if (Count < 2)
		{
			Count++;
		}
		
		nextInsertIsItem1 = !nextInsertIsItem1;
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
