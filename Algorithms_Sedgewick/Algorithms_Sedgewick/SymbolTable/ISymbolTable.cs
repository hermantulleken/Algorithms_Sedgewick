using System.Diagnostics.CodeAnalysis;

namespace Algorithms_Sedgewick.SymbolTable;

/*
	Note: This is almost what C# calls a dictionary. 
	I stick with the name used in the reference, to avoid getting confused with C# dictionaries. 
*/
public interface ISymbolTable<TKey, TValue>
{
	int Count { get; }

	public bool IsEmpty => Count == 0;

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

	IEnumerable<TKey> Keys { get; }

	public void Add(TKey key, TValue value);

	bool ContainsKey(TKey key) => TryGetValue(key, out _);

	void RemoveKey(TKey key);

	bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
}
