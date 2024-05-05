namespace AlgorithmsSW.String;

public static partial class StringSymbolsExtensions
{
	/// <summary>
	/// Returns a symbol table with string keys that supports an empty string key.
	/// </summary>
	/// <param name="stringSymbolTable">The underlying symbol table to use. This
	/// need not support empty strings as keys.</param>
	public static IStringSymbolTable<TValue> WithEmptyKeySupport<TValue>(
		this IStringSymbolTable<TValue> stringSymbolTable)
	{
		return new StringSymbolTableThatSupportsEmpty<TValue>(stringSymbolTable);
	}
}
