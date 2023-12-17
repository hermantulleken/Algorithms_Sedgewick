using System.Numerics;
using AlgorithmsSW.List;

namespace AlgorithmsSW.EdgeWeightedGraph;

/// <summary>
/// Graph where the weight of the edges is the euclidean distance between the vertexes, which are points in 3D space.
/// </summary>
public class EuclideanDistanceGraph
{
	private readonly ResizeableArray<Vector3> vertexes;
	private readonly EdgeWeightedGraphWithAdjacencyLists<double> graph;
	
	EuclideanDistanceGraph(IEnumerable<Vector3> vertexes)
	{
		this.vertexes = vertexes.ToResizableArray(vertexes.Count());
		graph = new EdgeWeightedGraphWithAdjacencyLists<double>(this.vertexes.Count, Comparer<double>.Default);	
	}
	
	public Edge<double> AddEdge(int vertex0, int vertex1)
	{
		var vector0 = vertexes[vertex0];
		var vector1 = vertexes[vertex1];
		var edge = new Edge<double>(vertex0, vertex1, (vector1 - vector0).Length());
		graph.AddEdge(edge);

		return edge;
	}
	
	public void RemoveEdge(Edge<double> edge)
	{
		graph.RemoveEdge(edge);
	}
	
	public IEnumerable<Edge<double>> GetIncidentEdges(int vertex)
	{
		return graph.GetIncidentEdges(vertex);
	}
}
