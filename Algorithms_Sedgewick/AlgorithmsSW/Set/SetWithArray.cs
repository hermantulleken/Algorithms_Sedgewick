namespace AlgorithmsSW.Set;

using System.Collections;
using List;

public class SetWithArray<T> : ISet<T>
{
	private readonly ResizeableArray<T> items;

	public IComparer<T> Comparer { get; }

	private readonly IEqualityComparer<T> equalityComparer;

	public SetWithArray(int initialCapacity, IComparer<T> comparer)
	{
		items = new ResizeableArray<T>(initialCapacity);
		Comparer = comparer;
		equalityComparer = comparer.ToEqualityComparer();
	}

	public void Add(T item)
	{
		if (!items.Contains(equalityComparer, item))
		{
			items.Add(item);
		}
	}
	
	public bool Remove(T item) => items.Remove(item, equalityComparer);
	
	public int Count => items.Count;

	public bool Contains(T item) => items.Contains(equalityComparer, item);
	
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
