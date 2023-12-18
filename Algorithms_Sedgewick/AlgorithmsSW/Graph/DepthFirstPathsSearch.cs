namespace AlgorithmsSW.Graph;

using System.Diagnostics;
using Support;

public sealed class DepthFirstPathsSearch : GraphPathsSearch
{
	public DepthFirstPathsSearch(IReadOnlyGraph graph, int sourceVertex)
		: base(graph, sourceVertex)
	{
		Search(graph, sourceVertex);
	}

#if WITH_INSTRUMENTATION
	public DepthFirstPathsSearch(IGraph graph, int sourceVertex, AlgorithmImplementation implementation)
		: base(graph, sourceVertex)
	{
		switch (implementation)
		{
			case AlgorithmImplementation.Iterative:
				Search(graph, sourceVertex);
				break;
			
			case AlgorithmImplementation.Recursive:
				ReferenceRecursiveSearch(graph, sourceVertex);
				break;
			
			default:
				throw new ArgumentOutOfRangeException(nameof(implementation), implementation, null);
		}
	}
#endif

	private void Search(IReadOnlyGraph graph, int vertex)
	{
		Stack<int> stack = new Stack<int>(graph.VertexCount);
		stack.Push(vertex);
		Marked[vertex] = true;

		while (stack.Count > 0)
		{
			int nextVertex = stack.Pop();

			foreach (int adjacent in graph.GetAdjacents(nextVertex))
			{
				if (!Marked[adjacent])
				{
					Marked[adjacent] = true;
					EdgeOnPathFromSourceTo[adjacent] = nextVertex;
					stack.Push(adjacent);
				}
			}
		}
	}

    [Conditional(Diagnostics.WithInstrumentationDefine)]
	private void ReferenceRecursiveSearch(IReadOnlyGraph graph, int vertex)
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
