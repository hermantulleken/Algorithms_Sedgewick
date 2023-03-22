namespace Algorithms_Sedgewick.SearchTrees;

using System.Diagnostics.CodeAnalysis;
using Stack;
using static System.Diagnostics.Debug;
using static Support.Tools;

public class BinarySearchTree<T> 
{
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = DataTransferStruct)]
	public class Node
	{
		public T Item;
		public Node LeftChild = null;
		public Node RightChild = null;
	}

	private readonly IComparer<T> comparer;

	/*
		This is a reusable stack that can be used by any method that needs one:
			- Assert the stack is empty at the start of your method.
			- Always clear it before returning on all paths. 
			- If your code can throw exceptions, use a try-finally to clear the stack. 
	*/
	private readonly IStack<Node> stack;

	private Node root;

	public int Count { get; private set; }

	public bool IsEmpty => root == null;

	public bool IsSingleton => Count == 1;

	// Laziness here is useful for implementing other methods 
	public IEnumerable<Node> NodesInOrder
	{
		get
		{
			Assert(stack.IsEmpty);
			
			var node = root;

			while (node != null || stack.Count > 0)
			{
				// Traverse left subtree until we reach the leftmost node
				while (node != null)
				{
					stack.Push(node);
					node = node.LeftChild;
				}

				// Pop the next node from the stack and yield return it
				node = stack.Pop();
				yield return node;

				// Traverse right subtree
				node = node.RightChild;
			}
			
			stack.Clear();
		}
	}

	public Node NextNodeInOrder(Node node)
	{
		var nodeAndNext = NodesInOrder.SkipWhile(n => n != node).Take(2);

		return nodeAndNext.Count() == 2 
			? nodeAndNext.Last() 
			: null; // The given node is last
	}

	public void Remove(T item)
	{
		root = RemoveNode(root, item);
	}
	
	private Node RemoveNode(Node rootNode, T item)
	{
		// Base case: tree is empty or key is not found
		if (rootNode == null)
		{
			return rootNode;
		}

		switch (comparer.Compare(item, rootNode.Item))
		{
			// Recur down the tree to find the node to delete
			case < 0:
				rootNode.LeftChild = RemoveNode(rootNode.LeftChild, item);
				break;
			
			case > 0:
				rootNode.RightChild = RemoveNode(rootNode.RightChild, item);
				break;
			
			// key matches the key of the current node
			default:
			{
				// Case 1: Node has no children or one child
				if (rootNode.LeftChild == null)
				{
					return rootNode.RightChild;
				}
				else if (rootNode.RightChild == null)
				{
					return rootNode.LeftChild;
				}

				// Case 2: Node has two children
				var smallestNodeInRightSubtree = GetMinNode(rootNode.RightChild);
				rootNode.Item = smallestNodeInRightSubtree.Item;
				rootNode.RightChild = RemoveNode(rootNode.RightChild, smallestNodeInRightSubtree.Item);
				break;
			}
		}

		return rootNode;
	}

	private Node FindSmallestNode(Node rootNode)
	{
		Node currentNode = rootNode;
		while (currentNode.LeftChild != null)
		{
			currentNode = currentNode.LeftChild;
		}
		return currentNode;
	}


	
	public BinarySearchTree(IComparer<T> comparer, int initialCapacity = Collection.DefaultCapacity)
	{
		this.comparer = comparer;
		Count = 0;
		stack = new StackWithResizeableArray<Node>(initialCapacity);
	}

	public void Add(T item)
	{
		if (IsEmpty)
		{
			root = new Node { Item = item };
		}
		else
		{
			AddToNode(root, item);
		}

		Count++;
	}

	public void AddToNode(Node node, T item)
	{
		while (true)
		{
			bool addToLeft = comparer.LessOrEqual(item, node.Item);
			var childNode = addToLeft ? node.LeftChild : node.RightChild;

			if (childNode == null)
			{
				if (addToLeft)
				{
					node.LeftChild = new Node { Item = item };
				}
				else
				{
					node.RightChild = new Node { Item = item };
				}
				
				return;
			}

			node = childNode;
		}
	}

	public int CountNodesSmallerThan(T item)
	{
		bool SmallerThanItem(Node node) =>
			comparer.Less(node.Item, item);
				
		return NodesInOrder
			.TakeWhile(SmallerThanItem)
			.Count();
	}
	
	public bool TryFindNode(T item, out Node node)
		=> TryFindAtNode(root, item, out node);

	public Node GetMaxNode()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		var node = root;

		return GetMaxNode(node);
	}

	public Node GetMinNode()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		var node = root;

		return GetMinNode(node);
	}

	// Can return null
	public Node LargestKeyLessThanOrEqualTo(T item)
	{
		if (IsEmpty)
		{
			return null;
		}
		
		if (IsSingleton)
		{
			return comparer.Less(item, root.Item) ? null : root;
		}
		
		return NodesInOrder
			.Buffer2()
			.FirstOrDefault(pair => comparer.Less(pair.Last.Item, item))
			?.First; // When all nodes are larger than item, will return null
	}

	private static Node GetMaxNode(Node node)
	{
		while (node.RightChild != null)
		{
			node = node.RightChild;
		}

		return node;
	}

	private static Node GetMinNode(Node node)
	{
		while (node.LeftChild != null)
		{
			node = node.LeftChild;
		}

		return node;
	}
	
	private bool TryFindAtNode(Node node, T item, out Node result)
	{
		result = null;
		while (node != null)
		{
			switch (comparer.Compare(node.Item, item))
			{
				case < 0:
					node = node.LeftChild;
					break;
				case > 0:
					node = node.RightChild;
					break;
				default:
					result = node;
					return true;
			}
		}
		
		return false;
	}
}
