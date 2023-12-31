namespace AlgorithmsSW.Deque;

/// <summary>
/// A double-ended queue.
/// </summary>
/// <typeparam name="T">The type of elements in the deque.</typeparam>
public interface IDeque<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the number of elements in the deque.
	/// </summary>
	int Count { get; }
	
	T PeekLeft { get; }
	
	T PeekRight { get; }
	
	void Clear();
	
	T PopLeft();
	
	T PopRight();
	
	void PushLeft(T item);
	
	void PushRight(T item);
}
