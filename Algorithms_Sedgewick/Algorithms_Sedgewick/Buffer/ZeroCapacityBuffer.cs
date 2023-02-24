using System.Collections;

namespace Algorithms_Sedgewick.Buffer;

public sealed class ZeroCapacityBuffer<T> : IBuffer<T>
{
	public int Count => 0;
	public int Capacity => 0;
	public T First => throw Buffer.EmptyBufferInvalid();
	public T Last  => throw Buffer.EmptyBufferInvalid();
    
	/// <summary>
	/// This method has no effect, since the capacity is 0.
	/// </summary>
	/*
	    Just like other buffers, it is not illegal to insert items 
	    when we are at capacity. So we don't throw an exception; we 
	    simply do nothing.
	*/
	public void Insert(T item) { } 
	//Nothing to do since there are no elements.
	public void Clear() { }
    
	public IEnumerator<T> GetEnumerator() 
		=> throw new NotImplementedException();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
