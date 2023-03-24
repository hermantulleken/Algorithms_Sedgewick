namespace Algorithms_Sedgewick.Buffer;

public interface IBuffer<T> : IEnumerable<T>
{
	public int Capacity { get; }
	public int Count { get; }

	public T First { get; }

	public T Last { get; }

	public void Clear();

	public void Insert(T item);
}
