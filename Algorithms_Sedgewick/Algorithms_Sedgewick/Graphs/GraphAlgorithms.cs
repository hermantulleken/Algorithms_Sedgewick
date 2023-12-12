using System.Diagnostics;
using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick.Graphs;

public class GraphAlgorithms
{
	public static int FindNodeSafeToDelete(IGraph graph)
	{
		var stack = new FixedCapacityStack<int>(graph.Count());
		bool[] marked = new bool[graph.VertexCount];
		
		stack.Push(0); // Assumes contains vertex 0

		while (stack.Any())
		{
			var vertex = stack.Pop();

			marked[vertex] = true;

			var adjacents = graph.GetAdjacents(vertex);

			if (adjacents.All(a => marked[a]))
			{
				return vertex;
			}

			foreach (var adjacent in adjacents)
			{
				if (!marked[adjacent])
				{
					stack.Push(adjacent);
				}
			}
		}

		// Impossible to reach
		Debug.Assert(false);
		return -1;
	}
}
