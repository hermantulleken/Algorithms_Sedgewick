namespace UnitTests.Strings;

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlgorithmsSW;
using AlgorithmsSW.String;
using Support;

[TestFixture]
public class TrieTests
{
	[Test]
	public void StaticConstructor_InvalidType_ThrowsException()
	{
		var exception = Assert.Throws<TypeInitializationException>(() => _ = new Trie<int>())!;
		Assert.That(exception.InnerException, Is.TypeOf<TypeArgumentException<int>>());
	}
	
	protected static readonly ImplementationFactory<IStringSymbolTable<int?>> Types =
	[
		() => new Trie<int?>(),
		() => new TertiarySearchTrie<int?>(),
		() => (StringSymbolsExtensions.StringSymbolTableThatSupportsEmpty<int?>)new Trie<int?>().WithEmptyKeySupport()
	];
}

[TestFixture(typeof(Trie<int?>))]
[TestFixture(typeof(TertiarySearchTrie<int?>))]
[TestFixture(typeof(StringSymbolsExtensions.StringSymbolTableThatSupportsEmpty<int?>))]
public class TrieTests<T> : TrieTests
	where T : IStringSymbolTable<int?>
{
	[Test]
	public void Add_AddsValueToTrie()
	{
		var trie = CreateTrie();
		trie.Add("test", 1);

		Assert.That(trie.TryGetValue("test", out var value), Is.True);
		Assert.That(value!.Value, Is.EqualTo(1));
	}

	[Test]
	public void Add_OverwritesExistingValue()
	{
		var trie = CreateTrie();
		trie.Add("test", 1);
		trie.Add("test", 2);

		Assert.That(trie.TryGetValue("test", out var value), Is.True);
		Assert.That(value, Is.EqualTo(2));
	}

	[Test]
	public void TryGetValue_ReturnsFalseForNonExistentKey()
	{
		var trie = CreateTrie();

		Assert.That(trie.TryGetValue("test", out _), Is.False);
	}

	[Test]
	public void RemoveKey_RemovesKeyFromTrie()
	{
		var trie = CreateTrie();
		trie.Add("test", 1);
		trie.RemoveKey("test");

		Assert.That(trie.TryGetValue("test", out _), Is.False);
	}

	[Test]
	public void LongestPrefixOf_ReturnsLongestPrefix()
	{
		var trie = CreateTrie();
		trie.Add("test", 1);
		trie.Add("testing", 2);

		Assert.That(trie.LongestPrefixOf("tester"), Is.EqualTo("test"));
	}

	[Test]
	public void KeysWithPrefix_ReturnsKeysWithGivenPrefix()
	{
		var trie = CreateTrie();
		trie.Add("test", 1);
		trie.Add("testing", 2);
		trie.Add("toast", 3);

		var keys = trie.KeysWithPrefix("test").ToList();

		Assert.That(keys.Count, Is.EqualTo(2));
		Assert.That(keys, Does.Contain("test"));
		Assert.That(keys, Does.Contain("testing"));
	}

	[Test]
	[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "Test case")]
	public void KeysThatMatch_ReturnsKeysThatMatchPattern()
	{
		var trie = CreateTrie();
		trie.Add("test", 1);
		trie.Add("toast", 2);
		trie.Add("teast", 3);

		var keys = trie.KeysThatMatch("t.ast").ToList();

		Assert.That(keys.Count, Is.EqualTo(2));
		Assert.That(keys, Does.Contain("toast"));
		Assert.That(keys, Does.Contain("teast"));
	}

	private T CreateTrie() => Types.GetInstance<T>();
}
