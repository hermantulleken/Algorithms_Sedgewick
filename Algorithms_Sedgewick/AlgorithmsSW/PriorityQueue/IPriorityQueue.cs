namespace AlgorithmsSW.PriorityQueue;

public interface IPriorityQueue<T> : IEnumerable<T>
{
	public int Count { get; }

	public T PeekMin { get; }

	public T PopMin();

	public void Push(T item);
}
