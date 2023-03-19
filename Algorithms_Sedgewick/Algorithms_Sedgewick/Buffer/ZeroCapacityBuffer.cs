using System.Collections;
using global::System.Collections.Generic;

namespace Algorithms_Sedgewick.Buffer;

public sealed class ZeroCapacityBuffer<T> : IBuffer<T>
{
	public int Count => 0;
	public int Capacity => 0;
	public T First => throw ThrowHelper.ContainerEmptyException;
	public T Last  => throw ThrowHelper.ContainerEmptyException;

	/// <summary>
	/// This method has no effect, since the capacity is 0.
	/// </summary>
	public void Insert(T item)
	{
		/*
		    Just like other buffers, it is not illegal to insert items 
		    when we are at capacity. So we don't throw an exception; we 
		    simply do nothing.
		*/
	}

	public void Clear()
	{
		//Nothing to do since there are no elements.
	}
    
	public IEnumerator<T> GetEnumerator() 
		=> throw new NotImplementedException();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
