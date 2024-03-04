namespace AlgorithmsSW.EdgeWeightedDigraph;

/// <summary>
/// Given two nodes, the edge that when removed will increases the shortest path between them the most. 
/// </summary>
public interface ICriticalEdge<TWeight>
{
	/// <summary>
	/// Gets a value indicating whether whether the graph has a critical edge.
	/// </summary>
	bool HasCriticalEdge { get; }
	
	/// <summary>
	/// Gets the critical edge, if any.
	/// </summary>
	/// <remarks>The critical edge is the edge that when removed will increases the shortest path between the source
	/// and the destination the most.</remarks>
	DirectedEdge<TWeight>? CriticalEdge { get; }
	
	/// <summary>
	/// Gets the distance of the shortest path oif the critical edge would be removed.  
	/// </summary>
	TWeight DistanceWithoutCriticalEdge { get; }
}
