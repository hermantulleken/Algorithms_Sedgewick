namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// Represents a weighted graph data structure.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IEdgeWeightedGraph<TWeight> : IReadOnlyEdgeWeightedGraph<TWeight>
{
	/// <summary>
	/// Adds an edge to the graph.
	/// </summary>
	/// <param name="edge">The edge to add.</param>
	void AddEdge(Edge<TWeight> edge);
	
	/// <summary>
	/// Removes an edge from the graph.
	/// </summary>
	/// <param name="edge">The edge to remove.</param>
	bool RemoveEdge(Edge<TWeight> edge);
}