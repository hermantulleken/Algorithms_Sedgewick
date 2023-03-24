namespace Algorithms_Sedgewick.PriorityQueue;

public interface IPriorityQueue<T>
    where T : IComparable<T>
{
	public int Count { get; }

	public T PeekMin { get; }

	public T PopMin();

	public void Push(T item);
}
