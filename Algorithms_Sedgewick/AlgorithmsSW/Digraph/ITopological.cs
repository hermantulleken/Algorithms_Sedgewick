namespace AlgorithmsSW.Digraph;

using System.Diagnostics.CodeAnalysis;

public interface ITopological
{
	/// <summary>
	/// Gets a value indicating whether the digraph is a directed acyclic graph.
	/// </summary>
	bool IsDirectedAcyclic { get; }
	
	/// <summary>
	/// Gets the vertices in topologically sorted order, if such an order is possible. Otherwise, null.
	/// </summary>
	/// <seealso cref="IsDirectedAcyclic"/>
	[MemberNotNullWhen(true, nameof(Order))]
	IEnumerable<int>? Order { get; }
}