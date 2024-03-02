namespace AlgorithmsSW.EdgeWeightedDigraph;

[ExerciseReference(4, 4, 28)]
public class AcyclicLongestPaths<TWeight> : IShortestPath<TWeight>
{
	private AcyclicShortestPaths<TWeight> invertedShortestPath;
	
	public AcyclicLongestPaths(IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source, 
		Func<TWeight, TWeight, TWeight> add, 
		TWeight zero, 
		TWeight minValue)
	{
		var invertedGraph = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(graph, graph.Comparer.Invert());
		invertedShortestPath = new AcyclicShortestPaths<TWeight>(invertedGraph, source, add, zero, minValue);
	}

	public TWeight GetDistanceTo(int vertex)
	{
		return invertedShortestPath.GetDistanceTo(vertex);
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
