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

	public IEnumerable<int> GetShortestPathBetween(int vertex0, int vertex1)
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
