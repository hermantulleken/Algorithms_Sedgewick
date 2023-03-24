namespace Algorithms_Sedgewick.Buffer;

using System.Collections;
using System.Collections.Generic;

public sealed class ZeroCapacityBuffer<T> : IBuffer<T>
{
	public int Capacity => 0;
	public int Count => 0;

	public T First => throw ThrowHelper.ContainerEmptyException;

	public T Last => throw ThrowHelper.ContainerEmptyException;

	public void Clear()
	{
		// Nothing to do since there are no elements.
	}

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
