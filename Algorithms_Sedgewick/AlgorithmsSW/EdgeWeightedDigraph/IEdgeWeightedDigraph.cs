namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;

/// <summary>
/// Represents a directed edge weighted graph data structure.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IEdgeWeightedDigraph<TWeight>
	: IReadOnlyEdgeWeightedDigraph<TWeight>
	//where TWeight : IComparable<TWeight>, IFloatingPoint<TWeight>
{
	/// <summary>
	/// Adds an edge to the graph.
	/// </summary>
	/// <param name="edge">The edge to add.</param>
	void AddEdge(DirectedEdge<TWeight> edge);

	/// <summary>
	/// Removes an edge from the graph.
	/// </summary>
	/// <param name="edge">The edge to remove.</param>
	void RemoveEdge(DirectedEdge<TWeight> edge);
}
