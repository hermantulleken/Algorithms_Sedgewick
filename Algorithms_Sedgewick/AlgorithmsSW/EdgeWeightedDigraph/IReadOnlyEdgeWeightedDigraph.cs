namespace AlgorithmsSW.EdgeWeightedDigraph;

using Digraph;

/// <summary>
/// Represents a read-only directed edge weighted graph data structure.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IReadOnlyEdgeWeightedDigraph<TWeight> 
	: IReadOnlyDigraph
{
	/// <summary>
	/// Gets the comparer this <see cref="IReadOnlyEdgeWeightedDigraph{T}"/> use to compare edge weights.
	/// </summary>
	IComparer<TWeight> Comparer { get; }
	
	/// <summary>
	/// Gets the edges that is part of this <see cref="IReadOnlyEdgeWeightedDigraph{T}"/>.
	/// </summary>
	IEnumerable<DirectedEdge<TWeight>> WeightedEdges { get; }
	
	/// <summary>
	/// Gets the edges that is incident to the given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to get the incident edges of.</param>
	/// <returns>An enumerable of incident edges.</returns>
	IEnumerable<DirectedEdge<TWeight>> GetIncidentEdges(int vertex);
	
	public new IEnumerable<(int source, int target)> Edges => WeightedEdges.Select(edge => (edge.Source, edge.Target));
	
	IEnumerable<(int source, int target)> IReadOnlyDigraph.Edges => Edges;
}
