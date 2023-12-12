using System.Diagnostics.CodeAnalysis;
using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick.Graphs;

public class DirectedCycle
{
	private readonly bool[] marked;
	private readonly int[] edgeTo;
	
	// Presumably so that the nodes come out in the right order without an explicit reverse required. Need to test this. 
	private readonly bool[] onStack; 
	
	private StackWithResizeableArray<int>? cycle;
	
	public DirectedCycle(IDigraph graph)
	{
		onStack = new bool[graph.VertexCount];
		edgeTo = new int[graph.VertexCount];
		marked = new bool[graph.VertexCount];

		for (int vertex = 0; vertex < graph.VertexCount; vertex++)
		{
			if (!marked[vertex])
			{
				Search(graph, vertex);
			}
		}
	}

	public void Search(IDigraph graph, int vertex)
	{
		onStack[vertex] = true;
		marked[vertex] = true;

		foreach (int adjacent in graph.GetAdjacents(vertex))
		{
			if (HasCycle())
			{
				return;
			}

			if (!marked[adjacent])
			{
				edgeTo[adjacent] = vertex;
				Search(graph, adjacent);
			}
			else if (onStack[adjacent])
			{
				cycle = new StackWithResizeableArray<int>();

				for (int x = vertex; x != adjacent; x = edgeTo[x])
				{
					cycle.Push(x);
				}

				cycle.Push(adjacent);
				cycle.Push(vertex);
			}

			onStack[vertex] = false;
		}
	}
	
	[MemberNotNullWhen(true, nameof(cycle))]
	public bool HasCycle() => cycle != null;
		
	public IEnumerable<int> Cycle()
	{
		if (!HasCycle())
		{
			throw new InvalidOperationException("No cycle has been found.");
		}
		
		return cycle;
	}
}