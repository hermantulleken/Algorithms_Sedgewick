namespace Algorithms_Sedgewick.Graphs;

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