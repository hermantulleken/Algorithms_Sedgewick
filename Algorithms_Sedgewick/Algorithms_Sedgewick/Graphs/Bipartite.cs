using static System.Diagnostics.Debug;

namespace Algorithms_Sedgewick.Graphs;

public class Bipartite
{
	/// <summary>
	/// Gets a value indicating whether the graph is bipartite.
	/// </summary>
	public bool IsBipartite { get; private set; } = true;
	
	private Bipartite()
	{
	}

	public static Bipartite Build(IGraph graph)
	{
		graph.ThrowIfNull();
		
		var bipartite = new Bipartite();
		bipartite.ColorGraph(graph);
		return bipartite;
	}

	private void ColorGraph(IGraph graph)
	{
		bool[] marked = new bool[graph.VertexCount];
		bool[] color = new bool[graph.VertexCount];

		IsBipartite = true; // Unless proven otherwise
		
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			if (marked[vertex])
			{
				continue;
			}
			
			color[vertex] = true;
			Search(vertex);
			
			if (!IsBipartite)
			{
				return;
			}
		}
		
		Assert(AllVerticesMarked());
		
		void Search(int vertex)
		{
			marked[vertex] = true;
		
			foreach (int adjacent in graph.GetAdjacents(vertex))
			{
				if (!marked[adjacent])
				{
					color[adjacent] = !color[vertex];
					Search(adjacent);
				}
				else if (color[adjacent] == color[vertex])
				{
					IsBipartite = false;
					return;
				}
			}
		}

		bool AllVerticesMarked() => marked.All(vertex => vertex);
	}
}
