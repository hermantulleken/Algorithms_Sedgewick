using System.Collections.Generic;
using Algorithms_Sedgewick.HashTable;

namespace Algorithms_Sedgewick_Tests;

using Algorithms_Sedgewick.SymbolTable;
using NUnit.Framework;

[Parallelizable]
public class SymbolTableTests
{
	private static TestCaseData[] factories =
	{
		new(() => new OrderedSymbolTableWithOrderedArray<int, string>(SharedData.IntComparer))
			{ TestName = "OST with Ordered Array" },
		
		new(() => new SymbolTableWithBinarySearchTree<int, string>(SharedData.IntComparer))
			{ TestName = "OST with Binary Search Tree" },
		
		new(() => new OrderedSymbolTableWithOrderedKeyArray<int, string>(SharedData.IntComparer))
			{ TestName = "OST with Ordered Key Array" },
		
		new(() => new OrderedSymbolTableWithOrderedLinkedList<int, string>(SharedData.IntComparer))
			{ TestName = "OST with Ordered LinkedList" },
		
		new(() => new OrderedSymbolTableWithUnorderedLinkedList<int, string>(SharedData.IntComparer))
			{ TestName = "OST with Unordered linked list" },
		
		new(() => new SymbolTableWithOrderedParallelArray<int, string>(SharedData.IntComparer))
			{ TestName = "OST with Ordered Parallel Array" },
		
		new(() => new HashTableWithLinearProbing<int, string>(SharedData.IntComparer))
			{ TestName = "HT with Linear Probing" },
		
		new(() => new HashTableWithLinearProbingAndLazyDelete<int, string>(SharedData.IntComparer))
			{ TestName = "HT with Linear Probing and Lazy Deletion" },
		
		new(() => new HashTableWithLinearProbing<int, string>(SharedData.IntComparer))
			{ TestName = "HT with Separate chaining" },
	};

	[TestCaseSource(nameof(factories))]
	public void TestEmptySymbolTable(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		Assert.That(table.Count, Is.EqualTo(0));
		Assert.That(table.IsEmpty, Is.True);
	}

	[TestCaseSource(nameof(factories))]
	public void TestAddSingleEntry(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		Assert.That(table.Count, Is.EqualTo(1));
		Assert.That(table.IsEmpty, Is.False);
	}

	[TestCaseSource(nameof(factories))]
	public void TestAddMultipleEntries(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		table[2] = "two";
		table[3] = "three";
		Assert.That(table.Count, Is.EqualTo(3));
		Assert.That(table.IsEmpty, Is.False);
	}

	[TestCaseSource(nameof(factories))]
	public void TestIndexerGet(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		Assert.That(table[1], Is.EqualTo("one"));
	}

	[TestCaseSource(nameof(factories))]
	public void TestIndexerSet(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		table[1] = "uno";
		Assert.That(table[1], Is.EqualTo("uno"));
	}

	[TestCaseSource(nameof(factories))]
	public void TestIndexerSetNewKey(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		table[2] = "two";
		Assert.That(table.Count, Is.EqualTo(2));
	}

	[TestCaseSource(nameof(factories))]
	public void TestKeysEnumeration(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		table[2] = "two";
		table[3] = "three";
		Assert.That(table.Keys, Is.EquivalentTo(new[] { 1, 2, 3 }));
	}

	[TestCaseSource(nameof(factories))]
	public void TestContainsKeyTrue(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		Assert.That(table.ContainsKey(1), Is.True);
	}

	[TestCaseSource(nameof(factories))]
	public void TestContainsKeyFalse(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		Assert.That(table.ContainsKey(2), Is.False);
	}

	[TestCaseSource(nameof(factories))]
	public void TestRemoveKeyExisting(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		table[2] = "two";
		table.RemoveKey(1);
		Assert.That(table.Count, Is.EqualTo(1));
		Assert.That(table.ContainsKey(1), Is.False);
	}
	
	[TestCaseSource(nameof(factories))]
	public void TestRemoveKeyNonExisting(Func<ISymbolTable<int, string>> factory)
	{
		var table = factory();
		table[1] = "one";
		table[2] = "two";
		void RemoveKey() => table.RemoveKey(3);

		Assert.Throws<KeyNotFoundException>(RemoveKey);
	}
}
