namespace Algorithms_Sedgewick.SymbolTable;

/// <summary>
/// Defines methods for managing a generic symbol table.
/// </summary>
/// <typeparam name="TKey">The type of keys in the symbol table.</typeparam>
/// <typeparam name="TValue">The type of values in the symbol table.</typeparam>
public interface ISymbolTable<TKey, TValue> : IReadOnlySymbolTable<TKey, TValue>
{
	/// <summary>
	/// Gets and sets the value associated with the given key. 
	/// </summary>
	/// <param name="key">The key whose value to get.</param>
	/// <exception cref="KeyNotFoundException">the key is not present in the symbol table.</exception>
	new TValue this[TKey key]
	{
		get => ((IReadOnlySymbolTable<TKey, TValue>)this)[key];
		set => Add(key, value);
	}

	/// <summary>
	/// Adds the specified key and value to the symbol table.
	/// </summary>
	/// <param name="key">The key of the element to add.</param>
	/// <param name="value">The value of the element to add.</param>
	// TODO: Should not allow setting if already set
	public void Add(TKey key, TValue value);
	
	/// <summary>
	/// Removes the element with the specified key from the symbol table.
	/// </summary>
	/// <param name="key">The key of the element to remove.</param>
	void RemoveKey(TKey key);

	/// <summary>
	/// Removes all keys and values from the symbol table.
	/// </summary>
	public void Clear()
	{
		foreach (var key in Keys)
		{
			RemoveKey(key);
		}
	}
}
