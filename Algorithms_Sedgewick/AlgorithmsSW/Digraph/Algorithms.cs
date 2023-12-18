namespace AlgorithmsSW.Digraph;

public static class Algorithms
{
	/// <summary>
	/// Reverses a directed graph.
	/// </summary>
	/// <param name="digraph">The graph to reverse.</param>
	public static IDigraph Reverse(this IDigraph digraph)
	{
		digraph.ThrowIfNull();
		
		var reversed = new DigraphWithAdjacentsLists(digraph.VertexCount);
		
		for (int vertex = 0; vertex < digraph.VertexCount; vertex++)
		{
			foreach (int adjacent in digraph.GetAdjacents(vertex))
			{
				reversed.AddEdge(adjacent, vertex);
			}
		}
		
		return reversed;
	}
	
	// 4.2.9
	public static bool IsTopologicalOrder(this IDigraph digraph, IEnumerable<int> order)
	{
		digraph.ThrowIfNull();
		order.ThrowIfNull();
		
		int[] ranks = new int[digraph.VertexCount];
		int rank = 0;
		
		foreach (int vertex in order)
		{
			ranks[vertex] = rank;
			rank++;
		}

		var outOfOrderVertices = 
			from vertex in digraph.Vertexes
			from adjacent in digraph.GetAdjacents(vertex)
			where ranks[vertex] > ranks[adjacent] // vertex is after adjacent in order, thus not topological
			select vertex; 

		return !outOfOrderVertices.Any();
	}
}
