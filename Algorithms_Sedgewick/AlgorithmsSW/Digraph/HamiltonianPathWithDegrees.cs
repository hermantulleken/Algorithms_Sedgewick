using System.Diagnostics.CodeAnalysis;
using AlgorithmsSW.List;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.Digraphs;

/// <summary>
/// Algorithm to find a Hamiltonian cycle in a directed graph.
/// </summary>
// 4.2.24
public class HamiltonianPathWithDegrees : IHamiltonianPath
{
	/// <inheritdoc/>
	[MemberNotNullWhen(true, nameof(cycle))]
	public bool HasHamiltonianPath { get; }
	
	/// <inheritdoc/>
	public IEnumerable<int> Path
	{
		get
		{
			if (!HasHamiltonianPath)
			{
				throw new InvalidOperationException();
			}
			
			return cycle;
		}
	}
	
	private ResizeableArray<int>? cycle;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="HamiltonianPathWithDegrees"/> class.
	/// </summary>
	/// <param name="digraph">A directed graph with no cycles.</param>
	public HamiltonianPathWithDegrees(IDigraph digraph)
	{
		if (digraph.VertexCount == 0)
		{
			HasHamiltonianPath = true;
			cycle = ResizeableArray<int>.Empty;
			return;
		}
		
		var directedCycle = new DirectedCycle(digraph);

		if (directedCycle.HasCycle)
		{
			throw new ArgumentException("Expects a DAG.", nameof(digraph));
		}
		
		var directedDepthFirstSearch = new DirectedDepthFirstSearch(digraph, 0);
		
		if (!directedDepthFirstSearch.IsConnectedToSources)
		{
			HasHamiltonianPath = false;
			return;
		}
		
		var degrees = new Degrees(digraph);
		HasHamiltonianPath = degrees is { SourcesCount: 1, SinksCount: 1 };
		
		if (HasHamiltonianPath)
		{
			Search(digraph, degrees.Sources.First(), degrees.Sinks.First());
		}
	}

	private void Search(IDigraph digraph, int source, int sink)
	{
		int node = source;
		cycle = [node];
		
		while (node != sink)
		{
			var adjacents = digraph.GetAdjacents(node);
			
			Assert(adjacents.Count() == 1);
			
			node = adjacents.First();
			cycle.Add(node);
		}
		
		Assert(cycle.Count == digraph.VertexCount);
	}
}
