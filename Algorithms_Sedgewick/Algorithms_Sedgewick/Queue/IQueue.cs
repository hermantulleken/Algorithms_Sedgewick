namespace Algorithms_Sedgewick;

public interface IQueue<T> : IEnumerable<T>
{
	public int Count { get; }
	public bool IsEmpty => Count == 0;
	public T Peek { get; }

	public void Enqueue(T item);
	public T Dequeue();

	public void Clear();
}
