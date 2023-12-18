namespace AlgorithmsSW.Digraph;

public class HamiltonianPathWithTopologicalSort : IHamiltonianPath
{
	private readonly Topological topological;

	/// <inheritdoc/>
	public bool HasHamiltonianPath { get; }

	/// <inheritdoc/>
	public IEnumerable<int> Path
	{
		get
		{
			if (!HasHamiltonianPath)
			{
				throw new InvalidOperationException("No Hamiltonian cycle exists.");
			}
			
			return topological.Order;
		}
	}

	public HamiltonianPathWithTopologicalSort(IDigraph digraph)
	{
		topological = new Topological(digraph);

		if (!topological.IsDirectedAcyclic)
		{
			throw new ArgumentException("Graph cannot have cycles.", nameof(digraph));
		}

		HasHamiltonianPath = true;
		var order = topological.Order.Buffer2();

		foreach (var pair in order)
		{
			if (!digraph.AreAdjacent(pair.First, pair.Last))
			{
				HasHamiltonianPath = false;
			}
		}
	}
}
