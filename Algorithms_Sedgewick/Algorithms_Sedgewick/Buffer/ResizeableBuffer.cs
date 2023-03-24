namespace Algorithms_Sedgewick.Buffer;

using System.Collections;

public class ResizeableBuffer<T> : IBuffer<T?>
{
	private IBuffer<T?> buffer;
	
	public int Count => buffer.Count;
	
	public int Capacity => buffer.Capacity;
	
	public T? First => buffer.First;
	
	public T? Last => buffer.Last;
	
	public ResizeableBuffer(int capacity) 
	
		=> buffer = CreateBuffer(capacity);
	
	public void Insert(T? item) => buffer.Insert(item);
	
	public void Clear() => buffer.Clear();
	
	public IEnumerator<T?> GetEnumerator() => buffer.GetEnumerator();
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
	public void Resize(int newCapacity)
	{
		switch (newCapacity)
		{
			case < 0:
				throw ThrowHelper.CapacityCannotBeNegativeException(newCapacity);
			case 0:
				buffer = new ZeroCapacityBuffer<T?>();
				return;
		}

		var newBuffer = CreateBuffer(newCapacity);
        
		foreach (var item in buffer.Take(newCapacity))
		{
			newBuffer.Insert(item);
		}
		
		buffer = newBuffer;
	}

	private static IBuffer<T?> CreateBuffer(int capacity) => new RingBuffer<T?>(capacity);
}
