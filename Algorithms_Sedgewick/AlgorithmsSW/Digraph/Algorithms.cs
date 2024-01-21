namespace AlgorithmsSW.Digraph;

using List;
using static System.Diagnostics.Debug;

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
	
	[ExerciseReference(4, 2, 9)]
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
	
	/// <summary>
	/// Add edges to a directed graph to make it strongly connected.
	/// </summary>
	/// <param name="graph">The graph to make strongly connected.</param>
	public static void ConnectComponents(this IDigraph graph)
	{
		var strongConnectivity = new StrongComponents(graph);

		int numberOfVertices = graph.VertexCount;
		int numberOfComponents = strongConnectivity.ComponentCount;

		if (strongConnectivity.ComponentCount == 1)
		{
			return;
		}

		int[] componentRepresentatives = new int[numberOfComponents];
		componentRepresentatives.Fill(-1);
		int componentsFound = 0;

		// Finding representatives for each strongly connected component
		for (int vertex = 0; vertex < numberOfVertices; vertex++)
		{
			int componentIndex = strongConnectivity.GetComponentIndex(vertex);

			if (componentRepresentatives[componentIndex] != -1)
			{
				continue; // We already have a representative for this component
			}

			componentRepresentatives[componentIndex] = vertex;
			componentsFound++;

			if (componentsFound == numberOfComponents)
			{
				break; // We have found all representatives
			}
		}

		Assert(componentsFound == numberOfComponents);

		// Connect the components in order
		for (int i = 0; i < numberOfComponents - 1; i++)
		{
			int vertex0 = componentRepresentatives[i];
			int vertex1 = componentRepresentatives[i + 1];

			Assert(vertex0 != -1);
			Assert(vertex1 != -1);
			Assert(vertex0 != vertex1);

			// Adding a directed edge from the representative of one component to the next
			graph.AddEdge(vertex0, vertex1);
			graph.AddEdge(vertex1, vertex0);
		}

#if DEBUG
	strongConnectivity = new StrongComponents(graph);
	Assert(strongConnectivity.ComponentCount == 1);
#endif
	}

}
