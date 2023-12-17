namespace AlgorithmsSW.Graph;

using Support;

/// <summary>
/// Represents a graph data structure.
/// </summary>
/// <remarks> Self-loops are supported, but parallel edges are not.
/// </remarks>
public interface IReadOnlyGraph : IEnumerable<(int vertex0, int vertex1)>
{
	/// <summary>
	/// Gets the number of vertices in the graph.
	/// </summary>
	public int VertexCount { get; }
	
	/// <summary>
	/// Gets the number of edges in the graph.
	/// </summary>
	public int EdgeCount { get; }

	/// <summary>
	/// Gets a value indicating whether the graph is empty (no vertices).
	/// </summary>
	public bool IsEmpty => VertexCount == 0;

	/// <summary>
	/// Gets the vertices in the graph.
	/// </summary>
	public IEnumerable<int> Vertices => Enumerable.Range(0, VertexCount); 
	
	internal bool SupportsParallelEdges { get; }
	
	internal bool SupportsSelfLoops { get; }

	/// <summary>
	/// Gets the adjacent vertices of a given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to find adjacents for.</param>
	/// <returns>An enumerable of adjacent vertices.</returns>
	public IEnumerable<int> GetAdjacents(int vertex);
	
	/// <summary>
	/// Creates a human-readable string representation of the graph.
	/// </summary>
	/// <returns>A string representing the graph's structure.</returns>
	public string AsText()
	{
		string NeighborsAsText(int vertex) => GetAdjacents(vertex).AsText(Formatter.Space);
		string VertexAsText(int vertex) => vertex.Describe(NeighborsAsText(vertex));
		
		return nameof(VertexCount).Describe(VertexCount.AsText())
				+ nameof(EdgeCount).Describe(EdgeCount.AsText())
				+ Vertices.Select(VertexAsText).AsText(Formatter.NewLine);
	}
}
