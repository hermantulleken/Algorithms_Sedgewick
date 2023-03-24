namespace Algorithms_Sedgewick_Tests;

using System.Collections.Generic;
using Algorithms_Sedgewick;
using Algorithms_Sedgewick.List;
using NUnit.Framework;

[Parallelizable]
public class AlgorithmTests
{
	public class Person : IComparable<Person>
	{
		public int Age { get; set; }
		public string Name { get; set; }

		public int CompareTo(Person other)
		{
			if (other == null)
			{
				return 1;
			}

			int nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);
			if (nameComparison != 0)
			{
				return nameComparison;
			}

			return Age.CompareTo(other.Age);
		}
	}

	public class Point : IComparable<Point>
	{
		public int X { get; set; }
		public int Y { get; set; }

		public int CompareTo(Point other)
		{
			if (other == null)
			{
				return 1;
			}

			int xComparison = X.CompareTo(other.X);
			if (xComparison != 0)
			{
				return xComparison;
			}

			return Y.CompareTo(other.Y);
		}
	}

	private static readonly ResizeableArray<int> Empty = new();

	private static readonly ResizeableArray<int> List13335 = new(){ 1, 3, 3, 3, 5 };
	private static readonly ResizeableArray<int> List135 = new(){ 1, 3, 5 };

	private static readonly IEnumerable<TestCaseData> FindTestCases = new List<TestCaseData>
	{
		new(Empty, 0) {ExpectedResult = 0, TestName = "Empty"},
		new(List135, 0) {ExpectedResult = 0, TestName = "Item before first"},
		new(List135, 6) {ExpectedResult = 3, TestName = "Item after last"},
		
		new(List135, 2) {ExpectedResult = 1, TestName = "Item after first"},
		new(List135, 4) {ExpectedResult = 2, TestName = "Item before last"},
		
		new(List135, 1) {ExpectedResult = 1, TestName = "Item at first"},
		new(List135, 3) {ExpectedResult = 2, TestName = "Item at second"},
		new(List135, 5) {ExpectedResult = 3, TestName = "Item at last"},
		new(List13335, 3) {ExpectedResult = 4, TestName = "Item at second to fourth"},
	};

	private static readonly IComparer<int> IncComparer = Comparer<int>.Default;


	[TestCaseSource(nameof(FindTestCases))]
	public int TestEqualsSecond(IReadonlyRandomAccessList<int> list, int itemToPlace)
		=> list.FindInsertionIndex(itemToPlace, Comparer<int>.Default);

	#region Remove Duplicates

	[Test]
	public void SortAndRemoveDuplicates_NullArray_ThrowsArgumentNullException()
	{
		ResizeableArray<int> array = null;

		Assert.That(() => array.SortAndRemoveDuplicates(), Throws.ArgumentNullException);
	}


	[Test]
	public void SortAndRemoveDuplicates_SmallArray_SortsElements()
	{
		var array = new ResizeableArray<int> { 3, 1, 4, 2 };

		array.SortAndRemoveDuplicates();

		Assert.That(array[0], Is.EqualTo(1));
		Assert.That(array[1], Is.EqualTo(2));
		Assert.That(array[2], Is.EqualTo(3));
		Assert.That(array[3], Is.EqualTo(4));
	}

	[Test]
	public void SortAndRemoveDuplicates_LargeArray_SortsElements()
	{
		var array = new ResizeableArray<int>();
		for (int i = 0; i < 10000; i++)
		{
			array.Add(i % 100);
		}

		array.SortAndRemoveDuplicates();

		Assert.That(array.Count, Is.EqualTo(100));
		for (int i = 0; i < array.Count - 1; i++)
		{
			Assert.That(array[i], Is.LessThanOrEqualTo(array[i + 1]));
		}
	}

	[Test]
	public void SortAndRemoveDuplicates_ArrayWithDuplicates_RemovesDuplicates()
	{
		var array = new ResizeableArray<int> { 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };

		array.SortAndRemoveDuplicates();

		Assert.That(array.Count, Is.EqualTo(4));
		Assert.That(array[0], Is.EqualTo(1));
		Assert.That(array[1], Is.EqualTo(2));
		Assert.That(array[2], Is.EqualTo(3));
		Assert.That(array[3], Is.EqualTo(4));
	}

	[Test]
	public void SortAndRemoveDuplicates_ArrayWithNoDuplicates_PreservesElements()
	{
		var array = new ResizeableArray<int> { 1, 2, 3, 4, 5 };

		array.SortAndRemoveDuplicates();

		Assert.That(array.Count, Is.EqualTo(5));
		Assert.That(array[0], Is.EqualTo(1));
		Assert.That(array[1], Is.EqualTo(2));
		Assert.That(array[2], Is.EqualTo(3));
		Assert.That(array[3], Is.EqualTo(4));
		Assert.That(array[4], Is.EqualTo(5));
	}

	[Test]
	public void SortAndRemoveDuplicates_ArrayWithAllDuplicates_RemovesAllDuplicates()
	{
		var array = new ResizeableArray<int> { 1, 1, 1, 1, 1 };

		array.SortAndRemoveDuplicates();

		Assert.That(array.Count, Is.EqualTo(1));
		Assert.That(array[0], Is.EqualTo(1));
	}

	[Test]
	public void SortAndRemoveDuplicates_ArrayPreservesRelativeOrderOfNonDuplicates()
	{
		var array = new ResizeableArray<int> { 3, 1, 2, 5, 4 };

		array.SortAndRemoveDuplicates();

		Assert.That(array.Count, Is.EqualTo(5));
		Assert.That(array[0], Is.EqualTo(1));
		Assert.That(array[1], Is.EqualTo(2));
		Assert.That(array[2], Is.EqualTo(3));
		Assert.That(array[3], Is.EqualTo(4));
		Assert.That(array[4], Is.EqualTo(5));
	}

	[Test]
	public void SortAndRemoveDuplicates_ArrayOfValueTypesWithCustomEqualityComparison_RemovesDuplicates()
	{
		var array = new ResizeableArray<Point>
		{
			new() { X = 1, Y = 2 },
			new() { X = 2, Y = 1 },
			new() { X = 1, Y = 2 }
		};

		array.SortAndRemoveDuplicates();

		Assert.That(array.Count, Is.EqualTo(2));
		Assert.That(array[0].X, Is.EqualTo(1));
		Assert.That(array[0].Y, Is.EqualTo(2));
		Assert.That(array[1].X, Is.EqualTo(2));
		Assert.That(array[1].Y, Is.EqualTo(1));
	}

	[Test]
	public void SortAndRemoveDuplicates_EmptyArray_DoesNotThrowException()
	{
		var array = new ResizeableArray<int>();

		Assert.That(() => array.SortAndRemoveDuplicates(), Throws.Nothing);
		Assert.That(array.Count, Is.EqualTo(0));
	}

	[Test]
	public void SortAndRemoveDuplicates_SingleElementArray_DoesNotThrowException()
	{
		var array = new ResizeableArray<int> { 42 };

		Assert.That(() => array.SortAndRemoveDuplicates(), Throws.Nothing);
		Assert.That(array.Count, Is.EqualTo(1));
		Assert.That(array[0], Is.EqualTo(42));
	}

	#endregion

	#region BinaryRankTests

	private static IEnumerable<TestCaseData> BinaryRankTestCases()
	{
		var emptyList = new ResizeableArray<int>();
		var singletonList = new ResizeableArray<int> { 2 };
		var repeatList = new ResizeableArray<int> { 2, 2, 2 };
		var generalList = new ResizeableArray<int> { 0, 1, 2, 3, 3, 5 };

		return new TestCaseData[]
		{
			new(emptyList, 0, 0),
			
			new(singletonList, 0, 0),
			new(singletonList, 2, 0),
			new(singletonList, 4, 1),
			
			new(repeatList, 0, 0),
			new(repeatList, 2, 0),
			new(repeatList, 4, 3),
			
			new(generalList, -1, 0),
			new(generalList, 0, 0),
			new(generalList, 1, 1),
			new(generalList, 2, 2),
			new(generalList, 3, 3),
			new(generalList, 4, 5),
			new(generalList, 5, 5),
			new(generalList, 6, 6),
		};
	}

	[Test, TestCaseSource(nameof(BinaryRankTestCases))]
	public void TestBinaryRank(ResizeableArray<int> list, int value, int expectedRank)
	{
		Assert.That(list.BinaryRank(value, IncComparer), Is.EqualTo(expectedRank));
	}

	[Test, TestCaseSource(nameof(BinaryRankTestCases))]
	public void TestSequentialRank(ResizeableArray<int> list, int value, int expectedRank)
	{
		Assert.That(list.SequentialRank(value, IncComparer), Is.EqualTo(expectedRank));
	}

	#endregion
}
