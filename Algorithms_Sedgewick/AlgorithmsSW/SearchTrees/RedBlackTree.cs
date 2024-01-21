namespace AlgorithmsSW.SearchTrees;

using System.Diagnostics.CodeAnalysis;
using Queue;
using Stack;
using static System.Diagnostics.Debug;
using static Diagnostics;
using static Tools;

public class RedBlackTree<T> : IBinarySearchTree<T>
{
	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = DataTransferStruct)]
	public class Node : INode<T>
	{
		public int Count;
		
		public Node? LeftChild = null;
		
		public Node? RightChild = null;

		private bool isRed; // Black otherwise 
		
		public T Item { get; set; }
		
		public bool IsLeaf => LeftChild == null && RightChild == null;

		public Node(T item, int count, bool isRed = false, Node? leftChild = null, Node? rightRight = null)
		{
			Item = item;
			LeftChild = leftChild;
			RightChild = rightRight;
			this.isRed = isRed;
			Count = count;
		}

		public static int GetCount(Node? node) 
			=> node?.Count ?? 0;
		
		public static bool IsNotNullAndRed([NotNullWhen(true)] Node? node)
			=> node is { isRed: true };

		public Node RotateLeft()
		{
			Assert(RightChild != null);
			var rightChild = RightChild;
			RightChild = rightChild.LeftChild;
			rightChild.LeftChild = this;
			rightChild.isRed = isRed;
			isRed = true;
			rightChild.Count = Count;
			Count = 1 + GetCount(LeftChild) + GetCount(RightChild);
			
			return rightChild;
		}

		public Node RotateRight()
		{
			Assert(LeftChild != null);
			
			var leftChild = LeftChild;
			LeftChild = leftChild.RightChild;
			leftChild.RightChild = this;
			leftChild.isRed = isRed;
			isRed = true;
			leftChild.Count = Count;
			Count = 1 + GetCount(LeftChild) + GetCount(RightChild);
			return leftChild;
		}

		public void SetBlack() => isRed = false;

		public void SetRed()
		{
			isRed = true;
		}

		public void SetRedAndChildrenBlack()
		{
			Assert(LeftChild != null);
			Assert(RightChild != null);
			
			isRed = true;
			LeftChild.isRed = false;
			RightChild.isRed = false;
		}

		// ReSharper disable once StaticMemberInGenericType
		private static int idCounter = 0;
	}

	private readonly IComparer<T> comparer;

	private Node? root;

	private int version = 0;

	public int Count => Node.GetCount(root);

	[MemberNotNullWhen(false, nameof(root))]
	public bool IsEmpty => root == null;

	public bool IsSingleton => Count == 1;
	
	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesInOrder => NodesInOrder;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesLevelOrder => NodesLevelOrder;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesPostOrder => NodesPostOrder;

	IEnumerable<INode<T>> IBinarySearchTree<T>.NodesPreOrder => NodesPreOrder;

	INode<T>? IBinarySearchTree<T>.Root => Root;

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

			var stack = new Stack<Node>();
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

	public RedBlackTree(IComparer<T> comparer)
	{
		this.comparer = comparer;
	}

	public void Add(T item)
	{ // Search for key. Update value if found; grow table if new.
		root = AddToNode(root, item);
		root.SetBlack();
	}

	public int CountNodesSmallerThan(T item)
	{
		return NodesInOrder
			.TakeWhile(SmallerThanItem)
			.Count();

		bool SmallerThanItem(Node node) => comparer.Less(node.Item, item);
	}

	public void Remove(T key) 
	{
		if (key == null)
		{
			throw ThrowHelper.ContainerEmptyException;
		}

		if (!TryFindNode(key, out var _))
		{
			return;
		}
		
		Assert(!IsEmpty); // Since we found the key 

		// if both children of root are black, set root to red
		if (!Node.IsNotNullAndRed(root.LeftChild) && !Node.IsNotNullAndRed(root.RightChild))
		{
			root.SetRed();
		}

		root = Remove(root, key);
		if (!IsEmpty)
		{
			root.SetBlack();
		}
		
		// assert check();
	}

	public Node RemoveMaxNode() 
	{
		if (IsEmpty)
		{
			throw ThrowHelper.ContainerEmptyException;
		}

		// TODO: This is temporary, ideally the remove node should return the max node directly
		var maxNode = GetMaxNode();
		
		// Question: Does the node get changed ny the code below?
		
		// if both children of root are black, set root to red
		if (!Node.IsNotNullAndRed(root.LeftChild) && !Node.IsNotNullAndRed(root.RightChild))
		{
			root.SetRed();
		}

		root = RemoveMaxNode(root);
		if (!IsEmpty)
		{
			root.SetBlack();
		}

		return maxNode;

		// assert check();
	}

	public Node RemoveMinNode()
	{
		if (IsEmpty)
		{
			throw ThrowHelper.ContainerEmptyException;
		}
		
		// TODO: This is temporary, ideally the remove node should return the max node directly
		var minNode = GetMaxNode();
		
		// Question: Does the node get changed ny the code below?

		// if both children of root are black, set root to red
		if (!Node.IsNotNullAndRed(root.LeftChild) && !Node.IsNotNullAndRed(root.RightChild))
		{
			root.SetRed();
		}

		root = RemoveMinNode(root);
		if (!IsEmpty)
		{
			root.SetBlack();
		}

		return minNode;
		
		// assert check();
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

	public Node? NextNodeInOrder(Node node)
	{
		var nodeAndNext = NodesInOrder.SkipWhile(n => n != node).Take(2);

		// ReSharper disable PossibleMultipleEnumeration
		return nodeAndNext.Count() == 2
			? nodeAndNext.Last() 
			: null; // The given node is last
		
		// ReSharper restore PossibleMultipleEnumeration
	}

	public IEnumerable<Node> Range(T start, T end) =>
		NodesInOrder
			.SkipWhile(node => comparer.Less(node.Item, start))
			.TakeWhile(node => comparer.Less(node.Item, end));
	
	IEnumerable<INode<T>> IBinarySearchTree<T>.Range(T start, T end) => Range(start, end);
	
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

	private static Node GetMaxNode(Node node)
	{
		while (node.RightChild != null)
		{
			node = node.RightChild;
		}

		return node;
	}
	
#pragma warning disable SA1202
	INode<T> IBinarySearchTree<T>.GetMaxNode() => GetMaxNode();

	INode<T> IBinarySearchTree<T>.GetMinNode() => GetMinNode();

	INode<T>? IBinarySearchTree<T>.LargestKeyLessThanOrEqualTo(T key) => LargestKeyLessThanOrEqualTo(key);

	// What if the item is not found?
	INode<T> IBinarySearchTree<T>.RemoveMaxNode() => RemoveMaxNode();

	INode<T> IBinarySearchTree<T>.RemoveMinNode() => RemoveMinNode();
	
	INode<T>? IBinarySearchTree<T>.SmallestKeyGreaterThanOrEqualTo(T key) => SmallestKeyGreaterThanOrEqualTo(key);
#pragma warning restore SA1202
	
	private static Node GetMinNode(Node node)
	{
		while (node.LeftChild != null)
		{
			node = node.LeftChild;
		}

		return node;
	}

	private Node AddToNode(Node? node, T item)
	{
		if (node == null) 
		{
			// Do standard insert, with red link to parent.
			return new Node(item, 1, true);
		}
		
		int cmp = comparer.Compare(item, node.Item);

		switch (cmp)
		{
			case < 0:
				node.LeftChild = AddToNode(node.LeftChild, item);
				break;
			case > 0:
				node.RightChild = AddToNode(node.RightChild, item);
				break;
			default:
				node.Item = item;
				break;
		}
		
		if (Node.IsNotNullAndRed(node.RightChild) && !Node.IsNotNullAndRed(node.LeftChild))
		{
			node = node.RotateLeft();
		}

		if (Node.IsNotNullAndRed(node.LeftChild) && Node.IsNotNullAndRed(node.LeftChild?.LeftChild))
		{
			node = node.RotateRight();
		}

		if (Node.IsNotNullAndRed(node.LeftChild) && Node.IsNotNullAndRed(node.RightChild))
		{
			node.SetRedAndChildrenBlack();
		}
		
		node.Count = Node.GetCount(node.LeftChild) + Node.GetCount(node.RightChild) + 1;
		return node;
	}

	private Node RestoreRedBlackTreeInvariant(Node node) 
	{
		// assert (node != null);

		if (Node.IsNotNullAndRed(node.RightChild) && !Node.IsNotNullAndRed(node.LeftChild))
		{
			node = node.RotateLeft();
		}

		if (Node.IsNotNullAndRed(node.LeftChild) && Node.IsNotNullAndRed(node.LeftChild?.LeftChild))
		{
			node = node.RotateRight();
		}

		if (Node.IsNotNullAndRed(node.LeftChild) && Node.IsNotNullAndRed(node.RightChild))
		{
			node.SetRedAndChildrenBlack();
		}

		node.Count = Node.GetCount(node.LeftChild) + Node.GetCount(node.RightChild) + 1;
		return node;
	}

	// delete the key-value pair with the given key rooted at h
	private Node? Remove(Node node, T key)
	{
		if (comparer.Compare(key, node.Item) < 0)  
		{
			Question(node.LeftChild != null);
			
			if (!Node.IsNotNullAndRed(node.LeftChild) && !Node.IsNotNullAndRed(node.LeftChild.LeftChild))
			{
				node = MoveRedLeft(node);
			}
			
			Question(node.LeftChild != null);
			
			node.LeftChild = Remove(node.LeftChild, key);
		}
		else
		{
			if (Node.IsNotNullAndRed(node.LeftChild))
			{
				node = node.RotateRight();
			}

			if (comparer.Compare(key, node.Item) == 0 && node.RightChild == null)
			{
				return null;
			}
			
			// Suppose node.RightChild is null
			// Then IsNotNullAndRed(node.RightChild) will be false
			// So !Node.IsNotNullAndRed(node.RightChild) will be true, 
			// and so !Node.IsNotNullAndRed(node.RightChild.LeftChild) will be evaluated
			// and so node.RightChild.LeftChild will null ref
			
			Question(node.RightChild != null);

			if (!Node.IsNotNullAndRed(node.RightChild) && !Node.IsNotNullAndRed(node.RightChild.LeftChild))
			{
				node = MoveRedRight(node);
			}
			
			Question(node.RightChild != null);
			
			if (comparer.Compare(key, node.Item) == 0)
			{
				Node minNode = GetMinNode(node.RightChild);
				node.Item = minNode.Item;
				node.RightChild = RemoveMinNode(node.RightChild);
			}
			else
			{
				node.RightChild = Remove(node.RightChild, key);
			}
		}
		
		return RestoreRedBlackTreeInvariant(node);
	}

	// delete the key-value pair with the maximum key rooted at h
	private Node? RemoveMaxNode(Node node) 
	{
		if (Node.IsNotNullAndRed(node.LeftChild))
		{
			node = node.RotateRight();
		}

		if (node.RightChild == null)
		{
			return null;
		}

		if (!Node.IsNotNullAndRed(node.RightChild) && !Node.IsNotNullAndRed(node.RightChild.LeftChild))
		{
			node = MoveRedRight(node);
		}
		
		Question(node.RightChild != null);

		node.RightChild = RemoveMaxNode(node.RightChild);

		return RestoreRedBlackTreeInvariant(node);
	}

	// delete the key-value pair with the minimum key rooted at h
	private Node? RemoveMinNode(Node node)
	{
		if (node.LeftChild == null)
		{
			return null;
		}

		if (!Node.IsNotNullAndRed(node.LeftChild) && !Node.IsNotNullAndRed(node.LeftChild.LeftChild))
		{
			node = MoveRedLeft(node);
		}
		
		Question(node.LeftChild != null);

		node.LeftChild = RemoveMinNode(node.LeftChild);
		return RestoreRedBlackTreeInvariant(node);
	}

	// Assuming that h is red and both h.LeftChild and h.LeftChild.LeftChild
	// are black, make h.LeftChild or one of its children red.
	private Node MoveRedLeft(Node node) 
	{
		// assert (h != null);
		// assert Node.IsRed(h) && !Node.IsRed(h.LeftChild) && !Node.IsRed(h.LeftChild.LeftChild);

		node.SetRedAndChildrenBlack();
		
		Assert(node.RightChild != null); // SetRedAndChildrenBlack already assumes this!

		if (!Node.IsNotNullAndRed(node.RightChild.LeftChild))
		{
			return node;
		}
		
		node.RightChild = node.RightChild.RotateRight();
		node = node.RotateLeft();
		node.SetRedAndChildrenBlack();

		return node;
	}

	// Assuming that h is red and both h.RightChild and h.RightChild.LeftChild
	// are black, make h.RightChild or one of its children red.
	private Node MoveRedRight(Node node) 
	{
		// assert (h != null);
		// assert Node.IsRed(h) && !Node.IsRed(h.RightChild) && !Node.IsRed(h.RightChild.LeftChild);
		node.SetRedAndChildrenBlack();

		Assert(node.LeftChild != null); // SetRedAndChildrenBlack already assumes it.
		
		if (Node.IsNotNullAndRed(node.LeftChild.LeftChild))
		{
			node = node.RotateRight();
			node.SetRedAndChildrenBlack();
		}
		
		return node;
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
