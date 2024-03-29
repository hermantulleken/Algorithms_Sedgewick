﻿namespace AlgorithmsSW.PriorityQueue;

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Queue;
using Support;
using static System.Diagnostics.Debug;

public class PriorityTree<T>
	: IPriorityQueue<T> 
{
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = Tools.DataTransferStruct)]
	private sealed class Node
	{
		public T Item;
		public Node? LeftChild;
		public Node? Parent;
		public Node? RightChild;

		public Node(T item)
		{
			Item = item;
		}

		public void BindLeftChild(Node? child)
		{
			if (child == null)
			{
				LeftChild = null;
				return;
			}
			
			LeftChild?.ClearParent();
			
			if (child.Parent != null)
			{
				if (child.IsLeftChildOfParent())
				{
					child.Parent.ClearLeftChild();
				}
				else
				{
					child.Parent.ClearRightChild();
				}
			}
			
			child.Parent = this;
			LeftChild = child;
		}

		public void BindRightChild(Node? child)
		{
			if (child == null)
			{
				RightChild = null;
				return;
			}
			
			RightChild?.ClearParent();

			if (child.Parent != null)
			{
				if (child.IsLeftChildOfParent())
				{
					child.Parent.ClearLeftChild();
				}
				else
				{
					child.ClearRightChild();
				}
			}
			
			child.Parent = this;
			RightChild = child;
		}

		public void CutLeftChild()
		{
			if (LeftChild == null)
			{
				return;
			}
			
			Assert(LeftChild.Parent == this);
			LeftChild.Parent = null;
			LeftChild = null;
		}

		public void CutRightChild()
		{
			if (RightChild == null)
			{
				return;
			}
			
			Assert(RightChild.Parent == this);
			RightChild.Parent = null;
			RightChild = null;
		}

		public void SetLeftChild(Node? child)
		{
			LeftChild = child;
			
			if (child != null)
			{
				child.Parent = this;
			}
		}

		public void SetRightChild(Node? child)
		{
			RightChild = child;

			if (child != null)
			{
				child.Parent = this;
			}
		}

		public void SwapWithParent()
		{
			Assert(Parent != null);
			
			var parent = Parent;
			var grandparent = parent.Parent;
			var leftChild = LeftChild;
			var rightChild = RightChild;

			bool parentIsLeftChild = grandparent != null && grandparent.LeftChild == parent;
			bool isLeftChild = parent.LeftChild == this;

			var oppositeChild = isLeftChild ? parent.RightChild : parent.LeftChild;

			if (grandparent != null)
			{
				if (parentIsLeftChild)
				{
					grandparent.CutLeftChild();
				}
				else
				{
					grandparent.CutRightChild();
				}
			}

			parent.CutChildren();
			CutChildren();

			if (grandparent != null)
			{
				if (parentIsLeftChild)
				{
					grandparent.SetLeftChild(this);
				}
				else
				{
					grandparent.SetRightChild(this);
				}
			}

			if (isLeftChild)
			{
				SetLeftChild(parent);
				SetRightChild(oppositeChild);
			}
			else
			{
				SetLeftChild(oppositeChild);
				SetRightChild(parent);
			}

			parent.SetLeftChild(leftChild);
			parent.SetRightChild(rightChild);
		}

		public bool SwapWithParentIfPossible(IComparer<T> comparer)
		{
			Assert(Parent != null);
			
			if (comparer.Less(Item, Parent.Item))
			{
				SwapWithParent();
				return true;
			}

			return false;
		}

		public override string ToString() => Pretty(5);

		private void ClearLeftChild() => LeftChild = null;

		private void ClearParent() => Parent = null;

		private void ClearRightChild() => RightChild = null;

		private void CutChildren()
		{
			CutLeftChild();
			CutRightChild();
		}

		private bool IsLeftChildOfParent()
		{
			Assert(Parent != null);
			return Parent.LeftChild == this;
		}

		private string Pretty(int maxDepth)
		{
			if (maxDepth == 0)
			{
				return "(...)";
			}

			string parentString = Parent == null ? "." : Parent.Item.ToString()!;
			string leftString = LeftChild == null ? "." : LeftChild.Pretty(maxDepth - 1);
			string rightString = RightChild == null ? "." : RightChild.Pretty(maxDepth - 1);

			return $"({Item} | P{parentString} : {leftString} {rightString})";
		}
	}

	private readonly QueueWithLinkedList<Node> searchQueue;
	private readonly IComparer<T> comparer;
	
	private Node? root;

	public int Count { get; private set; }

	public T PeekMin
	{
		get
		{
			if (IsEmpty)
			{
				ThrowHelper.ThrowContainerEmpty();
			}
			
			return root!.Item;
		}
	}

	[MemberNotNullWhen(false, nameof(root))]
	private bool IsEmpty => root == null;

	private bool IsSingleton => !IsEmpty && root.LeftChild == null && root.RightChild == null;

	public PriorityTree(IComparer<T> comparer)
	{
		this.comparer = comparer;
		searchQueue = new QueueWithLinkedList<Node>();
	}

	// GetLAstNode is O(n), therefor so is this method
	public T PopMin()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		var min = root!.Item;

		if (IsSingleton)
		{
			return min;
		}
		
		var lastNode = GetLastNode();

		MoveNodeToRoot(lastNode);
		
		Sink(root);
		Count--;
		
		return min;
	}

	// GetFirstNodeWithEmptyChild is O(n), therefor so is this method :-/
	public void Push(T item)
	{
		item.ThrowIfNull();
		
		var node = new Node(item);
		
		if (IsEmpty)
		{
			root = node;
			Count++;
			return;
		}

		var lastNode = GetFirstNodeWithEmptyChild();

		if (lastNode.LeftChild == null)
		{
			lastNode.BindLeftChild(node);
		}
		else
		{
			Assert(lastNode.RightChild == null);
			lastNode.BindRightChild(node);
		}
		
		Swim(node);
		Count++;
	}

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator()
	{
		if (IsEmpty)
		{
			yield break;
		}
		
		var queue = new QueueWithLinkedList<Node>();
		queue.Enqueue(root!);

		while (!queue.IsEmpty)
		{
			var nextNode = queue.Dequeue();
			yield return nextNode.Item;

			if (nextNode.LeftChild != null)
			{
				queue.Enqueue(nextNode.LeftChild);
			}

			if (nextNode.RightChild != null)
			{
				queue.Enqueue(nextNode.RightChild);
			}
		}
	}

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	/// <inheritdoc/>
	public override string ToString() => root!.ToString();
	
	private Node GetFirstNodeWithEmptyChild()
	{
		Assert(!IsEmpty);
		
		searchQueue.Enqueue(root);

		while (!searchQueue.IsEmpty)
		{
			var nextNode = searchQueue.Dequeue();

			if (nextNode.LeftChild == null || nextNode.RightChild == null)
			{
				Assert(nextNode.RightChild == null);
				searchQueue.Clear();
				return nextNode;
			}
			
			if (nextNode.LeftChild != null)
			{
				searchQueue.Enqueue(nextNode.LeftChild);
			}
			
			if (nextNode.RightChild != null)
			{
				searchQueue.Enqueue(nextNode.RightChild);
			}
		}
		
		// Not reachable
		Assert(false);
		return null;
	}

	private Node GetLastNode()
	{
		Assert(!IsEmpty);
		searchQueue.Enqueue(root);

		while (!searchQueue.IsEmpty)
		{
			var nextNode = searchQueue.Dequeue();

			if (searchQueue.IsEmpty)
			{
				if (nextNode.LeftChild == null && nextNode.RightChild == null)
				{
					return nextNode;
				}
			}

			if (nextNode.LeftChild != null)
			{
				searchQueue.Enqueue(nextNode.LeftChild);
			}

			if (nextNode.RightChild != null)
			{
				searchQueue.Enqueue(nextNode.RightChild);
			}
		}
		
		// Not reachable
		Assert(false);
		return null;
	}

	private void MoveNodeToRoot(Node node)
	{
		Assert(!IsEmpty);
		
		if (node.Parent == root)
		{
			var rightChild = root.RightChild;
			if (rightChild == node)
			{
				rightChild = null;
			}
			
			var leftChild = root.LeftChild;
			if (leftChild == node)
			{
				leftChild = null;
			}
			
			node.Parent = null;
			node.SetLeftChild(leftChild);
			node.SetRightChild(rightChild);
			root = node;

			return;
		}

		Assert(node != root);
		
		// Because node is not the root
		Assert(node.Parent != null);
		
		var a = root;
		var b = root.LeftChild;
		var c = root.RightChild;
		var d = node.Parent;
		
		a.CutLeftChild();
		a.CutRightChild();

		if (d.LeftChild == node)
		{
			d.CutLeftChild();
		}
		else
		{
			// If node is a child of the root, we already cut off the children so without the first part the assertion will fail 
			// Assert(d.RightChild == node);
			
			d.CutRightChild();
		}

		node.SetLeftChild(b);
		node.SetRightChild(c);

		root = node;
	}

	// ReSharper disable once CognitiveComplexity
	private void Sink(Node node)
	{
		while (node.LeftChild != null)
		{ // if left child is null so is right child
			if (node.RightChild != null && comparer.Less(node.RightChild.Item, node.LeftChild.Item))
			{
				var rightChild = node.RightChild;
				if (!rightChild.SwapWithParentIfPossible(comparer))
				{
					break;
				}
				
				if (rightChild.Parent == null)
				{
					root = rightChild;
				}
				
				// After the swap node has a new child
			}
			else
			{
				Assert(node.LeftChild != null);

				var leftChild = node.LeftChild;
				if (!node.LeftChild.SwapWithParentIfPossible(comparer))
				{
					break;
				}

				if (leftChild.Parent == null)
				{
					root = leftChild;
				}
				
				// After the swap node has a new child
			}
		}
	}

	private void Swim(Node node)
	{
		while (node.Parent != null && comparer.Less(node.Item, node.Parent.Item))
		{
			node.SwapWithParent();
		}

		if (node.Parent == null)
		{
			root = node;
		}
	}
}
