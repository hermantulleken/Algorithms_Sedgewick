namespace AlgorithmsSW.Set;

public interface ISet<T> : IEnumerable<T>
{
	IComparer<T> Comparer { get; }
	
	public void Add(T item);
	
	public bool Contains(T item);
	
	public bool Remove(T item);

	int Count { get; }
}
