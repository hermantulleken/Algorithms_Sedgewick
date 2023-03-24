namespace Algorithms_Sedgewick.Queue;

public interface IQueue<T> : IEnumerable<T>
{
	public int Count { get; }
	public bool IsEmpty => Count == 0;

	public bool IsSingleton => Count == 1;
	public T Peek { get; }

	public void Clear();
	public T Dequeue();

	public void Enqueue(T item);
}
