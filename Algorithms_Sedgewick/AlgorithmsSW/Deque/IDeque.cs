namespace AlgorithmsSW.Deque;


public interface IDeque<T> : IEnumerable<T>
{
	int Count { get; }
	
	T PeekLeft { get; }
	
	T PeekRight { get; }
	
	void Clear();
	
	T PopLeft();
	
	T PopRight();
	
	void PushLeft(T item);
	
	void PushRight(T item);
}
