namespace Algorithms_Sedgewick.Set;

public interface ISet<T> : IEnumerable<T>
{
	public void Add(T item);
	
	public bool Contains(T item);
	
	public bool Remove(T item);
}
