using System.Diagnostics.CodeAnalysis;

namespace AlgorithmsSW.Digraphs;

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

/// <summary>
/// An algorithm that topologically sorts a directed graph.
/// </summary>
public class Topological : ITopological
{
	private readonly IEnumerable<int>? order;
	
	/// <inheritdoc />
	[MemberNotNullWhen(true, nameof(order))]
	public bool IsDirectedAcyclic => order != null;
	
	/// <inheritdoc />
	public IEnumerable<int> Order
	{
		get
		{
			if (!IsDirectedAcyclic)
			{
				throw new InvalidOperationException("The graph has cycles and cannot be topologically sorted.");
			}
			
			return order;
		}
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Topological"/> class.
	/// </summary>
	/// <param name="digraph">The graph to topologically sort.</param>
	public Topological(IDigraph digraph)
	{
		digraph.ThrowIfNull();
		var cycleFinder = new DirectedCycle(digraph);

		if (cycleFinder.HasCycle)
		{
			return;
		}
		
		var depthFirstOrder = new DepthFirstOrder(digraph);
		order = depthFirstOrder.ReversePostOrder;
	}
}
