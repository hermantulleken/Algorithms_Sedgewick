using System.Diagnostics;
using Support;

namespace Algorithms_Sedgewick.Graphs;

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
	
	private DepthFirstSearch(IGraph graph)
	{
		marked = new bool[graph.VertexCount];
		Count = 0;
	}

	// Making this method so the constructor does not do heavy processing
	public static DepthFirstSearch Build(IGraph graph, int sourceVertex)
	{
		var search = new DepthFirstSearch(graph);
		
		search.Search(graph, sourceVertex);
		
		return search;
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
