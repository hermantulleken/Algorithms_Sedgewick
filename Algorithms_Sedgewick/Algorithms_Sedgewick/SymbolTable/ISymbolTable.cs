namespace Algorithms_Sedgewick.SymbolTable;

/*
	Note: This is almost what C# calls a dictionary. 
	I stick with the book name, to avoid getting confused with C# dictionaries. 
*/
public interface ISymbolTable<TKey, TValue>
{
	int Count { get; }
	
	public bool IsEmpty => Count == 0;
	
	TValue this[TKey key] { get; set; }
	
	IEnumerable<TKey> Keys { get; }
	
	bool ContainsKey(TKey key);
	
	void RemoveKey(TKey key);
}
