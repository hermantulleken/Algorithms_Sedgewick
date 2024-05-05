namespace AlgorithmsSW.String;

/// <summary>
/// Provides operations for a collection of strings (often, the keys of a symbol table).
/// </summary>
public interface IStringCollection 
{
	/// <summary>
	/// Returns the longest string that is a prefix of the given string.
	/// </summary>
	/// <returns>The longest string that is a prefix of the given string;
	/// <see langword="null"/> if no keys are a prefix of the given string.
	/// </returns>
	public string? LongestPrefixOf(string str);
	
	/// <summary>
	/// Returns all keys in the symbol table that have the given prefix.
	/// </summary>
	/// <param name="prefix">The prefix used to search for keys.</param>
	public IEnumerable<string> KeysWithPrefix(string prefix);
	
	/// <summary>
	/// Returns all keys in the symbol table that match the given pattern.
	/// </summary>
	/// <param name="pattern">The pattern used to search for keys.</param>
	public IEnumerable<string> KeysThatMatch(string pattern);
	
	/// <summary>
	/// Returns <see langword="true"/> if the symbol table supports empty keys;
	/// otherwise, <see langword="false"/>.
	/// </summary>
	public bool SupportsEmptyKeys { get; }
}
