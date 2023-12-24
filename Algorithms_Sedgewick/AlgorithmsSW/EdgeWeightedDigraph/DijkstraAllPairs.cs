namespace AlgorithmsSW.EdgeWeightedDigraph;

/// <summary>
/// A class for finding the shortest path from a source vertex to all other vertices in a directed edge weighted graph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class DijkstraAllPairs<TWeight>
{
	private Dijkstra<TWeight>[] shortestPaths;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraAllPairs{T}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="add">The function to add two weights.</param>
	/// <param name="zero">The zero value for the weights.</param>
	/// <param name="maxValue">The maximum value for the weights.</param>
	public DijkstraAllPairs(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		Func<TWeight, TWeight, TWeight> add,
		TWeight zero, 
		TWeight maxValue)
	{
		shortestPaths = new Dijkstra<TWeight>[graph.VertexCount];
		
		for (int i = 0; i < graph.VertexCount; i++)
		{
			shortestPaths[i] = new(graph, i, add, zero, maxValue);
		}
	}
	
	/// <summary>
	/// Gets the shortest path from the source vertex to the target vertex.
	/// </summary>
	/// <param name="sourceVertex">The vertex to find the shortest path from.</param>
	/// <param name="targetVertex">The vertex to find the shortest path to.</param>
	/// <returns>An enumerable of edges representing the shortest path.</returns>
	public IEnumerable<DirectedEdge<TWeight>> GetPath(int sourceVertex, int targetVertex)
	{
		return shortestPaths[sourceVertex].GetEdgesOfPathTo(targetVertex);
	}
	
	/// <summary>
	/// Gets the distance from the source vertex to the target vertex.
	/// </summary>
	/// <param name="sourceVertex">The vertex to find the distance from.</param>
	/// <param name="targetVertex">The vertex to find the distance to.</param>
	/// <returns>The distance from the source vertex to the target vertex.</returns>
	public TWeight GetDistance(int sourceVertex, int targetVertex)
	{
		return shortestPaths[sourceVertex].GetDistanceTo(targetVertex);
	}
}
