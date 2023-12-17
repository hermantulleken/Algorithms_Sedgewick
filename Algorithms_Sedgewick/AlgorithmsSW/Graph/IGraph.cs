using Support;

namespace AlgorithmsSW.Graph;

/// <summary>
/// Represents a graph data structure.
/// </summary>
/// <remarks> Self-loops are supported, but parallel edges are not.
/// </remarks>
public interface IGraph : IReadOnlyGraph
{
	/// <summary>
	/// Adds an edge between two vertices in the graph.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge to add.</param>
	/// <param name="vertex1">The second vertex of the edge to add.</param>
	public void AddEdge(int vertex0, int vertex1);

	/// <summary>
	/// Removes an edge between two vertices in the graph.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge to remove.</param>
	/// <param name="vertex1">The second vertex of the edge to remove.</param>
	/// <returns><see langword="true"/> if the edge was removed, <see langword="false"/> otherwise.</returns>
	public bool RemoveEdge(int vertex0, int vertex1);
}
