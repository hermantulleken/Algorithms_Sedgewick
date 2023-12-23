namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;

/// <summary>
/// Represents a directed edge weighted graph data structure using adjacency lists.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class EdgeWeightedDigraphWithAdjacencyLists<TWeight> 
	: IEdgeWeightedDigraph<TWeight>
{
	private readonly ResizeableArray<DirectedEdge<TWeight>>[] adjacencyLists;

	/// <inheritdoc />
	public int VertexCount { get; }

	/// <inheritdoc />
	public int EdgeCount { get; private set; }

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex) => GetIncidentEdges(vertex).Select(edge => edge.ToVertex);

	/// <inheritdoc />
	public IComparer<TWeight> Comparer { get; }

	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> Edges => adjacencyLists.SelectMany(list => list);
	
	/// <summary>
	/// Initializes a new instance of the <see cref="EdgeWeightedDigraphWithAdjacencyLists{TWeight}"/> class.
	/// </summary>
	/// <param name="vertexCount">The number of vertices in the graph.</param>
	/// <param name="comparer">The comparer to use when comparing edge weights.</param>
	public EdgeWeightedDigraphWithAdjacencyLists(int vertexCount, IComparer<TWeight> comparer)
	{
		VertexCount = vertexCount;
		Comparer = comparer;
		adjacencyLists = new ResizeableArray<DirectedEdge<TWeight>>[vertexCount];
		adjacencyLists.Fill(() => []);
	}

	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> GetIncidentEdges(int vertex) => adjacencyLists[vertex];

	/// <inheritdoc />
	public void AddEdge(DirectedEdge<TWeight> edge)
	{
		adjacencyLists[edge.FromVertex].Add(edge);
		EdgeCount++;
	}

	/// <inheritdoc />
	public void RemoveEdge(DirectedEdge<TWeight> edge)
	{
		adjacencyLists[edge.FromVertex].Remove(edge);
		EdgeCount--;
	}
}