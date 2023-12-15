namespace AlgorithmsSW.Set;

using System.Collections;
using List;

public class SetWithArray<T> : ISet<T>
{
	private readonly ResizeableArray<T> items;
	
	private readonly IEqualityComparer<T> comparer;

	public SetWithArray(int initialCapacity, IEqualityComparer<T> comparer)
	{
		items = new ResizeableArray<T>(initialCapacity);
		this.comparer = comparer;
	}

	public void Add(T item)
	{
		if (!items.Contains(comparer, item))
		{
			items.Add(item);
		}
	}
	
	public bool Remove(T item) => items.Remove(item, comparer);
	
	public int Count => items.Count;

	public bool Contains(T item) => items.Contains(comparer, item);
	
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
