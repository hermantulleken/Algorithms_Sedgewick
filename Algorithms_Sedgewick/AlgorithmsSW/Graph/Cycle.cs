namespace AlgorithmsSW.Graphs;

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
