namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Collections;
using Digraph;
using EdgeWeightedGraph;
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
	public IEnumerable<int> GetAdjacents(int vertex) => GetIncidentEdges(vertex).Select(edge => edge.Target);

	public bool SupportsParallelEdges => true;
	public bool SupportsSelfLoops => true;

	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> WeightedEdges => adjacencyLists.SelectMany(list => list);
	
	/// <summary>
	/// Initializes a new instance of the <see cref="EdgeWeightedDigraphWithAdjacencyLists{TWeight}"/> class.
	/// </summary>
	/// <param name="vertexCount">The number of vertices in the graph.</param>
	public EdgeWeightedDigraphWithAdjacencyLists(int vertexCount)
	{
		VertexCount = vertexCount;
		adjacencyLists = new ResizeableArray<DirectedEdge<TWeight>>[vertexCount];
		adjacencyLists.Fill(() => []);
	}
	
	public EdgeWeightedDigraphWithAdjacencyLists(IReadOnlyEdgeWeightedGraph<TWeight> graph)
		: this(graph.VertexCount)
	{
		foreach (var edge in graph.WeightedEdges)
		{
			AddEdge(new(edge.Vertex0, edge.Vertex1, edge.Weight));
			AddEdge(new(edge.Vertex1, edge.Vertex0, edge.Weight));
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="EdgeWeightedDigraphWithAdjacencyLists{TWeight}"/> class from the
	/// specified graph, but with a different comparer.
	/// </summary>
	/// <param name="graph">The graph to construct the new graph from.</param>
	public EdgeWeightedDigraphWithAdjacencyLists(IReadOnlyEdgeWeightedDigraph<TWeight> graph)
		: this(graph.VertexCount)
	{
		/*
			TODO: It would be nice to share the edge data. To do so we need two things:
			-	We need to original graph to be immutable. (This is slightly different from being read-only, since
				we can still have a mutable graph behind the read-only interface. 
				
				[This is maybe why IsReadOnly properties may be useful oin practice.]
				
			- We need to have code that looks if the graph is of the right type. 
		*/
		foreach (var edge in graph.WeightedEdges)
		{
			AddEdge(edge);
		}
	}
	
	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> GetIncidentEdges(int vertex) => adjacencyLists[vertex];

	/// <inheritdoc />
	public void AddEdge(DirectedEdge<TWeight> edge)
	{
		adjacencyLists[edge.Source].Add(edge);
		EdgeCount++;
	}

	/// <inheritdoc />
	public bool RemoveEdge(DirectedEdge<TWeight> edge)
	{
		bool wasRemoved = adjacencyLists[edge.Source].Remove(edge);

		if (wasRemoved)
		{
			EdgeCount--;
		}

		return wasRemoved;
	}

	public bool TryGetUniqueEdge(int pairFirst, int pairLast, out DirectedEdge<TWeight> edge) 
		=> throw new NotImplementedException();

	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator() => ((IReadOnlyDigraph)this).Edges.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
