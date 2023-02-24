namespace Algorithms_Sedgewick;

public interface IBag<T> : IEnumerable<T>
{
	public int Count { get; }
	public bool IsEmpty { get; }
	public void Add(T item);
}