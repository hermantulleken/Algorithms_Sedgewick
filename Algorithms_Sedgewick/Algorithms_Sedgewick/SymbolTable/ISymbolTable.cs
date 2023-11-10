using System.Diagnostics.CodeAnalysis;

namespace Algorithms_Sedgewick.SymbolTable;

/// <summary>
/// Defines methods for managing a generic symbol table.
/// </summary>
/// <typeparam name="TKey">The type of keys in the symbol table.</typeparam>
/// <typeparam name="TValue">The type of values in the symbol table.</typeparam>
public interface ISymbolTable<TKey, TValue>
{
	/// <summary>
	/// Gets the number of key/value pairs contained in the symbol table.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Gets a value indicating whether the symbol table is empty.
	/// </summary>
	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets the value associated with the given key. 
	/// </summary>
	/// <param name="key">The key whose value to get.</param>
	/// <exception cref="KeyNotFoundException">the key is not present in the symbol table.</exception>
	TValue this[TKey key] 
	{
		get
		{
			if (TryGetValue(key, out var value))
			{
				return value;
			}

			throw ThrowHelper.KeyNotFoundException(key);
		}

		set => Add(key, value);
	}
	
	/// <summary>
	/// Gets an <see cref="IEnumerable{T}"/> containing the keys of the symbol table.
	/// </summary>
	IEnumerable<TKey> Keys { get; }

	/// <summary>
	/// Adds the specified key and value to the symbol table.
	/// </summary>
	/// <param name="key">The key of the element to add.</param>
	/// <param name="value">The value of the element to add.</param>
	// TODO: Should not allow setting if already set
	public void Add(TKey key, TValue value);

	/// <summary>
	/// Determines whether the symbol table contains a specific key.
	/// </summary>
	/// <param name="key">The key to locate in the symbol table.</param>
	/// <returns><c>true</c> if the symbol table contains an element with the specified key; otherwise, <c>false</c>.</returns>

	bool ContainsKey(TKey key) => TryGetValue(key, out _);

	/// <summary>
	/// Removes the element with the specified key from the symbol table.
	/// </summary>
	/// <param name="key">The key of the element to remove.</param>
	void RemoveKey(TKey key);

	/// <summary>
	/// Tries to get the value associated with the specified key from the symbol table.
	/// </summary>
	/// <param name="key">The key of the value to get.</param>
	/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
	/// <returns><c>true</c> if the symbol table contains an element with the specified key; otherwise, <c>false</c>.</returns>
	bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);

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
