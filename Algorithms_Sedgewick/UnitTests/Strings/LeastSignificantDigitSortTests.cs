namespace UnitTests.Strings;

using System.Diagnostics.CodeAnalysis;
using AlgorithmsSW.String;
using NUnit.Framework;

[TestFixture(0)]
[TestFixture(1)]
[TestFixture(2)]
[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "All strings are abstrct examples.")]
public class StringSortTests
{
	Action<string[]>[] algorithms =
	[
		strings => StringSort.LeastSignificantDigitSort(strings, 3, 256),
		StringSort.MostSignificantDigitSort,
		StringSort.Quicksort3Way,
	];
	
	private Action<string[]> algorithm;
	
	public StringSortTests(int algorithmIndex)
	{
		algorithm = algorithms[algorithmIndex];
	}
	
	[Test]
	public void Sort_WithTypicalInput_ShouldSortCorrectly()
	{
		string[] strings = ["bca", "acb", "bac", "abc", "cba"];
		int leadingCharsToSort = 3;
		int radix = 256; // ASCII radix

		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new[] { "abc", "acb", "bac", "bca", "cba" }));
	}

	[Test]
	public void Sort_WithEmptyArray_ShouldNotModifyArray()
	{
		string[] strings = [];
		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new string[] { }));
	}

	[Test]
	public void Sort_WithAllSameStrings_ShouldNotModifyArray()
	{
		string[] strings = ["aaa", "aaa", "aaa"];
		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new[] { "aaa", "aaa", "aaa" }));
	}

	[Test]
	public void Sort_WithSingleCharacterStrings_ShouldSortCorrectly()
	{
		string[] strings = ["c", "b", "a"];
		algorithm(strings);

		Assert.That(strings, Is.EqualTo(new[] { "a", "b", "c" }));
	}

	[Test]
	public void Sort_WithNullStrings_ShouldThrowArgumentException()
	{
		string[]? strings = null;

		var exception = Assert.Throws<ArgumentNullException>(() => algorithm(strings));
		Assert.That(exception!.ParamName, Is.EqualTo("strings"));
	}

	[Test]
	public void Sort_WithMixedCaseStrings_ShouldSortCaseSensitive()
	{
		string[] strings = ["aBc", "Abc", "abc"];
		algorithm(strings);

		// Note: ASCII capital letters are lower than lowercase letters
		Assert.That(strings, Is.EqualTo(new[] { "Abc", "aBc", "abc" }));
	}
}

[TestFixture]
[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "All strings are abstrct examples.")]
public class LeastSignificantDigitSortTests
{
	[Test]
	public void Sort_WithLeadingCharsLessThanStringLength_ShouldSortBasedOnLeadingCharsOnly()
	{
		string[] strings = ["abcde", "abdce", "abcce"];
		int leadingCharsToSort = 3; // Only sort based on first three characters
		int radix = 256;

		StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix);

		Assert.That(strings, Is.EqualTo(new[] { "abcde", "abcce", "abdce" }));
	}

	[Test]
	public void Sort_WithInvalidLeadingChars_ShouldThrowArgumentException()
	{
		string[] strings = ["abc", "def"];
		int leadingCharsToSort = -1; // Invalid number of leading characters
		int radix = 256;

		Assert.Throws<ArgumentOutOfRangeException>(() => StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix));
	}

	[Test]
	public void Sort_WithInvalidRadix_ShouldThrowArgumentException()
	{
		string[] strings = ["abc", "def"];
		int leadingCharsToSort = 3;
		int radix = 0; // Invalid radix

		Assert.Throws<ArgumentOutOfRangeException>(() => StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix));
	}

	[Test]
	public void Sort_WithStringsShorterThanLeadingChars_ShouldThrowArgumentException()
	{
		string[] strings = ["hi", "hello"];
		int leadingCharsToSort = 5; // More than length of "hi"
		int radix = 256;

		Assert.Throws<ArgumentException>(() => StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix));
	}
	
	[Test]
	public void Sort_GeorgianStrings_ShouldSortCorrectly()
	{
		string[] strings = ["გეგები", "ბაბუა", "დედა"]; // Unsorted Georgian words
		string[] expected = ["ბაბუა", "გეგები", "დედა"]; // Correctly sorted array

		StringSort.LeastSignificantDigitSort(strings, 4, Alphabet.Mkhedruli); // Replace 'YourSortClass.GeorgianSort' with your actual method

		Assert.That(strings, Is.EqualTo(expected));
	}
}
