namespace Algorithms_Sedgewick.GapBuffer;

using System.Collections;

using List;

/// <summary>
/// A <see cref="LazyMoveGapBuffer{T}"/> that also provides random access.
/// </summary>
public sealed class RandomAccessLazyMoveGapBuffer<T> : LazyMoveGapBuffer<T>, IRandomAccessList<T>
{
	private readonly IRandomAccessList<T> bufferAsList;

	public T this[int index]
	{
		get => bufferAsList[index];
		set => bufferAsList[index] = value;
	}

	public static RandomAccessLazyMoveGapBuffer<T> Create<TBuffer>(Func<TBuffer> eagerBufferFactory)
		where TBuffer : IGapBuffer<T>, IRandomAccessList<T>
	{
		var eagerBuffer = eagerBufferFactory();
		var bufferAsList = (IRandomAccessList<T>)eagerBuffer; // Safe because of type constraint
	
		return new RandomAccessLazyMoveGapBuffer<T>(eagerBuffer, bufferAsList);
	}
	
	private RandomAccessLazyMoveGapBuffer(IGapBuffer<T> eagerBuffer, IRandomAccessList<T> bufferAsList)
		: base(eagerBuffer)
	{
		this.bufferAsList = bufferAsList;
	}
	
	public IEnumerator<T> GetEnumerator() => bufferAsList.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
