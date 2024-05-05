namespace UnitTests;

using System.Linq;
// This is often needed for LINQ methods like .Count()
using NUnit.Framework;
using static AlgorithmsSW.Algorithms;

[TestFixture]
public class PowerSetTests
{
	[Test]
	public void TestEmptySet()
	{
		var result = PowerSet(new int[] { });
		Assert.That(result.Count, Is.EqualTo(1)); // Expecting only one subset: the empty set
		Assert.That(result[0].Count, Is.EqualTo(0)); // The only subset should be empty
	}

	[Test]
	public void TestSingleElementSet()
	{
		var result = PowerSet(new[] { "a" });
		Assert.That(result.Count, Is.EqualTo(2)); // Empty set + set with one element
		Assert.That(result, Has.Some.Contains("a")); // Ensure one subset contains the element
	}

	[Test]
	public void TestTwoElementSet()
	{
		var result = PowerSet(new[] { "a", "b" });
		Assert.That(result.Count, Is.EqualTo(4)); // Expecting four subsets
		Assert.That(result, Has.Exactly(1).EquivalentTo(new string[] { })); // Empty set
		Assert.That(result, Has.Exactly(1).EquivalentTo(new[] { "a" })); // Single elements
		Assert.That(result, Has.Exactly(1).EquivalentTo(new[] { "b" }));
		Assert.That(result, Has.Exactly(1).EquivalentTo(new[] { "a", "b" })); // All elements
	}

	[Test]
	public void TestMultipleElementSet()
	{
		var result = PowerSet(new[] { 1, 2, 3 });
		Assert.That(result.Count, Is.EqualTo(8)); // 2^3 subsets for 3 elements
	}

	[Test]
	public void TestNullInput()
	{
		Assert.That(() => PowerSet<int>(null), Throws.ArgumentNullException);
	}

	[Test]
	public void TestTypeIntegrity()
	{
		var result = PowerSet(new[] { 1, 2 });
		Assert.That(result.All(subset => subset.All(item => item is int)), Is.True);
	}

	[Test]
	public void TestDuplicatesInInputSet()
	{
		var result = PowerSet(new[] { "a", "a" }); // Handling duplicates
		// Should treat duplicates as unique for set purposes or not, based on your implementation
		Assert.That(result.Count, Is.EqualTo(4)); // Empty, one 'a', two 'a's
	}

	[Test]
	public void TestLargeSet()
	{
		var largeSet = Enumerable.Range(1, 10).ToArray(); // A set with 10 elements
		var result = PowerSet(largeSet);
		Assert.That(result.Count, Is.EqualTo(1024)); // 2^10 subsets for 10 elements
	}

	[Test]
	public void TestSpecialCharactersAndTypes()
	{
		var specialSet = new[] { "@", "#" };
		var result = PowerSet(specialSet);
		Assert.That(result.Count, Is.EqualTo(4)); // Empty, '@', '#', '@#'
		Assert.That(result, Has.Exactly(1).EquivalentTo(new[] { "@" }));
		Assert.That(result, Has.Exactly(1).EquivalentTo(new[] { "#" }));
		Assert.That(result, Has.Exactly(1).EquivalentTo(new[] { "@", "#" }));
	}
}
