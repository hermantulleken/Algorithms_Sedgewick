using AlgorithmsSW.Counter;
using AlgorithmsSW.List;
using AlgorithmsSW.Stack;
using AlgorithmsSW.SymbolTable;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.Graph;

using Support;

public static class GraphAlgorithms
{
	[ExerciseReference(4, 1, 10)]
	public static int FindNodeSafeToDelete(IGraph graph)
	{
		graph.ThrowIfNull();

		if (graph.IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}

		if (graph.VertexCount == 1)
		{
			return graph.Vertexes.First();
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
		stack.Push(graph.Vertexes.First());

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
		Assert(false);
		return -1;
	}

	[ExerciseReference(4, 1, 23)]
	public static Counter<int> DistanceHistogram<T>(this IGraph graph, int sourceVertex) 
		=> BreadthFirstPathsSearch
			.Build(graph, sourceVertex)
			.Distances.ToSymbolTable().CountKeysWithValue(Comparer<int>.Default);
	
	public static bool AllDegreesEven(this IGraph graph)
	{
		graph.ThrowIfNull();

		return graph.Vertexes.All(vertex => graph.GetDegree(vertex) % 2 == 0);
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

	public static bool IsBridge(this IGraph graph, int vertex0, int vertex1)
	{
		graph.ThrowIfNull();

		if (!graph.ContainsEdge(vertex0, vertex1))
		{
			return false;
		}

		graph.RemoveEdge(vertex0, vertex1);
		var connectivity = new Connectivity(graph);
		bool connected = !connectivity.AreConnected(vertex0, vertex1);
		graph.AddEdge(vertex0, vertex1);
		
		return !connected;

	}
	
	public static void ConnectComponents(this IGraph graph)
	{
		var connectivity = new Connectivity(graph);
		
		int numberOfVertices = graph.VertexCount;
		int numberOfComponents = connectivity.ComponentCount;

		if (connectivity.IsConnected)
		{
			return;
		}
		
		int[] componentRepresentatives = new int[numberOfComponents];
		componentRepresentatives.Fill(-1);
		int componentsFound = 0;

		// TODO: We can store representatives in the Connectivity class since we find them there and nicely speed this up
		for (int vertex = 0; vertex < numberOfVertices; vertex++)
		{
			int componentIndex = connectivity.GetComponentIndex(vertex);

			if (componentRepresentatives[componentIndex] != -1)
			{
				continue; // We already have a representative for this component
			}
			
			componentRepresentatives[componentIndex] = vertex;
			componentsFound++;

			if (componentsFound == numberOfComponents)
			{
				break; // We have them all
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
			
			graph.AddEdge(vertex0, vertex1);
		}
		
		#if DEBUG
		connectivity = new Connectivity(graph);
		Assert(connectivity.IsConnected);
		#endif
	}

	// This is a helper for a version of 4.4.33 (Combine with undirected graph version of ToEdgeWeightedDigraph). 
	[ExerciseReference(4, 4, 33)]
	public static GridGraph GetFullGrid(int width, int height)
	{
		var graph = new GridGraph(width, height);

		for (int j = 0; j < height; j++)
		{
			for (int i = 0; i < width - 1; i++)
			{
				graph.AddEdge((i, j), (i + 1, j));
			}
		}

		for (int j = 0; j < height - 1; j++)
		{
			for (int i = 0; i < width; i++)
			{
				graph.AddEdge((i, j), (i, j + 1));
			}
		}

		return graph;
	}
}
