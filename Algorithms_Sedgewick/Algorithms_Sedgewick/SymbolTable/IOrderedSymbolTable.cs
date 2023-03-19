using System.Collections.Generic;

namespace Algorithms_Sedgewick.SymbolTable;

//	p. 366
public interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	TKey MinKey();
	TKey MaxKey();
	TKey LargestKeyLessThanOrEqualTo(TKey key); //The book uses Floor
	TKey SmallestKeyGreaterThanOrEqualTo(TKey key); //The book uses Ceil
	int RankOf(TKey key);
	TKey KeyWithRank(int rank); // Select
	int CountRange(TKey start, TKey end);
	IEnumerable<TKey> KeysRange(TKey start, TKey end);
}
