using System.Diagnostics.CodeAnalysis;

namespace AlgorithmsSW.SearchTrees;

public interface IBinarySearchTree<T>
{
	bool IsEmpty { get; }
	
	int Count { get; }
	
	INode<T>? Root { get; }

	void Add(T item);

	int CountNodesSmallerThan(T item);
	
	INode<T> GetMaxNode();
	
	INode<T> GetMinNode();
	
	int Height();
	
	INode<T>? LargestKeyLessThanOrEqualTo(T key);
	
	IEnumerable<INode<T>> Range(T start, T end);
	
	void Remove(T item);
	
	INode<T> RemoveMaxNode();
	
	INode<T> RemoveMinNode();
	
	INode<T>? SmallestKeyGreaterThanOrEqualTo(T key);
	
	bool TryFindNode(T item, [MaybeNullWhen(false)] out INode<T> node);
	
	IEnumerable<INode<T>> NodesInOrder { get; }
	
	IEnumerable<INode<T>> NodesLevelOrder { get; }
	
	IEnumerable<INode<T>> NodesPostOrder { get; }
	
	IEnumerable<INode<T>> NodesPreOrder { get; }
}
