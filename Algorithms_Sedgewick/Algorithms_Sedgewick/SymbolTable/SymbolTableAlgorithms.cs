using Algorithms_Sedgewick.Counter;

namespace Algorithms_Sedgewick.SymbolTable;

public class SymbolTableAlgorithm
{
	public Counter<TValue> CountKeysWithValue<TKey, TValue>(IReadOnlySymbolTable<TKey, TValue> symbols, IComparer<TValue> comparer)
	{
		var counter = new Counter<TValue>(comparer);

		foreach (var key in symbols.Keys)
		{
			var value = symbols[key];
			counter.Add(value);
		}

		return counter;
	}

}
