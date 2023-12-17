using Support;

namespace AlgorithmsSW.Graph;

public class Cycle
{
	private readonly bool[] marked;

	public bool HasCycle { get; private set; }

	public Cycle(IGraph graph)
	{
		marked = new bool[graph.VertexCount];
		FindCycles(graph);
	}
	
	private void FindCycles(IGraph graph)
	{
		Tracer.Trace(nameof(FindCycles));
		Tracer.IncLevel();
		
		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			Tracer.TraceIteration("Vertex", vertex);
			if (!marked[vertex])
			{
				Search(graph, vertex, vertex);
			}
		}
		
		Tracer.DecLevel();
	}
	
	private void Search(IGraph graph, int vertex0, int vertex1)
	{
		// TODO: Does this class detect self loops too?
		// Can it halt early?
		// Should we not use one SearchPaths or such instead?
		
		Tracer.IncLevel();
		Tracer.Trace(nameof(Search), (vertex0, vertex1));
		
		marked[vertex0] = true;
		
		Tracer.IncLevel();
		foreach (int adjacent in graph.GetAdjacents(vertex0))
		{
			Tracer.TraceIteration("Adjacent", adjacent);
			if (!marked[adjacent])
			{
				Search(graph, adjacent, vertex0);
			}
			else if (adjacent != vertex1)
			{
				HasCycle = true;
			}
		}
		Tracer.DecLevel();
		Tracer.DecLevel();
	}
}
