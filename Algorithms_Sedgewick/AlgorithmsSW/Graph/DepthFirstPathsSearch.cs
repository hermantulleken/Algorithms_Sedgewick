using System.Diagnostics;
using Support;

namespace AlgorithmsSW.Graphs;

public sealed class DepthFirstPathsSearch : GraphPathsSearch
{
	public DepthFirstPathsSearch(IGraph graph, int sourceVertex) 
		: base(graph, sourceVertex)
	{
	}

	public static DepthFirstPathsSearch Build(IGraph graph, int sourceVertex)
	{
		var search = new DepthFirstPathsSearch(graph, sourceVertex);
		search.Search(graph, sourceVertex);
		
		return search;
	}

#if WITH_INSTRUMENTATION
	public static DepthFirstPathsSearch Build(IGraph graph, int sourceVertex, AlgorithmImplementation implementation)
	{
		var search = new DepthFirstPathsSearch(graph, sourceVertex);
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
	
	private void Search(IGraph graph, int vertex)
	{
		Stack<int> stack = new Stack<int>(graph.VertexCount);
		stack.Push(vertex);

		while (stack.Any())
		{
			int nextVertex = stack.Pop();
			
			Marked[nextVertex] = true;
			stack.Push(nextVertex);

			foreach (int adjacent in graph.GetAdjacents(nextVertex))
			{
				if (!Marked[adjacent])
				{
					EdgeOnPathFromSourceTo[adjacent] = nextVertex;
					stack.Push(adjacent);
				}
			}
		}
	}
	
	[Conditional(Diagnostics.WithInstrumentationDefine)]
	private void ReferenceRecursiveSearch(IGraph graph, int vertex)
	{
		Marked[vertex] = true;
		
		foreach (int adjacent in graph.GetAdjacents(vertex))
		{
			if (!Marked[adjacent])
			{
				EdgeOnPathFromSourceTo[adjacent] = vertex;
				Search(graph, adjacent);
			}
		}
	}
}
