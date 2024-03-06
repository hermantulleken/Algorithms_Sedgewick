namespace AlgorithmsSW.Digraph;

using System.Runtime.CompilerServices;

public interface IReadOnlyDigraph : IEnumerable<(int vertex0, int vertex1)>
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
	/// Gets the vertices in the graph.
	/// </summary>
	public IEnumerable<int> Vertexes => Enumerable.Range(0, VertexCount); 
	
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
				+ Vertexes.Select(VertexAsText).AsText(Formatter.NewLine);
	}

	bool AreAdjacent(int item1, int item2) => GetAdjacents(item1).Contains(item2);
	
	public IEnumerable<(int source, int target)> Edges { get; }
	
	bool SupportsParallelEdges { get; }
	
	bool SupportsSelfLoops { get; }
	
	// TODO: does this belong here?
	internal void ValidateVertex(int source, [CallerArgumentExpression(nameof(source))] string? sourceArgName = null)
	{
		if (source < 0 || source >= VertexCount)
		{
			throw new ArgumentOutOfRangeException(sourceArgName);
		}
	}
}
