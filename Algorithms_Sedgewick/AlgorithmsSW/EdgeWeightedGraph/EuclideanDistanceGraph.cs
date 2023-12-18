using System.Collections;
using System.Numerics;
using AlgorithmsSW.List;

namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// Graph where the weight of the edges is the euclidean distance between the vertexes, which are points in 3D space.
/// </summary>
public class EuclideanDistanceGraph : IEdgeWeightedGraph<double>
{
	private readonly ResizeableArray<Vector3> vertexes;
	private readonly EdgeWeightedGraphWithAdjacencyLists<double> graph;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="EuclideanDistanceGraph"/> class.
	/// </summary>
	/// <param name="vertexes">The spatial representation of the vertexes.</param>
	public EuclideanDistanceGraph(IEnumerable<Vector3> vertexes)
	{
		this.vertexes = vertexes.ToResizableArray(vertexes.Count());
		graph = new EdgeWeightedGraphWithAdjacencyLists<double>(this.vertexes.Count, Comparer<double>.Default);	
	}
	
	/// <summary>
	/// Adds an edge between the two given vertexes to the graph.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge to add.</param>
	/// <param name="vertex1">The second vertex of the edge to add.</param>
	/// <returns>The added edge.</returns>
	public Edge<double> AddEdge(int vertex0, int vertex1)
	{
		var edge = MakeEdge(vertex0, vertex1);
		graph.AddEdge(edge);

		return edge;
	}

	/// <summary>
	/// Makes an edge between the two given vertexes.
	/// </summary>
	/// <param name="vertex0">The first vertex of the edge to make.</param>
	/// <param name="vertex1">The second vertex of the edge to make.</param>
	/// <returns>An edge between the two given vertexes.</returns>
	/// <remarks>The weight of the edge is the euclidean distance between the vertexes.</remarks>
	public Edge<double> MakeEdge(int vertex0, int vertex1)
	{
		var vector0 = vertexes[vertex0];
		var vector1 = vertexes[vertex1];
		
		return new Edge<double>(vertex0, vertex1, (vector1 - vector0).Length());
	}

	/// <inheritdoc />
	public void AddEdge(Edge<double> edge)
	{
		var vector0 = vertexes[edge.Vertex0];
		var vector1 = vertexes[edge.Vertex1];
		double length = (vector1 - vector0).Length();

		if (!MathX.ApproximatelyEqual(length, edge.Weight))
		{
			throw new Exception("The edge weight does not match the euclidean distance between the vertexes. Use AddEdge(int, int) instead.");
		}
		
		graph.AddEdge(edge);
	}

	/// <inheritdoc cref="IEdgeWeightedGraph{TWeight}" />
	public bool RemoveEdge(Edge<double> edge) => graph.RemoveEdge(edge);

	/// <inheritdoc />
	public IComparer<double> Comparer => graph.Comparer;

	/// <inheritdoc />
	public IEnumerable<Edge<double>> Edges => graph.Edges;

	/// <inheritdoc />
	public IEnumerable<Edge<double>> GetIncidentEdges(int vertex) => graph.GetIncidentEdges(vertex);

	/// <inheritdoc />
	public IEnumerator<(int vertex0, int vertex1)> GetEnumerator() => graph.GetEnumerator();

	/// <inheritdoc />
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)graph).GetEnumerator();

	/// <inheritdoc />
	public int VertexCount => graph.VertexCount;

	/// <inheritdoc />
	public int EdgeCount => graph.EdgeCount;

	/// <inheritdoc />
	public bool SupportsParallelEdges => graph.SupportsParallelEdges;

	/// <inheritdoc />
	public bool SupportsSelfLoops => graph.SupportsSelfLoops;

	/// <inheritdoc />
	public IEnumerable<int> GetAdjacents(int vertex) => graph.GetAdjacents(vertex);

	/// <inheritdoc />
	public bool ContainsEdge(int vertex0, int vertex1) => graph.ContainsEdge(vertex0, vertex1);
}
