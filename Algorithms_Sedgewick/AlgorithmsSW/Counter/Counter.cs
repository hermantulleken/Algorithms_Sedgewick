namespace AlgorithmsSW.Counter;

using HashTable;
using SymbolTable;

public class Counter<T>(IComparer<T> comparer) : ICounter<T>
{
	private readonly ISymbolTable<T, int> counts = new HashTableWithLinearProbing2<T, int>(comparer);

	public IEnumerable<T> Keys => counts.Keys;

	public int this[T item] => counts.ContainsKey(item) ? counts[item] : 0;

	public void Add(T item)
	{
		if (!counts.ContainsKey(item))
		{
			counts[item] = 1;
		}
		else
		{
			counts[item]++;
		}
	}

	public void Remove(T item)
	{
		if (!counts.ContainsKey(item))
		{
			counts[item] = -1;
		}
		else
		{
			counts[item]--;
			
			if (counts[item] == 0)
			{
				counts.RemoveKey(item);
			}
		}
	}

	public int GetCount(T item) => counts.ContainsKey(item) ? counts[item] : 0;

	public void Clear() => counts.Clear();

	public void RemoveAll(T item)
	{
		if (counts.ContainsKey(item))
		{
			counts.RemoveKey(item);
		} // else nothing left to do
	}
}
