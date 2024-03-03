using System.Diagnostics.CodeAnalysis;

namespace AlgorithmsSW.Digraph;

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
	public Topological(IReadOnlyDigraph digraph)
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
