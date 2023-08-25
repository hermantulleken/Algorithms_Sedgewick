using Algorithms_Sedgewick;
using NUnit.Framework;

namespace UnitTests;

[TestFixture]
public class MedianTests
{
	[Test]
	public void TestMedianWithOddNumberOfElements()
	{
		float[] data = { 5, 3, 2, 7, 1 };
		float median = Algorithms.Median(data);
		Assert.That(median, Is.EqualTo(3));
	}

	[Test]
	public void TestMedianWithEvenNumberOfElements()
	{
		float[] data = { 5, 2, 7, 1 };
		float median = Algorithms.Median(data);
		Assert.That(median, Is.EqualTo(2f));
	}

	[Test]
	public void TestMedianWithDuplicateValues()
	{
		float[] data = { 5, 5, 5, 5 };
		float median = Algorithms.Median(data);
		Assert.That(median, Is.EqualTo(5));
	}

	[Test]
	public void TestMedianWithSingleElement()
	{
		float[] data = { 5 };
		float median = Algorithms.Median(data);
		Assert.That(median, Is.EqualTo(5));
	}

	[Test]
	public void TestMedianWithEmptyList()
	{
		float[] data = Array.Empty<float>();
		Assert.Throws<IndexOutOfRangeException>(() => Algorithms.Median(data));
	}
}
