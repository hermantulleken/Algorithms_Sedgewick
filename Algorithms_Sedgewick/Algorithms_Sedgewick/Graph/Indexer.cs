using Algorithms_Sedgewick.HashTable;
using Algorithms_Sedgewick.SymbolTable;

namespace Algorithms_Sedgewick.Graphs;

/// <summary>
/// Class that can assign indexes to symbols.
/// </summary>
/// <typeparam name="T">The type of symbols to index.</typeparam>
public class Indexer<T>(IComparer<T> comparer)
{
	private readonly ISymbolTable<T, int> symbolToIndex = new HashTableWithLinearProbing<T, int>(comparer);
	private int nextIndex = 0;
	
	/// <summary>
	/// Gets an <see cref="IReadOnlySymbolTable{TKey,TValue}"/> containing the symbols and their associated indexes.
	/// </summary>
	public IReadOnlySymbolTable<T, int> Symbols => symbolToIndex;

	/// <summary>
	/// Returns an index associated with this index. This index is unique for each symbol.
	/// </summary>
	/// <param name="symbol">The symbol to get the index for.</param>
	/// <returns>The index associated with this symbol.</returns>
	public int GetIndex(T symbol)
	{
		if (symbolToIndex.ContainsKey(symbol))
		{
			return symbolToIndex[symbol];
		}
		
		int index = nextIndex;
		nextIndex++;
		symbolToIndex[symbol] = index;
		return index;
	}
}
