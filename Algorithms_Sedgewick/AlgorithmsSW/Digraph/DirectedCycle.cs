namespace AlgorithmsSW.Digraphs;

using System.Diagnostics.CodeAnalysis;
using Stack;

/// <summary>
/// Provides an algorithm for finding a directed cycle in a digraph== if one exists. 
/// </summary>
public class DirectedCycle
{
	private readonly bool[] marked;
	private readonly int[] edgeTo;
	
	// Presumably so that the nodes come out in the right order without an explicit reverse required. Need to test this. 
	private readonly bool[] onStack; 
	
	private StackWithResizeableArray<int>? cycle;
	
	/// <summary>
	/// Gets a value indicating whether the graph has a directed cycle.
	/// </summary>
	[MemberNotNullWhen(true, nameof(cycle))]
	public bool HasCycle => cycle != null;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedCycle"/> class.
	/// </summary>
	/// <param name="digraph">The graph to find a cycle in.</param>
	public DirectedCycle(IDigraph digraph)
	{
		onStack = new bool[digraph.VertexCount];
		edgeTo = new int[digraph.VertexCount];
		marked = new bool[digraph.VertexCount];

		for (int vertex = 0; vertex < digraph.VertexCount; vertex++)
		{
			if (!marked[vertex])
			{
				Search(digraph, vertex);
			}
		}
	}
	
	/// <summary>
	/// Returns the vertices in a directed cycle, if one exists.
	/// </summary>
	/// <exception cref="InvalidOperationException">No cycle has been found.</exception>
	public IEnumerable<int> Cycle()
	{
		if (!HasCycle)
		{
			throw new InvalidOperationException("No cycle has been found.");
		}
		
		return cycle;
	}

	private void Search(IDigraph digraph, int vertex)
	{
		onStack[vertex] = true;
		marked[vertex] = true;

		foreach (int adjacent in digraph.GetAdjacents(vertex))
		{
			if (HasCycle)
			{
				return;
			}

			if (!marked[adjacent])
			{
				edgeTo[adjacent] = vertex;
				Search(digraph, adjacent);
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
}
