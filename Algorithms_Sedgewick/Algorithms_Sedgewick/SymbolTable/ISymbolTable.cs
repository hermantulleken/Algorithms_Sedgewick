namespace Algorithms_Sedgewick.SymbolTable;

/*
	Note: This is almost what C# calls a dictionary. 
	I stick with the book name, to avoid getting confused with C# dictionaries. 
*/
public interface ISymbolTable<TKey, TValue>
{
	public bool IsEmpty => Count == 0;
	int Count { get; }
	IEnumerable<TKey> Keys { get; }
	TValue this[TKey key] { get; set; }
	void RemoveKey(TKey key);
	bool ContainsKey(TKey key);
}
