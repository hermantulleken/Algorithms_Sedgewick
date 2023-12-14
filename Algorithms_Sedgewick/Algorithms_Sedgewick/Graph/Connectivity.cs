using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick.Graphs;

/// <summary>
/// A class for querying connectivity properties of a graph. 
/// </summary>
public class Connectivity
{
	private readonly ResizeableArray<int> vertexOfComponent;
	private readonly ResizeableArray<BreadthFirstPathsSearch> components;
	private readonly ResizeableArray<int> componentIndexOfVertex;

	/// <summary>
	/// Gets the total number of connected components in the graph.
	/// </summary>
	public int ComponentCount { get; private set; }

	/// <summary>
	/// Gets a value indicating whether the entire graph is connected as a single component.
	/// </summary>
	public bool IsConnected => ComponentCount == 1;
	
	/// <summary>
	/// Gets the total number of vertices in the graph.
	/// </summary>
	private int VertexCount => componentIndexOfVertex.Count;

	private Connectivity(IGraph graph)
	{
		vertexOfComponent = new ResizeableArray<int>();
		components = new ResizeableArray<BreadthFirstPathsSearch>();
		componentIndexOfVertex = new ResizeableArray<int>(graph.VertexCount);
		componentIndexOfVertex.AddN(-1, graph.VertexCount);
	}

	/// <summary>
	/// Builds a <see cref="Connectivity"/> object for a given graph.
	/// </summary>
	/// <param name="graph">The graph to analyze.</param>
	/// <returns>A <see cref="Connectivity"/> object representing the graph's connectivity.</returns>
	public static Connectivity Build(IGraph graph)
	{
		graph.ThrowIfNull();
		var connectivity = new Connectivity(graph);
		connectivity.FindConnectedComponents(graph);
		
		return connectivity;
	}

	/// <summary>
	/// Determines if two vertices are connected.
	/// </summary>
	/// <param name="vertex0">The first vertex.</param>
	/// <param name="vertex1">The second vertex.</param>
	/// <returns><see langword="true"/> if the vertices are connected; otherwise, <see langword="false"/>.</returns>
	public bool AreConnected(int vertex0, int vertex1)
	{
		vertex0.ThrowIfOutOfRange(VertexCount);
		vertex1.ThrowIfOutOfRange(VertexCount);
		
		return componentIndexOfVertex[vertex0] == componentIndexOfVertex[vertex1];
	}

	/// <summary>
	/// Gets the shortest path between two vertices.
	/// </summary>
	/// <param name="vertex0">The starting vertex.</param>
	/// <param name="vertex1">The ending vertex.</param>
	/// <returns>An enumerable of integers representing the shortest path.</returns>
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
			if (componentIndexOfVertex[vertex] != -1)
			{
				vertex++;
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
