using System.Diagnostics;
using Support;

namespace AlgorithmsSW.Graphs;

public sealed class DepthFirstSearch
{
	private readonly bool[] marked;
	
	public IEnumerable<int> MarkedVertexes
	{
		get
		{
			for (int i = 0; i < marked.Length; i++)
			{
				if (marked[i])
				{
					yield return i;
				}
			}
		}
	}

	public IReadOnlyList<bool> IsMarked => marked;

	public int Count { get; private set; }
	
	private DepthFirstSearch(IGraph graph, int sourceVertex)
	{
		marked = new bool[graph.VertexCount];
		Count = 0;
		Search(graph, sourceVertex);
	}
	
#if WITH_INSTRUMENTATION
	public static DepthFirstSearch Build(IGraph graph, int sourceVertex, AlgorithmImplementation implementation)
	{
		var search = new DepthFirstSearch(graph);
		
		switch (implementation)
		{
			case AlgorithmImplementation.Iterative:
				search.Search(graph, sourceVertex);
				break;
			
			case AlgorithmImplementation.Recursive:
				search.ReferenceRecursiveSearch(graph, sourceVertex);
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(implementation), implementation, null);
		}
		
		return search;
	}
#endif

	[Conditional(Diagnostics.WithInstrumentationDefine)]
	private void ReferenceRecursiveSearch(IGraph graph, int vertex)
	{
		Mark(vertex);
		
		foreach (int adjacent in graph.GetAdjacents(vertex))
		{
			if (!marked[adjacent])
			{
				ReferenceRecursiveSearch(graph, adjacent);
			}
		}
	}

	private void Search(IGraph graph, int vertex)
	{
		var stack = new Stack<int>();

		Mark(vertex);
		stack.Push(vertex);

		while (stack.Any())
		{
			int current = stack.Pop();
			
			foreach (int adjacent in graph.GetAdjacents(current))
			{
				if (!marked[adjacent])
				{
					Mark(adjacent);
					stack.Push(adjacent);
				}
			}
		}
	}

	private void Mark(int vertex)
	{
		marked[vertex] = true;
		Count++;
	}
}
