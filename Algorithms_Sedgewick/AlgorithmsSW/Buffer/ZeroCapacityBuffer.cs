namespace AlgorithmsSW.Buffer;

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// An <see cref="IBuffer{T}"/> with zero capacity.
/// </summary>
/// <typeparam name="T">The type of element that this buffer could contain.</typeparam>
/// <remarks> This class is useful to use in algorithms where arbitrary capacity is
/// required that includes 0 capacity.
///
/// Only one instance is maintained; get it with <see cref="Instance"/>.
/// </remarks>
public sealed class ZeroCapacityBuffer<T> : IBuffer<T>
{
	/// <summary>
	/// Gets the singleton instance of <see cref="ZeroCapacityBuffer{T}"/>.
	/// </summary>
	public static IBuffer<T> Instance { get; } = new ZeroCapacityBuffer<T>();
	
	/// <inheritdoc />
	public int Capacity => 0;

	/// <inheritdoc />
	public int Count => 0;

	/// <inheritdoc />
	public T First => throw ThrowHelper.ContainerEmptyException;

	/// <inheritdoc />
	public T Last => throw ThrowHelper.ContainerEmptyException;

	private ZeroCapacityBuffer()
	{
	}
	
	/// <inheritdoc />
	public void Clear()
	{
		// Nothing to do since there are no elements.
	}

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() 
		=> throw new NotImplementedException();

	/// <summary>
	/// This method has no effect, since the capacity is 0.
	/// </summary>
	/// <param name="item">The item to insert into the buffer.</param>
	public void Insert(T? item)
	{
		/*
			Just like other buffers, it is not illegal to insert items 
			when we are at capacity. So we don't throw an exception; we 
			simply do nothing.
		*/
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
