namespace AlgorithmsSW.PriorityQueue;

[PageReference(309)] // I did do min instead of max though, as I felt it more consistent with the sorting. 
public interface IPriorityQueue<T> : IEnumerable<T>
{
	public int Count { get; }

	public T PeekMin { get; }

	public T PopMin();

	public void Push(T item);
}
