using System.Diagnostics.CodeAnalysis;
using Algorithms_Sedgewick.SymbolTable;

namespace Algorithms_Sedgewick.HashTable;

public class SystemDictionary<TKey, TValue> : ISymbolTable<TKey, TValue> 
	where TKey : notnull
{
	private readonly Dictionary<TKey, TValue> dictionary;

	public class EqualityComparer : IEqualityComparer<TKey>
	{
		private readonly IComparer<TKey> comparer;

		public EqualityComparer(IComparer<TKey> comparer)
		{
			this.comparer = comparer;
		}

		public bool Equals(TKey? x, TKey? y) => comparer.Compare(x, y) == 0;

		public int GetHashCode(TKey obj) => obj.GetHashCode();
	}

	public SystemDictionary(IComparer<TKey> comparer)
	{
		dictionary = new Dictionary<TKey, TValue>(new EqualityComparer(comparer));
	}

	public int Count => dictionary.Count;

	public IEnumerable<TKey> Keys => dictionary.Keys;

	public void Add(TKey key, TValue value)
	{
		dictionary[key] = value;
	}

	public void RemoveKey(TKey key)
	{
		if (!dictionary.Remove(key))
		{
			ThrowHelper.ThrowKeyNotFound(key);
		}
	}

	public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) 
		=> dictionary.TryGetValue(key, out value);
}
