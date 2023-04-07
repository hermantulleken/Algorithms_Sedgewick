using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Support;

namespace Algorithms_Sedgewick.Graphs;

[SuppressMessage(
	"StyleCop.CSharp.MaintainabilityRules", 
	"SA1401:Fields should be private", 
	Justification = Tools.AbstractClassWithProtectedFields)]
public abstract class GraphPathsSearch
{
	protected readonly bool[] Marked;

	protected readonly int[] EdgeOnPathFromSourceTo;
	
	protected readonly int SourceVertex;
	
	public IReadOnlyList<bool> HasPathTo => Marked;
	
	public IEnumerable<int> MarkedVertexes
	{
		get
		{
			for (int i = 0; i < Marked.Length; i++)
			{
				if (Marked[i])
				{
					yield return i;
				}
			}
		}
	}

	public GraphPathsSearch(IGraph graph, int sourceVertex)
	{
		Marked = new bool[graph.VertexCount];
		EdgeOnPathFromSourceTo = new int[graph.VertexCount];
		SourceVertex = sourceVertex;
	}
	
	public IEnumerable<int>? GetPathTo(int targetVertex)
	{
		if (!HasPathTo[targetVertex])
		{
			return null;
		}
		
		var path = new Stack<int>();
		
		for (int pathVertex = targetVertex; pathVertex != SourceVertex; pathVertex = EdgeOnPathFromSourceTo[pathVertex])
		{
			path.Push(pathVertex);
		}
		
		path.Push(SourceVertex);
		
		return path;
	}
}

public sealed class BreadthFirstPathsSearch : GraphPathsSearch
{
	public BreadthFirstPathsSearch(IGraph graph, int sourceVertex) 
		: base(graph, sourceVertex)
	{
	}

	public static BreadthFirstPathsSearch Build(IGraph graph, int sourceVertex)
	{
		var search = new BreadthFirstPathsSearch(graph, sourceVertex);
		search.Search(graph, sourceVertex);
		
		return search;
	}

	private void Search(IGraph graph, int vertex)
	{
		Queue<int> stack = new Queue<int>(graph.VertexCount);
		stack.Enqueue(vertex);

		while (stack.Any())
		{
			int nextVertex = stack.Dequeue();
			
			Marked[nextVertex] = true;
			stack.Enqueue(nextVertex);

			foreach (int adjacent in graph.GetAdjacents(nextVertex))
			{
				if (!Marked[adjacent])
				{
					EdgeOnPathFromSourceTo[adjacent] = nextVertex;
					stack.Enqueue(adjacent);
				}
			}
		}
	}
}

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
