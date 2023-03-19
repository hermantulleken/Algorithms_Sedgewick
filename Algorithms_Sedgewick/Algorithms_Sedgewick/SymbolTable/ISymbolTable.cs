using System.Collections.Generic;

namespace Algorithms_Sedgewick.SymbolTable;

public interface ISymbolTable<TKey, TValue>
{
	public bool IsEmpty => Count == 0;
	int Count { get; }
	IEnumerable<TKey> Keys { get; }
	TValue this[TKey key] { get; set; }
	void RemoveKey(TKey key);
	bool ContainsKey(TKey key);
}