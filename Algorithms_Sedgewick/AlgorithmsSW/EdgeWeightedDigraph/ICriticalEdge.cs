namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Given two nodes, the edges that when any is removed will increases the shortest path between them the most, but not
/// remove all shortest paths.
/// </summary>
/*	Note: It is not clear whether "but not remove all shortest paths" should be part of the definition or not.
*/
[ExerciseReference(4, 4, 37)]
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
	TWeight? DistanceWithoutCriticalEdge { get; }

	/// <summary>
	/// Gets a value indicating whether there are any critical edges in a graph, where a critical edge is defined
	/// as an edge that, when removed, increases the shortest path between two nodes the most,
	/// but does not remove all shortest paths.
	/// </summary>
	/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
	/// <seealso cref="AlgorithmsSW.EdgeWeightedDigraph.ICriticalEdge{TWeight}" />
	[MemberNotNullWhen(true, nameof(DistanceWithoutCriticalEdge))]
	public bool HasCriticalEdges { get; }
}
