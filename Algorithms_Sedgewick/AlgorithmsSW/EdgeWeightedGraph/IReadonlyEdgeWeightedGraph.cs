using AlgorithmsSW.Graph;

namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Collections.Generic;

/// <summary>
/// Represents a read-only weighted graph data structure.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IReadOnlyEdgeWeightedGraph<TWeight> : IReadOnlyGraph
{
	/// <summary>
	/// Gets the comparer this <see cref="IReadOnlyEdgeWeightedGraph{T}"/> use to compare edge weights. 
	/// </summary>
	IComparer<TWeight> Comparer { get; }
	
	/// <summary>
	/// Gets the edges that is part of this <see cref="IReadOnlyEdgeWeightedGraph{T}"/>.
	/// </summary>
	IEnumerable<Edge<TWeight>> Edges { get; }
	
	/// <summary>
	/// Gets the edges that is incident to the given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to get the incident edges of.</param>
	IEnumerable<Edge<TWeight>> GetIncidentEdges(int vertex);
}
