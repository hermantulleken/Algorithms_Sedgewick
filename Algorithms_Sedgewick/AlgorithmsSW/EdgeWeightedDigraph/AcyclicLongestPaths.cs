namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;

[ExerciseReference(4, 4, 28)]
public class AcyclicLongestPaths<TWeight> : IShortestPath<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private AcyclicShortestPaths<TWeight> invertedShortestPath;
	
	public AcyclicLongestPaths(IReadOnlyEdgeWeightedDigraph<TWeight> graph, int source)
	{
		var invertedGraph = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(graph.VertexCount);

		foreach (var edge in graph.WeightedEdges)
		{
			invertedGraph.AddEdge(edge.Source, edge.Target, -edge.Weight);
		}
		
		invertedShortestPath = new(invertedGraph, source);
	}

	public TWeight GetDistanceTo(int vertex)
	{
		return -invertedShortestPath.GetDistanceTo(vertex);
	}

	public bool HasPathTo(int vertex)
	{
		return invertedShortestPath.HasPathTo(vertex);
	}

	public IEnumerable<DirectedEdge<TWeight>> GetEdgesOfPathTo(int target)
	{
		return invertedShortestPath.GetEdgesOfPathTo(target);
	}
}
