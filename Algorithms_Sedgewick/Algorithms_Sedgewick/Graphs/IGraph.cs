namespace Algorithms_Sedgewick.Graphs;

/// <summary>
/// Represents a graph data structure.
/// </summary>
/// <remarks> Self-loops are supported, but parallel edges are not.
/// </remarks>
public interface IGraph : IEnumerable<(int vertex0, int vertex2)>
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
	IEnumerable<int> Vertices => Enumerable.Range(0, VertexCount); 
	
	/// <summary>
	/// Adds an edge between two vertices in the graph.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge.</param>
	/// <param name="vertex1">The second vertex of the edge.</param>
	/// <remarks>If the edge exists, calling this method has no effect.</remarks>
	public void AddEdge(int vertex0, int vertex1);

	/// <summary>
	/// Gets the adjacent vertices of a given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to find adjacents for.</param>
	/// <returns>An enumerable of adjacent vertices.</returns>
	IEnumerable<int> GetAdjacents(int vertex);
	
	/// <summary>
	/// Creates a human-readable string representation of the graph.
	/// </summary>
	/// <returns>A string representing the graph's structure.</returns>
	public string AsText()
	{
		string NeighborsAsText(int vertex) => GetAdjacents(vertex).AsText(Textify.Space);
		string VertexAsText(int vertex) => vertex.Describe(NeighborsAsText(vertex));
		
		return nameof(VertexCount).Describe(VertexCount.AsText())
				+ nameof(EdgeCount).Describe(EdgeCount.AsText())
				+ Vertices.Select(VertexAsText).AsText(Textify.NewLine);
	}
}
