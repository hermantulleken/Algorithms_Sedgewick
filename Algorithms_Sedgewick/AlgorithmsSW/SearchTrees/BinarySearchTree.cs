namespace AlgorithmsSW.SearchTrees;

using System.Diagnostics.CodeAnalysis;
using Queue;
using Stack;
using static System.Diagnostics.Debug;
using static Support.Tools;

public static class BinarySearchTree
{
	public static IBinarySearchTree<KeyValuePair<TKey, TValue>> Plain<TKey, TValue>(IComparer<KeyValuePair<TKey, TValue>> comparer)
	{
		return new BinarySearchTree<KeyValuePair<TKey, TValue>>(comparer);
	}
	
	public static IBinarySearchTree<KeyValuePair<TKey, TValue>> RedBlack<TKey, TValue>(IComparer<KeyValuePair<TKey, TValue>> comparer)
	{
		return new RedBlackTree<KeyValuePair<TKey, TValue>>(comparer);
	}
}

public class BinarySearchTree<T> : IBinarySearchTree<T>
{
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = DataTransferStruct)]
	public class Node : INode<T>
	{
		public Node? LeftChild = null;
		public Node? RightChild = null;

		public bool IsLeaf => LeftChild == null && RightChild == null;

		public T Item { get; set; }

		public Node(T item, Node? leftChild = null, Node? rightRight = null)
		{
			Item = item;
			LeftChild = leftChild;
			RightChild = rightRight;
		}
	}

	private readonly IComparer<T> comparer;

	private Node? root;

	private int version = 0;

	public int Count { get; private set; } = 0;

	[MemberNotNullWhen(false, nameof(root))]
	public bool IsEmpty => root == null;

	public bool IsSingleton => Count == 1;

	// Laziness here is useful for implementing other methods 
	public IEnumerable<Node> NodesInOrder
	{
		get
		{
			IStack<Node> stack = new StackWithResizeableArray<Node>(Count);
			Assert(stack.IsEmpty);

			int versionAtStartOfIteration = version;
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
				ValidateVersion(versionAtStartOfIteration);
				
				yield return node;

				// Traverse right subtree
				node = node.RightChild;
			}
			
			stack.Clear();
		}
	}

	public IEnumerable<Node> NodesLevelOrder
	{
		get
		{
			if (IsEmpty)
			{
				yield break;
			}

			int versionAtStartOfIteration = version;
			var queue = new QueueWithLinkedList<Node>();
			
			queue.Enqueue(root!);

			while (!queue.IsEmpty)
			{
				var node = queue.Dequeue();
				
				if (node.LeftChild != null)
				{
					queue.Enqueue(node.LeftChild);
				}

				if (node.RightChild != null)
				{
					queue.Enqueue(node.RightChild);
				}
				
				ValidateVersion(versionAtStartOfIteration);
				yield return node;
			}
		}
	}

	public IEnumerable<Node> NodesPostOrder
	{
		get
		{
			if (IsEmpty)
			{
				yield break;
			}

			int versionAtStartOfIteration = version;
			var stack1 = new StackWithResizeableArray<Node>();
			var stack2 = new StackWithResizeableArray<Node>();

			stack1.Push(root!);

			while (stack1.Count > 0)
			{
				Node currentNode = stack1.Pop();
				stack2.Push(currentNode);

				if (currentNode.LeftChild != null)
				{
					stack1.Push(currentNode.LeftChild);
				}

				if (currentNode.RightChild != null)
				{
					stack1.Push(currentNode.RightChild);
				}
			}

			while (stack2.Count > 0)
			{
				ValidateVersion(versionAtStartOfIteration);
				yield return stack2.Pop();
			}
		}
	}

	public IEnumerable<Node> NodesPreOrder
	{
		get
		{
			if (IsEmpty)
			{
				yield break;
			}

			int versionAtStartOfIteration = version;

			Stack<Node> stack = new Stack<Node>();
			stack.Push(root!);

			while (stack.Count > 0)
			{
				Node currentNode = stack.Pop();
				ValidateVersion(versionAtStartOfIteration);
				
				yield return currentNode;

				if (currentNode.RightChild != null)
				{
					stack.Push(currentNode.RightChild);
				}

				if (currentNode.LeftChild != null)
				{
					stack.Push(currentNode.LeftChild);
				}
			}
		}
	}

	public Node? Root => root;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesInOrder => NodesInOrder;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesLevelOrder => NodesLevelOrder;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesPostOrder => NodesPostOrder;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesPreOrder => NodesPreOrder;

	INode<T>? IBinarySearchTree<T>.Root => Root;

	public BinarySearchTree(IComparer<T> comparer)
	{
		this.comparer = comparer;
	}

	public void Add(T item)
	{
		if (IsEmpty)
		{
			root = new Node(item);
		}
		else
		{
			AddToNode(root!, item);
		}

		Count++;
		version++;
	}

	public int CountNodesSmallerThan(T item)
	{
		bool SmallerThanItem(Node node) =>
			comparer.Less(node.Item, item);
				
		return NodesInOrder
			.TakeWhile(SmallerThanItem)
			.Count();
	}

	public Node GetMaxNode()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		var node = root!;

		return GetMaxNode(node);
	}

	public Node GetMinNode()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		var node = root!;

		return GetMinNode(node);
	}

	public int Height()
	{
		if (root == null)
		{
			return 0;
		}

		int height = 0;
		var queue = new Queue<Node>();
		queue.Enqueue(root);

		while (queue.Count > 0)
		{
			int levelNodeCount = queue.Count;
			height++;

			while (levelNodeCount > 0)
			{
				var currentNode = queue.Dequeue();

				if (currentNode.LeftChild != null)
				{
					queue.Enqueue(currentNode.LeftChild);
				}

				if (currentNode.RightChild != null)
				{
					queue.Enqueue(currentNode.RightChild);
				}

				levelNodeCount--;
			}
		}

		return height;
	}

	public Node? LargestKeyLessThanOrEqualTo(T key) 
		=> NodesInOrder
			.TakeWhile(node => comparer.LessOrEqual(node.Item, key))
			.LastOrDefault();

	public IEnumerable<Node> Range(T start, T end) =>
		NodesInOrder
			.SkipWhile(node => comparer.Less(node.Item, start))
			.TakeWhile(node => comparer.Less(node.Item, end));

	IEnumerable<INode<T>> IBinarySearchTree<T>.Range(T start, T end) => Range(start, end);

	public void Remove(T item)
	{
		root = RemoveNode(root, item);
		Count--;
		version++;
	}

	// TODO: The null forgive here may be wrong
	// GetMaxNode throws if IsEmpty, but what if the item is not found?
	public Node RemoveMaxNode() => RemoveNode(root, GetMaxNode().Item)!;

	public Node RemoveMinNode()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
		
		Node? parent = null;
		Node child = root!;

		while (child.LeftChild != null)
		{
			parent = child;
			child = child.LeftChild;
		}

		Node removedNode = child;

		if (parent == null)
		{
			// The root node was the smallest node
			root = root!.RightChild;
		}
		else
		{
			parent.LeftChild = child.RightChild;
		}

		return removedNode;
	}

	public Node? SmallestKeyGreaterThanOrEqualTo(T key) 
		=> NodesInOrder
			.SkipWhile(node => comparer.Less(node.Item, key))
			.FirstOrDefault();

	public bool TryFindNode(T item, [MaybeNullWhen(false)] out INode<T> node)
	{
		bool found = TryFindAtNode(root, item, out var theNode);
		node = theNode;
		return found;
	}

	INode<T> IBinarySearchTree<T>.GetMaxNode() => GetMaxNode();

	INode<T> IBinarySearchTree<T>.GetMinNode() => GetMinNode();

	INode<T>? IBinarySearchTree<T>.LargestKeyLessThanOrEqualTo(T key) => LargestKeyLessThanOrEqualTo(key);

	// What if the item is not found?
	INode<T> IBinarySearchTree<T>.RemoveMaxNode() => RemoveMaxNode();

	INode<T> IBinarySearchTree<T>.RemoveMinNode() => RemoveMinNode();
	
	INode<T>? IBinarySearchTree<T>.SmallestKeyGreaterThanOrEqualTo(T key) => SmallestKeyGreaterThanOrEqualTo(key);

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

	private void AddToNode(Node node, T item)
	{
		while (true)
		{
			bool addToLeft = comparer.LessOrEqual(item, node.Item);
			var childNode = addToLeft ? node.LeftChild : node.RightChild;

			if (childNode == null)
			{
				if (addToLeft)
				{
					node.LeftChild = new Node(item);
				}
				else
				{
					node.RightChild = new Node(item);
				}
				
				return;
			}

			node = childNode;
		}
	}

	private Node? RemoveNode(Node? rootNode, T item)
	{
		// Base case: tree is empty or key is not found
		if (rootNode == null)
		{
			return null;
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
				// Case 1: Node has no children or one child
				if (rootNode.LeftChild == null)
				{
					return rootNode.RightChild;
				}
				
				if (rootNode.RightChild == null)
				{
					return rootNode.LeftChild;
				}

				// Case 2: Node has two children
				var smallestNodeInRightSubtree = GetMinNode(rootNode.RightChild);
				rootNode.Item = smallestNodeInRightSubtree.Item;
				rootNode.RightChild = RemoveNode(rootNode.RightChild, smallestNodeInRightSubtree.Item);
				break;
		}

		return rootNode;
	}

	private bool TryFindAtNode(Node? node, T item, out Node? result)
	{
		result = null;
		while (node != null)
		{
			switch (comparer.Compare(item, node.Item))
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

	private void ValidateVersion(int versionAtStartOfIteration)
	{
		if (version != versionAtStartOfIteration)
		{
			ThrowHelper.ThrowIteratingOverModifiedContainer();
		}
	}
}
