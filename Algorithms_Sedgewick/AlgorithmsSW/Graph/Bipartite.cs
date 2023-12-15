using static System.Diagnostics.Debug;

namespace AlgorithmsSW.Graphs;

public class Bipartite
{
	private readonly bool[] color;
	
	private Bipartite(IGraph graph)
	{
		color = new bool[graph.VertexCount];
	}
	
	public static Bipartite Build(IGraph graph)
	{
		graph.ThrowIfNull();
		
		var bipartite = new Bipartite(graph);
		bipartite.ColorGraph(graph);
		return bipartite;
	}

	/// <summary>
	/// Gets a value indicating whether the graph is bipartite.
	/// </summary>
	public bool IsBipartite { get; private set; } = true;
	
	/// <summary>
	/// Gets a value indicating whether the graph has odd cycles.
	/// </summary>
	public bool HasOddCycles => !IsBipartite; // 4.1.33
	
	/// <summary>
	/// Gets the color of the given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to get the color of.</param>
	/// <returns>Either 0 or 1.</returns>
	public bool GetColor(int vertex) => color[vertex];
	
	private void ColorGraph(IGraph graph)
	{
		bool[] marked = new bool[graph.VertexCount];
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
