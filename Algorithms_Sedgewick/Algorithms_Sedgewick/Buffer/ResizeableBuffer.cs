namespace Algorithms_Sedgewick.Buffer;

using System.Collections;

public class ResizeableBuffer<T> : IBuffer<T?>
{
	private IBuffer<T?> buffer;

	/// <inheritdoc />
	public int Capacity => buffer.Capacity;

	/// <inheritdoc />
	public int Count => buffer.Count;

	/// <inheritdoc />
	public T? First => buffer.First;

	/// <inheritdoc />
	public T? Last => buffer.Last;

	public ResizeableBuffer(int capacity) 
	
		=> buffer = CreateBuffer(capacity);

	/// <inheritdoc />
	public void Clear() => buffer.Clear();

	/// <inheritdoc />
	public IEnumerator<T?> GetEnumerator() => buffer.GetEnumerator();

	/// <inheritdoc />
	public void Insert(T? item) => buffer.Insert(item);

	public void Resize(int newCapacity)
	{
		switch (newCapacity)
		{
			case < 0:
				throw ThrowHelper.CapacityCannotBeNegativeException(newCapacity);
			case 0:
				buffer = ZeroCapacityBuffer<T?>.Instance;
				return;
		}

		var newBuffer = CreateBuffer(newCapacity);
		
		foreach (var item in buffer.Take(newCapacity))
		{
			newBuffer.Insert(item);
		}
		
		buffer = newBuffer;
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private static IBuffer<T?> CreateBuffer(int capacity) => new RingBuffer<T?>(capacity);
}
