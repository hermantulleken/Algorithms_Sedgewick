using Algorithms_Sedgewick.List;
using static System.Diagnostics.Debug;

namespace Algorithms_Sedgewick.Digraphs;

public class EulerianCycle
{
	private readonly ResizeableArray<int> path;
	
	public bool HasEulerianCycle { get; }

	public IEnumerable<int> Path
	{
		get
		{
			if (!HasEulerianCycle)
			{
				throw new InvalidOperationException();
			}
			
			return path;
		}
	}

	public EulerianCycle(IDigraph digraph)
	{
		HasEulerianCycle = IsConnected(digraph) && InDegreesMathOutDegrees(digraph);
		
		if (!HasEulerianCycle)
		{
			return;
		}

		bool[] marked = new bool[digraph.VertexCount];
		path = new ResizeableArray<int>();

		int node = 0;
		int length = 0;
		path.Add(node);
		
		while (length < digraph.VertexCount)
		{
			foreach (int adjacent in digraph.GetAdjacents(node))
			{
				if (marked[adjacent])
				{
					continue;
				}
				
				marked[adjacent] = true;
				node = adjacent;
				path.Add(node);
				length++;
				break;
			}
		}
		
		Assert(node == 0);
		path.RemoveLast(); // Since this is a cycle the node has already been added
	}

	private bool IsConnected(IDigraph digraph)
	{
		var dfs = new DirectedDepthFirstSearch(digraph, 0);
		return dfs.IsConnectedToSources;
	}

	private bool InDegreesMathOutDegrees(IDigraph digraph)
	{
		var degrees = new Degrees(digraph);
		
		for (int i = 0; i < digraph.VertexCount; i++)
		{
			if (degrees.GetIndegree(i) == degrees.GetOutdegree(i))
			{
				continue;
			}

			return false;
		}

		return true;
	}
}
