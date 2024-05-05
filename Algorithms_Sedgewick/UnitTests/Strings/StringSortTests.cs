namespace UnitTests.Strings;

using System.Diagnostics.CodeAnalysis;
using AlgorithmsSW.List;
using AlgorithmsSW.String;
using Assert = NUnit.Framework.Assert;

[TestFixture(0)]
[TestFixture(1)]
[TestFixture(2)]
[TestFixture(3)]
[TestFixture(4)]
[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "All strings are abstrct examples.")]
public class StringSortTests
{
	private readonly Action<IRandomAccessList<string>>[] algorithms =
	[
		strings => StringSort.LeastSignificantDigitSort(strings, 3, 256),
		StringSort.MostSignificantDigitSort,
		StringSort.Quicksort3Way,
		strings => StringSort.CountSort(strings, StringComparer.Ordinal),
		strings => StringSort.CountSortWithQueues(strings, StringComparer.Ordinal),
	];
	
	private readonly Action<IRandomAccessList<string>>[] singleCharAlgorithms =
	[
		strings => StringSort.LeastSignificantDigitSort(strings, 1, 256),
		StringSort.MostSignificantDigitSort,
		StringSort.Quicksort3Way,
		strings => StringSort.CountSort(strings, StringComparer.Ordinal),
		strings => StringSort.CountSortWithQueues(strings, StringComparer.Ordinal),
	];
	
	private readonly Action<IRandomAccessList<string>> algorithm;
	private readonly Action<IRandomAccessList<string>> singleCharAlgorithm;
	
	public StringSortTests(int algorithmIndex)
	{
		algorithm = algorithms[algorithmIndex];
		singleCharAlgorithm = singleCharAlgorithms[algorithmIndex];
	}

	[Test]
	public void Sort_WithTypicalInput_ShouldSortCorrectly()
	{
		ResizeableArray<string> strings = ["bca", "acb", "bac", "abc", "cba"];
		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new[] { "abc", "acb", "bac", "bca", "cba" }));
	}

	[Test]
	public void Sort_WithEmptyArray_ShouldNotModifyArray()
	{
		ResizeableArray<string> strings = [];
		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new string[] { }));
	}

	[Test]
	public void Sort_WithAllSameStrings_ShouldNotModifyArray()
	{
		ResizeableArray<string> strings = ["aaa", "aaa", "aaa"];
		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new[] { "aaa", "aaa", "aaa" }));
	}

	[Test]
	public void Sort_WithSingleCharacterStrings_ShouldSortCorrectly()
	{
		ResizeableArray<string> strings = ["c", "b", "a"];
		singleCharAlgorithm(strings);

		Assert.That(strings, Is.EqualTo(new[] { "a", "b", "c" }));
	}

	[Test]
	public void Sort_WithNullStrings_ShouldThrowArgumentException()
	{
		ResizeableArray<string>? strings = null;

		var exception = Assert.Throws<ArgumentNullException>(() => algorithm(strings!));
		Assert.That(exception!.ParamName, Is.EqualTo("list"));
	}

	[Test]
	public void Sort_WithMixedCaseStrings_ShouldSortCaseSensitive()
	{
		ResizeableArray<string> strings = ["aBc", "Abc", "abc"];
		algorithm(strings);

		// Note: ASCII capital letters are lower than lowercase letters
		Assert.That(strings, Is.EqualTo(new[] { "Abc", "aBc", "abc" }));
	}
}
