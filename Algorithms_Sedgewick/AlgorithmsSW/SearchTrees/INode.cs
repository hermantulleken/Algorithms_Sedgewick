namespace AlgorithmsSW.SearchTrees;

/// <summary>
/// Represents a node in a search tree.
/// </summary>
/// <typeparam name="T">The type of the item in the node.</typeparam>
public interface INode<T>
{
	/// <summary>
	/// Gets or sets the item in the node.
	/// </summary>
	T Item { get; set; }
}
