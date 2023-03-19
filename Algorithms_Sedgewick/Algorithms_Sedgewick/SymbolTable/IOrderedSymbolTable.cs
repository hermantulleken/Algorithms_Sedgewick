using System.ComponentModel.Design;
using global::System.Collections.Generic;

namespace Algorithms_Sedgewick.SymbolTable;

/*	Note: This is almost what C# calls a dictionary. 
	However, there are additional operations not usually suported by dictionaries, such as 
	min and floor.

	I stick with the book name, to avoid getting confused with C# dictionaries. 
	
	p. 366
*/
public interface ISymbolTable<TKey, TValue>
{
	public bool IsEmpty => Count == 0;
	int Count { get; }

	IEnumerable<TKey> Keys { get; }

	TValue this[TKey key] { get; set; }

	void RemoveKey(TKey key);
	bool ContainsKey(TKey key);
	TKey MinKey();
	TKey MaxKey();
	
	//The book uses Floor
	TKey LargestKeyLessThanOrEqualTo(TKey key);
	
	//The book uses Ceil
	TKey SmallestKeyGreaterThanOrEqualTo(TKey key);

	int RankOf(TKey key);

	// Select
	TKey KeyWithRank(int rank);

	int CountRange(TKey start, TKey end);

	IEnumerable<TKey> KeysRange(TKey start, TKey end);
}
