using AlgorithmsSW.SearchTrees;

namespace UnitTests;

using System.Collections.Generic;
using AlgorithmsSW.SymbolTable;
using NUnit.Framework;

[TestFixture]
public class OrderedSymbolTableTests
{
	private static TestCaseData[] factories =
	{
		new(() => new OrderedSymbolTableWithOrderedArray<string, int>(SharedData.StringComparer))
			{ TestName = "OST with Ordered Array" },
		new(() => new SymbolTableWithBinarySearchTree<string, int>(BinarySearchTree.Plain, SharedData.StringComparer))
			{ TestName = "OST with Plain Binary Search Tree" },
		new(() => new SymbolTableWithBinarySearchTree<string, int>(BinarySearchTree.RedBlack, SharedData.StringComparer))
			{ TestName = "OST with Red Black Binary Search Tree" },
		
		new(() => new OrderedSymbolTableWithOrderedKeyArray<string, int>(SharedData.StringComparer))
			{ TestName = "OST with Ordered Key Array" },
		
		new(() => new OrderedSymbolTableWithOrderedLinkedList<string, int>(SharedData.StringComparer))
			{ TestName = "OST with Ordered LinkedList" },
		new(() => new OrderedSymbolTableWithUnorderedLinkedList<string, int>(SharedData.StringComparer))
			{ TestName = "OST with Unordered linked list" },
		new(() => new SymbolTableWithOrderedParallelArray<string, int>(SharedData.StringComparer))
			{ TestName = "OST with Ordered Parallel Array" },
	};

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestIsEmpty(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		Assert.That(symbolTable.IsEmpty, Is.True);
		AddElements(symbolTable);
		Assert.That(symbolTable.IsEmpty, Is.False);
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestMinKeyAndMaxKey(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		Assert.That(symbolTable.MinKey(), Is.EqualTo("apple"));
		Assert.That(symbolTable.MaxKey(), Is.EqualTo("cherry"));
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestLargestKeyLessThanOrEqualTo(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		Assert.That(symbolTable.LargestKeyLessThanOrEqualTo("banana"), Is.EqualTo("banana"));
		Assert.That(symbolTable.LargestKeyLessThanOrEqualTo("blueberry"), Is.EqualTo("banana"));
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestSmallestKeyGreaterThanOrEqualTo(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		Assert.That(symbolTable.SmallestKeyGreaterThanOrEqualTo("banana"), Is.EqualTo("banana"));
		Assert.That(symbolTable.SmallestKeyGreaterThanOrEqualTo("blueberry"), Is.EqualTo("cherry"));
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestRankOf(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		Assert.That(symbolTable.RankOf("banana"), Is.EqualTo(1));
		Assert.That(symbolTable.RankOf("blueberry"), Is.EqualTo(2));
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestKeyWithRank(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		Assert.That(symbolTable.KeyWithRank(1), Is.EqualTo("banana"));
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestCountRange(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		Assert.That(symbolTable.CountRange("apple", "cherry"), Is.EqualTo(2));
		Assert.That(symbolTable.CountRange("banana", "blueberry"), Is.EqualTo(1));
	}

	[Test]
	[TestCaseSource(nameof(factories))]
	public void TestKeysRange(Func<IOrderedSymbolTable<string, int>> factory)
	{
		var symbolTable = factory();
		AddElements(symbolTable);

		IEnumerable<string> keysInRange = symbolTable.KeysRange("apple", "cherry");
		CollectionAssert.AreEqual(keysInRange, new List<string> { "apple", "banana" });

		keysInRange = symbolTable.KeysRange("banana", "blueberry");
		CollectionAssert.AreEqual(keysInRange, new List<string> { "banana" });
	}

	private void AddElements(IOrderedSymbolTable<string, int> symbolTable)
	{
		symbolTable["apple"] = 1;
		symbolTable["banana"] = 2;
		symbolTable["cherry"] = 3;
	}
}
