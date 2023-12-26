namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using List;

/// <summary>
/// A digraph where the weight of the edges is the euclidean distance between the vertexes, which are points in
/// 3D space.
/// </summary>
// 4.4.27
public class EuclideanDistanceDigraph : IEdgeWeightedDigraph<double>
{
	private readonly IReadonlyRandomAccessList<Vector3> vertexes;
	private readonly IEdgeWeightedDigraph<double> graph;

	/// <inheritdoc/>
	public int VertexCount => graph.VertexCount;

	/// <inheritdoc/>
	public int EdgeCount => graph.EdgeCount;

	/// <inheritdoc/>
	public IComparer<double> Comparer => graph.Comparer;

	/// <inheritdoc/>
	public IEnumerable<DirectedEdge<double>> Edges => graph.Edges;

	/// <summary>
	/// Initializes a new instance of the <see cref="EuclideanDistanceDigraph"/> class.
	/// </summary>
	/// <param name="vertexes">The spatial representation of the vertexes.</param>
	public EuclideanDistanceDigraph(IEnumerable<Vector3> vertexes)
	{
		this.vertexes = vertexes.ToRandomAccessList();
		graph = DataStructures.EdgeWeightedDigraph(this.vertexes.Count, Comparer<double>.Default);
	}

	/// <inheritdoc/>
	public IEnumerable<int> GetAdjacents(int vertex) => graph.GetAdjacents(vertex);

	/// <inheritdoc/>
	public IEnumerable<DirectedEdge<double>> GetIncidentEdges(int vertex) => graph.GetIncidentEdges(vertex);

	/// <inheritdoc/>
	public void AddEdge(DirectedEdge<double> edge)
	{
		if (!graph.Comparer.ApproximatelyEqual(edge.Weight, GetDistance(edge.Source, edge.Target), 0.000_000_1, (x, y) => x + y))
		{
			throw new ArgumentException(
				"The edge weight does not match the euclidean distance between the vertexes. Use AddEdge(int, int) instead.",
				nameof(edge));
		}
		
		graph.AddEdge(edge);
	}
	
	public void AddEdge(int source, int target)
	{
		var edge = MakeEdge(source, target);
		graph.AddEdge(edge);
	}

	/// <inheritdoc/>
	public void RemoveEdge(DirectedEdge<double> edge) => graph.RemoveEdge(edge);
	
	public DirectedEdge<double> MakeEdge(int source, int target) 
		=> new(source, target, GetDistance(source, target));

	private double GetDistance(int vertex0, int vertex1)
	{
		var vector0 = vertexes[vertex0];
		var vector1 = vertexes[vertex1];
		
		return (vector1 - vector0).Length();
	}
}
