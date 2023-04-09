namespace Algorithms_Sedgewick.Counter;

using HashTable;
using SymbolTable;

public class Counter<T> : ICounter<T>
{
	private readonly ISymbolTable<T, int> counts;

	public Counter(IComparer<T> comparer)
	{
		counts = new HashTableWithLinearProbing2<T, int>(comparer);
	}

	public IEnumerable<T> Keys => counts.Keys;

	public int this[T item] => counts[item];

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
}
