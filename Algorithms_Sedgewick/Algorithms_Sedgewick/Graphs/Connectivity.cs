using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.Graphs;

public class Connectivity
{
	private readonly ResizeableArray<int> vertexOfComponent;
	private readonly ResizeableArray<BreadthFirstPathsSearch> components;
	private readonly ResizeableArray<int> componentIndexOfVertex;

	public int ComponentCount { get; private set; }
	
	private int VertexCount => componentIndexOfVertex.Count;

	public Connectivity(IGraph graph)
	{
		vertexOfComponent = new ResizeableArray<int>();
		components = new ResizeableArray<BreadthFirstPathsSearch>();
		componentIndexOfVertex = new ResizeableArray<int>(graph.VertexCount);

		for (int i = 0; i < graph.VertexCount; i++)
		{
			componentIndexOfVertex.Add(-1);
		}
	}

	public static Connectivity Build(IGraph graph)
	{
		graph.ThrowIfNull();
		var connectivity = new Connectivity(graph);
		connectivity.FindConnectedComponents(graph);
		
		return connectivity;
	}

	public bool AreConnected(int vertex0, int vertex1)
	{
		vertex0.ThrowIfOutOfRange(VertexCount);
		vertex1.ThrowIfOutOfRange(VertexCount);
		
		return vertexOfComponent[vertex0] == vertexOfComponent[vertex1];
	}

	public IEnumerable<int>? GetShortestPathBetween(int vertex0, int vertex1)
	{
		vertex0.ThrowIfOutOfRange(VertexCount);
		vertex1.ThrowIfOutOfRange(VertexCount);
		
		int componentIndex = componentIndexOfVertex[vertex0];

		return components[componentIndex].GetPathTo(vertex1);
	}

	private void FindConnectedComponents(IGraph graph)
	{
		int vertex = 0;
		ComponentCount = 0;

		while (vertex < graph.VertexCount)
		{
			if (componentIndexOfVertex[vertex] == -1)
			{
				continue;
			}
			
			var search = BreadthFirstPathsSearch.Build(graph, vertex);
			components.Add(search);
			vertexOfComponent.Add(vertex);

			foreach (int markedVertex in search.MarkedVertexes)
			{
				componentIndexOfVertex[markedVertex] = ComponentCount;
			}

			ComponentCount++;
			vertex++;
		}
	}
}

public class Cycle
{
	private readonly bool[] marked;

	public bool HasCycle { get; private set; }

	private Cycle(IGraph graph)
	{
		marked = new bool[graph.VertexCount];
	}

	public static Cycle Build(IGraph graph)
	{
		var cycle = new Cycle(graph);
		cycle.FindCycles(graph);
		return cycle;
	}

	private void FindCycles(IGraph graph)
	{
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			if (!marked[vertex])
			{
				Search(graph, vertex, vertex);
			}
		}
	}
	
	private void Search(IGraph graph, int vertex0, int vertex1)
	{
		// TODO: Does this class detect self loops too?
		// Can it halt early?
		// Should we not use one SearchPaths or such instead?
		marked[vertex0] = true;
		
		foreach (int adjacent in graph.GetAdjacents(vertex0))
		{
			if (!marked[adjacent])
			{
				Search(graph, adjacent, vertex0);
			}
			else if (adjacent != vertex1)
			{
				HasCycle = true;
			}
		}
	}
}

public class Bipartite
{
	public bool IsBipartite { get; private set; } = true;

	private readonly bool[] marked;
	private readonly bool[] color;

	private Bipartite(IGraph graph)
	{
		marked = new bool[graph.VertexCount];
		color = new bool[graph.VertexCount];
	}

	public static Bipartite Build(IGraph graph)
	{
		graph.ThrowIfNull();
		
		var bipartite = new Bipartite(graph);
		bipartite.ColorGraph(graph);
		return bipartite;
	}

	private void ColorGraph(IGraph graph)
	{
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			if (!marked[vertex])
			{
				Search(graph, vertex);
			}
		}
	}

	private void Search(IGraph graph, int vertex)
	{
		marked[vertex] = true;
		
		foreach (int w in graph.GetAdjacents(vertex))
		{
			if (!marked[w])
			{
				color[w] = !color[vertex];
				Search(graph, w);
			}
			else if (color[w] == color[vertex])
			{
				IsBipartite = false;
			}
		}
	}
}
