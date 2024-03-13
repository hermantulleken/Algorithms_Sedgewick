namespace UnitTests.Strings;

using System.Diagnostics.CodeAnalysis;
using AlgorithmsSW.List;
using AlgorithmsSW.String;
using NUnit.Framework;

[TestFixture]
[SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "All strings are abstrct examples.")]
public class LeastSignificantDigitSortTests
{
	[Test]
	public void Sort_WithLeadingCharsLessThanStringLength_ShouldSortBasedOnLeadingCharsOnly()
	{
		ResizeableArray<string> strings = ["abcde", "abdce", "abcce"];
		int leadingCharsToSort = 3; // Only sort based on first three characters
		int radix = 256;

		StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix);

		Assert.That(strings, Is.EqualTo(new[] { "abcde", "abcce", "abdce" }));
	}

	[Test]
	public void Sort_WithInvalidLeadingChars_ShouldThrowArgumentException()
	{
		ResizeableArray<string> strings = ["abc", "def"];
		int leadingCharsToSort = -1; // Invalid number of leading characters
		int radix = 256;

		Assert.Throws<ArgumentOutOfRangeException>(() => StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix));
	}

	[Test]
	public void Sort_WithInvalidRadix_ShouldThrowArgumentException()
	{
		ResizeableArray<string> strings = ["abc", "def"];
		int leadingCharsToSort = 3;
		int radix = 0; // Invalid radix

		Assert.Throws<ArgumentOutOfRangeException>(() => StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix));
	}

	[Test]
	public void Sort_WithStringsShorterThanLeadingChars_ShouldThrowArgumentException()
	{
		ResizeableArray<string> strings = ["hi", "hello"];
		int leadingCharsToSort = 5; // More than length of "hi"
		int radix = 256;

		Assert.Throws<ArgumentException>(() => StringSort.LeastSignificantDigitSort(strings, leadingCharsToSort, radix));
	}
	
	[Test]
	public void Sort_GeorgianStrings_ShouldSortCorrectly()
	{
		ResizeableArray<string> strings = ["გეგები", "ბაბუა", "დედა"]; // Unsorted Georgian words
		ResizeableArray<string> expected = ["ბაბუა", "გეგები", "დედა"]; // Correctly sorted array

		StringSort.LeastSignificantDigitSort(strings, 4, Alphabet.Mkhedruli); // Replace 'YourSortClass.GeorgianSort' with your actual method

		Assert.That(strings, Is.EqualTo(expected));
	}
}
