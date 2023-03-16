using static System.Diagnostics.Debug;
using static Algorithms_Sedgewick.Sort.Sort;

namespace Algorithms_Sedgewick.PriorityQueue;

public class PriorityTree<T> where T : IComparable<T>
{
	private sealed class Node
	{
		public T Item;
		public Node LeftChild;
		public Node Parent;
		public Node RightChild;
		
		public void SwapWithParent()
		{
			var parent = Parent;
			var grandParent = parent.Parent;

			if (grandParent != null)
			{
				if (grandParent.LeftChild == parent)
				{
					grandParent.BindLeftChild(this);
				}
				else
				{
					grandParent.BindRightChild(this);
				}
			}
			else
			{
				Parent = null;
			}

			if (parent.LeftChild == this)
			{
				BindLeftChild(parent);
			}
			else
			{
				BindRightChild(parent);
			}
		}
		
		public bool SwapWithParentIfPossible()
		{
			Assert(Parent != null);
			
			if (Less(Item, Parent.Item))
			{
				SwapWithParent();
				return true;
			}

			return false;
		}
		
		private void ClearParent()
		{
			Parent = null;
		}
		
		public void BindLeftChild(Node child)
		{
			LeftChild?.ClearParent();
			child.Parent = this;
			LeftChild = child;
		}

		public void BindRightChild(Node child)
		{
			RightChild?.ClearParent();
			child.Parent = this;
			RightChild = child;
		}
	}

	private bool IsEmpty => root == null;

	private bool IsSingleton => !IsEmpty && root.LeftChild == null && root.RightChild == null;

	private Node root;

	public T Pop()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		var min = root.Item;

		if (IsSingleton)
		{
			return min;
		}
		var lastNode = GetLastNode();

		lastNode.BindLeftChild(root.LeftChild);
		lastNode.BindRightChild(root.RightChild);
		
		root = lastNode;
        Sink(root);
		
		return min;
	}

	public void Push(T item)
	{
		var node = new Node() { Item = item };
		
		if (IsEmpty)
		{
			root = node;
			return;
		}

		var lastNode = GetLastNode();

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
	}
	
	private static void Sink(Node node)
	{
		while (node.LeftChild != null) //if left child is null so is right child
		{
			if (node.RightChild != null)
			{
				if (!node.RightChild.SwapWithParentIfPossible())
				{
					break;
				}
				//After the swap node has a new children
			}
			else
			{
				Assert(node.LeftChild != null);

				if (!node.LeftChild.SwapWithParentIfPossible())
				{
					break;
				}
				//After the swap node has a new children
			}
		}
	}

	

	private static void Swim(Node node)
	{
		while (node.Parent != null && Less(node.Item, node.Parent.Item))
		{
			node.SwapWithParent();
			node = node.Parent;
		}
	}

	private Node GetLastNode()
	{
		var node = root;

		while (node.LeftChild != null)
		{
			node = node.RightChild ?? node.LeftChild;
		}

		return node;
	}
}
