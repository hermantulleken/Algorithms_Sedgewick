namespace Algorithms_Sedgewick.SymbolTable;

// P. 366
public interface IOrderedSymbolTable<TKey, TValue> : ISymbolTable<TKey, TValue>
{
	int CountRange(TKey start, TKey end);

	IEnumerable<TKey> KeysRange(TKey start, TKey end);

	TKey KeyWithRank(int rank); // Select

	TKey LargestKeyLessThanOrEqualTo(TKey key); // The book uses Floor

	TKey MaxKey();
	TKey MinKey();

	int RankOf(TKey key);

	TKey SmallestKeyGreaterThanOrEqualTo(TKey key); // The book uses Ceil
}
