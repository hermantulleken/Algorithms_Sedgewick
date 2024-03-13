using AlgorithmsSW.Counter;
using AlgorithmsSW.List;

namespace AlgorithmsSW.SymbolTable;

public static class SymbolTableAlgorithms
{
	private class ReadOnlyListAsTable<T> : IReadOnlySymbolTable<int, T>
	{
		private readonly IReadonlyRandomAccessList<T> list;

		public int Count => list.Count;
		
		public IComparer<int> Comparer => Comparer<int>.Default;
	
		public IEnumerable<int> Keys { get; }

		public bool TryGetValue(int key, out T value)
		{
			if (key < 0 || key >= Count)
			{
				value = default!;
				return false;
			}
		
			value = list[key];
			return true;
		}

		public ReadOnlyListAsTable(IReadonlyRandomAccessList<T> list)
		{
			this.list = list;
			Keys = Enumerable.Range(0, Count);
		}
	}
	
	private class ResizeableArrayAsSymbolTable<TValue>(IRandomAccessList<TValue> list) : ISymbolTable<int, TValue>
	{
		public int Count => list.Count;
		
		public IComparer<int> Comparer => Comparer<int>.Default;

		public IEnumerable<int> Keys => Enumerable.Range(0, list.Count);

		public bool TryGetValue(int key, out TValue value)
		{
			if (key < 0 || key >= list.Count)
			{
				value = default!;
				return false;
			}
		
			value = list[key];
			return true;
		}

		public void Add(int key, TValue value)
		{
			if (key < 0 || key >= list.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(key));
			}
		
			list[key] = value;
		}
		
		/// <exception cref="NotImplementedException">this method is called.</exception>
		public void RemoveKey(int key)
		{
			throw new NotImplementedException();
		}
	}
	
	public static Counter<TValue> CountKeysWithValue<TKey, TValue>(this IReadOnlySymbolTable<TKey, TValue> symbols, IComparer<TValue> comparer)
	{
		var counter = new Counter<TValue>(comparer);

		foreach (var key in symbols.Keys)
		{
			var value = symbols[key];
			counter.Add(value);
		}

		return counter;
	}

	public static IReadOnlySymbolTable<int, T> ToSymbolTable<T>(this IReadonlyRandomAccessList<T> list) 
		=> new ReadOnlyListAsTable<T>(list);
	 
	public static ISymbolTable<int, T> ToSymbolTable<T>(this IRandomAccessList<T> list) 
		=> new ResizeableArrayAsSymbolTable<T>(list);
	
	public static ISymbolTable<TValue, TKey> Invert<TKey, TValue>(this IReadOnlySymbolTable<TKey, TValue> symbols, IComparer<TValue> comparer)
	{
		var table = DataStructures.HashTable<TValue, TKey>(symbols.Count, comparer);

		foreach (var key in symbols.Keys)
		{
			if (table.ContainsKey(symbols[key]))
			{
				throw new InvalidOperationException("Duplicate  values in the symbol table.");
			}
			
			var value = symbols[key];
			table[value] = key;
		}

		return table;
	}

	public static ISymbolTable<TValue, int> Invert<TValue>(
		this IReadonlyRandomAccessList<TValue> list, 
		IComparer<TValue> comparer) 
		=> list.ToSymbolTable().Invert(comparer);

	public static IReadOnlySymbolTable<TKey, TValue> ToSymbolTable<TKey, TValue>(this IEnumerable<(TKey key, TValue value)> pairs, IComparer<TKey> comparer)
	{
		var table = DataStructures.HashTable<TKey, TValue>(comparer);

		foreach (var (key, value) in pairs)
		{
			table[key] = value;
		}

		return table;
	}
}
