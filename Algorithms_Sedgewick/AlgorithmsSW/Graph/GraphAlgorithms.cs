using System.Diagnostics;
using AlgorithmsSW.SymbolTable;
using AlgorithmsSW.Counter;
using AlgorithmsSW.Stack;

namespace AlgorithmsSW.Graphs;

public static class GraphAlgorithms
{
	// 4.1.10
	public static int FindNodeSafeToDelete(IGraph graph)
	{
		graph.ThrowIfNull();

		if (graph.IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		if (graph.VertexCount == 1)
		{
			return graph.Vertices.First();
		}
		
		// TODO: How can we do this without the connectivity class?
		// Or should we make this function part of a class too.
		var connectivity = new Connectivity(graph);

		if (!connectivity.IsConnected)
		{
			throw new ArgumentException("Graph is not connected", nameof(graph));
		}
		
		var stack = new FixedCapacityStack<int>(graph.VertexCount);
		bool[] marked = new bool[graph.VertexCount];
		stack.Push(graph.Vertices.First());

		while (stack.Any())
		{
			int vertex = stack.Pop();
			marked[vertex] = true;
			var adjacents = graph.GetAdjacents(vertex);

			if (!adjacents.Any())
			{
				throw new ArgumentException("Graph has isolated nodes", nameof(graph));
			}
			
			if (adjacents.All(a => marked[a]))
			{
				return vertex;
			}

			foreach (int adjacent in adjacents)
			{
				if (!marked[adjacent])
				{
					stack.Push(adjacent);
				}
			}
		}

		// Impossible to reach
		Debug.Assert(false);
		return -1;
	}

	// 4.1.23
	public static Counter<int> DistanceHistogram<T>(this IGraph graph, int sourceVertex) 
		=> BreadthFirstPathsSearch
			.Build(graph, sourceVertex)
			.Distances.ToSymbolTable().CountKeysWithValue(Comparer<int>.Default);
	
	public static bool AllDegreesEven(this IGraph graph)
	{
		graph.ThrowIfNull();

		return graph.Vertices.All(vertex => graph.GetDegree(vertex) % 2 == 0);
	}
	
	public static bool HasEulerCycle(this IGraph graph)
	{
		graph.ThrowIfNull();

		if (graph.IsEmpty)
		{
			return true;
		}

		if (!graph.AllDegreesEven())
		{
			return false;
		}

		var connectivity = new Connectivity(graph);

		return connectivity.IsConnected;
	}
}
