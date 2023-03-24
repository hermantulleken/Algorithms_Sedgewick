namespace Algorithms_Sedgewick_Tests;

using System.Collections.Generic;
using System.Linq;
using Algorithms_Sedgewick.SearchTrees;
using NUnit.Framework;

[TestFixture]
public class BinarySearchTreeTests
{
	[SetUp]
	public void SetUp()
	{
		tree = new BinarySearchTree<int>(Comparer<int>.Default);
		tree.Add(4);
		
		tree.Add(2);
		tree.Add(1);
		tree.Add(3);
		
		tree.Add(6);
		tree.Add(5);
		tree.Add(7);
	}

	private BinarySearchTree<int> tree = null!;

	[Test]
	public void TestPreOrderTraversal()
	{
		int[] expectedTraversal = { 4, 2, 1, 3, 6, 5, 7 };
		int[] actualTraversal = tree.NodesPreOrder.Select(node => node.Item).ToArray();

		Assert.That(actualTraversal, Is.EqualTo(expectedTraversal));
	}

	[Test]
	public void TestInOrderTraversal()
	{
		int[] expectedTraversal = { 1, 2, 3, 4, 5, 6, 7 };
		int[] actualTraversal = tree.NodesInOrder.Select(node => node.Item).ToArray();

		Assert.That(actualTraversal, Is.EqualTo(expectedTraversal));
	}

	[Test]
	public void TestPostOrderTraversal()
	{
		int[] expectedTraversal = { 1, 3, 2, 5, 7, 6, 4 };
		int[] actualTraversal = tree.NodesPostOrder.Select(node => node.Item).ToArray();

		Assert.That(actualTraversal, Is.EqualTo(expectedTraversal));
	}

	[Test]
	public void TestLevelOrderTraversal()
	{
		int[] expectedTraversal = { 4, 2, 6, 1, 3, 5, 7 };
		int[] actualTraversal = tree.NodesLevelOrder.Select(node => node.Item).ToArray();

		Assert.That(actualTraversal, Is.EqualTo(expectedTraversal));
	}

	[Test]
	public void AddTest()
	{
		var bst = new BinarySearchTree<int>(Comparer<int>.Default);

		// Test adding elements to the tree
		int[] elementsToAdd = { 8, 3, 10, 1, 6, 14, 4, 7, 13 };
		
		foreach (int element in elementsToAdd)
		{
			bst.Add(element);
		}

		// Check if the elements are added in the correct order
		int[] expectedInOrderTraversal = { 1, 3, 4, 6, 7, 8, 10, 13, 14 };
		var inOrderTraversal = new List<int>();
		
		foreach (var node in bst.NodesInOrder)
		{
			inOrderTraversal.Add(node.Item);
		}
		
		Assert.That(inOrderTraversal, Is.EqualTo(expectedInOrderTraversal));
	}

	[Test]
	public void RemoveTest()
	{
		var bst = new BinarySearchTree<int>(Comparer<int>.Default);

		// Create a tree
		int[] elementsToAdd = { 8, 3, 10, 1, 6, 14, 4, 7, 13 };
		
		foreach (int element in elementsToAdd)
		{
			bst.Add(element);
		}

		// Test removing a leaf node
		bst.Remove(1);
		int[] expectedInOrderTraversalAfterLeafRemoval = { 3, 4, 6, 7, 8, 10, 13, 14 };
		var inOrderTraversalAfterLeafRemoval = new List<int>();
		
		foreach (var node in bst.NodesInOrder)
		{
			inOrderTraversalAfterLeafRemoval.Add(node.Item);
		}
		
		Assert.That(inOrderTraversalAfterLeafRemoval, Is.EqualTo(expectedInOrderTraversalAfterLeafRemoval));

		// Test removing a node with one child
		bst.Remove(14);
		int[] expectedInOrderTraversalAfterOneChildRemoval = { 3, 4, 6, 7, 8, 10, 13 };
		var inOrderTraversalAfterOneChildRemoval = new List<int>();
		
		foreach (var node in bst.NodesInOrder)
		{
			inOrderTraversalAfterOneChildRemoval.Add(node.Item);
		}
		
		Assert.That(inOrderTraversalAfterOneChildRemoval, Is.EqualTo(expectedInOrderTraversalAfterOneChildRemoval));

		// Test removing a node with two children
		bst.Remove(3);
		int[] expectedInOrderTraversalAfterTwoChildrenRemoval = { 4, 6, 7, 8, 10, 13 };
		var inOrderTraversalAfterTwoChildrenRemoval = new List<int>();
		
		foreach (var node in bst.NodesInOrder)
		{
			inOrderTraversalAfterTwoChildrenRemoval.Add(node.Item);
		}
		
		Assert.That(inOrderTraversalAfterTwoChildrenRemoval, Is.EqualTo(expectedInOrderTraversalAfterTwoChildrenRemoval));
	}
}
