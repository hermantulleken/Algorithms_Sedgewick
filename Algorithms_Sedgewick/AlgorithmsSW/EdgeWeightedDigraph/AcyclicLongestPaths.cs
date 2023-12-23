namespace AlgorithmsSW.EdgeWeightedDigraph;

public class AcyclicLongestPaths<TWeight> : IShortestPath<TWeight>
{
	private AcyclicShortestPaths<TWeight> invertedShortestPath;
	
	AcyclicLongestPaths(IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source, 
		Func<TWeight, TWeight, TWeight> add, 
		TWeight zero, 
		TWeight minValue)
	{
		var invertedGraph = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(graph, graph.Comparer.Invert());
		invertedShortestPath = new AcyclicShortestPaths<TWeight>(invertedGraph, source, add, zero, minValue);
	}

	public TWeight DistanceTo(int vertex)
	{
		return invertedShortestPath.DistanceTo(vertex);
	}

	public bool HasPathTo(int vertex)
	{
		return invertedShortestPath.HasPathTo(vertex);
	}

	public IEnumerable<DirectedEdge<TWeight>> PathTo(int target)
	{
		return invertedShortestPath.PathTo(target);
	}
}