﻿using Algorithms_Sedgewick.Counter;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.SymbolTable;

public static class SymbolTableAlgorithms
{
	private class ReadOnlyListAsTable<T> : IReadOnlySymbolTable<int, T>
	{
		private readonly IReadonlyRandomAccessList<T> list;

		public int Count => list.Count;
	
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

	public static IReadOnlySymbolTable<int, T> ToSymbolTable<T>(this IReadonlyRandomAccessList<T> list) => new ReadOnlyListAsTable<T>(list);
}