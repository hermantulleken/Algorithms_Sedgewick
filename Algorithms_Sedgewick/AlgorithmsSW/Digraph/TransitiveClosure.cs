namespace AlgorithmsSW.Digraphs;

/// <summary>
/// Provides an algorithm for finding all vertices reachable from a given vertex or set of vertices in a directed
/// graph.
/// </summary>
public class TransitiveClosure
{
	private readonly DirectedDepthFirstSearch[] searches;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="TransitiveClosure"/> class.
	/// </summary>
	/// <param name="digraph">The graph to find the transitive closure of.</param>
	public TransitiveClosure(IDigraph digraph)
	{
		digraph.ThrowIfNull();
		
		searches = new DirectedDepthFirstSearch[digraph.VertexCount];
		
		for (int vertex = 0; vertex < digraph.VertexCount; vertex++)
		{
			searches[vertex] = new DirectedDepthFirstSearch(digraph, vertex);
		}
	}
	
	/// <summary>
	/// Returns whether a given vertex is reachable from another given vertex.
	/// </summary>
	public bool Reachable(int vertex1, int vertex2) => searches[vertex1].Reachable(vertex2);
}
