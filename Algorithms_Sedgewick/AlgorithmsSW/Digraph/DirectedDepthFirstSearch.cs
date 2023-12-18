namespace AlgorithmsSW.Digraph;

/// <summary>
/// Provides and algorithm for finding all vertices reachable from a given vertex or set of vertices in a directed
/// graph.
/// </summary>
public class DirectedDepthFirstSearch
{
	private readonly bool[] marked;
	
	public bool IsConnectedToSources { get; }
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedDepthFirstSearch"/> class.
	/// </summary>
	/// <param name="digraph">The graph to search.</param>
	/// <param name="startVertex">The vertex to start the search from.</param>
	public DirectedDepthFirstSearch(IDigraph digraph, int startVertex)
	{
		digraph.ThrowIfNull();
		
		marked = new bool[digraph.VertexCount];
		Search(digraph, startVertex);
		IsConnectedToSources = marked.All(x => x);
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedDepthFirstSearch"/> class.
	/// </summary>
	/// <param name="digraph">The graph to search.</param>
	/// <param name="startVertexes">The vertices to start the search from.</param>
	public DirectedDepthFirstSearch(IDigraph digraph, IEnumerable<int> startVertexes)
	{
		digraph.ThrowIfNull();
		startVertexes.ThrowIfNull();
		
		marked = new bool[digraph.VertexCount];
		
		foreach (int vertex in startVertexes)
		{
			if (!marked[vertex])
			{
				Search(digraph, vertex);
			}
		}
		
		IsConnectedToSources = marked.All(x => x);
	}

	/// <summary>
	/// Checks whether a given vertex is reachable from the start vertex or vertices.
	/// </summary>
	/// <param name="vertex">The vertex to check.</param>
	public bool Reachable(int vertex) => marked[vertex];
	
	private void Search(IDigraph digraph, int vertex)
	{
		marked[vertex] = true;
		
		foreach (int adjacent in digraph.GetAdjacents(vertex))
		{
			if (!marked[adjacent])
			{
				Search(digraph, adjacent);
			}
		}
	}
}
