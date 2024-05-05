namespace AlgorithmsSW.String;

using System.Collections;
using Set;

[ExerciseReference(5, 2, 6)]
public class StringSetWithTertiarySearchTree : ISet<string>
{
	private static readonly object Sentinel = new();
	
	private readonly IStringSymbolTable<object> trie = new TertiarySearchTrie<object>();

	/// <inheritdoc/>
	public IEnumerator<string> GetEnumerator() => trie.Keys.GetEnumerator();

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	public IComparer<string> Comparer => trie.Comparer;

	/// <inheritdoc/>
	public void Add(string item) => trie[item] = Sentinel;

	/// <inheritdoc/>
	public bool Contains(string item) => trie.ContainsKey(item);

	/// <inheritdoc/>
	public bool Remove(string item)
	{
		if (!Contains(item))
		{
			return false;
		}
		
		trie.RemoveKey(item);
		
		return true;
	}

	/// <inheritdoc/>
	public int Count => trie.Count;
}
