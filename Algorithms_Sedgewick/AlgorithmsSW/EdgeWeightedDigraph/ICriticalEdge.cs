namespace AlgorithmsSW.EdgeWeightedDigraph;

/// <summary>
/// Given two nodes, the edges that when any is removed will increases the shortest path between them the most, but not
/// remove all shortest paths.
/// </summary>
/*	Note: It is not clear whether "but not remove all shortest paths" should be part of the definition or not.
*/
public interface ICriticalEdge<TWeight>
{
	/// <summary>
	/// Gets the critical edge, if any.
	/// </summary>
	/// <remarks>The critical edge is the edge that when removed will increases the shortest path between the source
	/// and the destination the most.</remarks>
	IEnumerable<DirectedEdge<TWeight>> CriticalEdges { get; }
	
	/// <summary>
	/// Gets the distance of the shortest path oif the critical edge would be removed.  
	/// </summary>
	TWeight DistanceWithoutCriticalEdge { get; }
}
