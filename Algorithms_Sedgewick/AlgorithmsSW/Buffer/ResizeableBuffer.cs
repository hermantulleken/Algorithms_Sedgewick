namespace AlgorithmsSW.Buffer;

using System.Collections;

/// <summary>
/// A buffer whose capacity can be changed.
/// </summary>
/// <typeparam name="T">The type of elements in the buffer.</typeparam>
/// <param name="capacity">The number of elements the buffer can retain.</param>
public class ResizeableBuffer<T>(int capacity) : IBuffer<T?>
{
	private IBuffer<T?> buffer = CreateBuffer(capacity);

	/// <inheritdoc />
	public int Capacity => buffer.Capacity;

	/// <inheritdoc />
	public int Count => buffer.Count;

	/// <inheritdoc />
	public T? First => buffer.First;

	/// <inheritdoc />
	public T? Last => buffer.Last;

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

	private static RingBuffer<T?> CreateBuffer(int capacity) => new RingBuffer<T?>(capacity);
}
