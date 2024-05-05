namespace AlgorithmsSW.String;

public static partial class StringSymbolsExtensions
{
	/// <summary>
	/// A symbol table with string keys that supports an empty string key.
	/// </summary>
	/// <remarks>
	/// Use <see cref="StringSymbolsExtensions.WithEmptyKeySupport{TValue}"/> to create an instance of this class.
	/// </remarks>
	[ExerciseReference(5, 2, 7)]
	internal class StringSymbolTableThatSupportsEmpty<TValue>(IStringSymbolTable<TValue> stringSymbolTableImplementation)
		: IStringSymbolTable<TValue>
	{
		// Note: We need this in switch statements where we cannot use string.E,pty
		private const string Empty = "";

		private bool hasEmptyKey;

		private TValue? emptyValue;

		/// <inheritdoc/>
		public IComparer<string> Comparer => stringSymbolTableImplementation.Comparer;

		/// <inheritdoc/>
		/// <returns><see langword="true"/>.</returns>
		public bool SupportsEmptyKeys => true;

		/// <inheritdoc/>
		public int Count =>
			hasEmptyKey
				? stringSymbolTableImplementation.Count + 1
				: stringSymbolTableImplementation.Count;

		/// <inheritdoc/>
		public IEnumerable<string> Keys =>
			hasEmptyKey
				? stringSymbolTableImplementation.Keys.Append(string.Empty)
				: stringSymbolTableImplementation.Keys;

		/// <inheritdoc/>
		public bool TryGetValue(string key, out TValue value)
		{
			switch (key)
			{
				case Empty:
					value = emptyValue!;
					return hasEmptyKey;
				default:
					return stringSymbolTableImplementation.TryGetValue(key, out value!);
			}
		}

		/// <inheritdoc/>
		public void Add(string key, TValue value)
		{
			switch (key)
			{
				case Empty:
					emptyValue = value;
					hasEmptyKey = true;
					break;
				default:
					stringSymbolTableImplementation.Add(key, value);
					break;
			}
		}

		/// <inheritdoc/>
		public void RemoveKey(string key)
		{
			switch (key)
			{
				case Empty:
					hasEmptyKey = false;
					emptyValue = default;
					return;
				default:
					stringSymbolTableImplementation.RemoveKey(key);
					break;
			}
		}

		/// <inheritdoc/>
		public string? LongestPrefixOf(string str)
		{
			string? prefix = stringSymbolTableImplementation.LongestPrefixOf(str);

			return prefix == null && hasEmptyKey ? Empty : prefix;
		}

		/// <inheritdoc/>
		public IEnumerable<string> KeysWithPrefix(string prefix)
		{
			return hasEmptyKey
				? stringSymbolTableImplementation.KeysWithPrefix(prefix).Append(Empty)
				: stringSymbolTableImplementation.KeysWithPrefix(prefix);
		}

		/// <inheritdoc/>
		public IEnumerable<string> KeysThatMatch(string pattern)
		{
			return pattern != Empty
				? stringSymbolTableImplementation.KeysThatMatch(pattern)
				: hasEmptyKey
					? [Empty]
					: [];
		}
	}
}
