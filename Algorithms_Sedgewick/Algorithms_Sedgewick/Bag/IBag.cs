namespace Algorithms_Sedgewick.Bag;

public interface IBag<T> : IEnumerable<T>
{
	public int Count { get; }
	public bool IsEmpty { get; }
	public void Add(T item);
}