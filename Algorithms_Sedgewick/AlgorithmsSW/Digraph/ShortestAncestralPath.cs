using AlgorithmsSW.List;

namespace AlgorithmsSW.Digraph;

using System.Diagnostics.CodeAnalysis;

public class ShortestAncestralPath
{
	private readonly int bestVertex;

	private readonly ResizeableArray<int>? path1;
	private readonly ResizeableArray<int>? path2;
	
	[MemberNotNullWhen(true, nameof(path1))]
	[MemberNotNullWhen(true, nameof(path2))]
	public bool HasShortestAncestralPath => bestVertex != -1;
	
	public IEnumerable<int> Path1
	{
		get
		{
			if (!HasShortestAncestralPath)
			{
				throw new InvalidOperationException();
			}
			
			return path1;
		}
	}

	public IEnumerable<int> Path2
	{
		get
		{
			if (!HasShortestAncestralPath)
			{
				throw new InvalidOperationException();
			}
			
			return path2;
		}
	}

	public ShortestAncestralPath(IDigraph digraph, int vertex1, int vertex2)
	{
		var directedCycle = new DirectedCycle(digraph);

		if (directedCycle.HasCycle)
		{
			throw new ArgumentException("Contains cycles.", nameof(digraph));
		}
		
		var paths1 = new BreadthFirstDirectedPaths(digraph, vertex1);
		var paths2 = new BreadthFirstDirectedPaths(digraph, vertex2);

		int minLength = -1; 
		bestVertex = -1;
		
		// I am not sure this is a very good approach
		foreach (int vertex in digraph.Vertexes)
		{
			if (!paths1.HasPathTo(vertex) || !paths2.HasPathTo(vertex))
			{
				continue;
			}
			
			path1 = paths1.PathTo(vertex).ToResizableArray(digraph.VertexCount);
			path2 = paths2.PathTo(vertex).ToResizableArray(digraph.VertexCount);
				
			int length = Path1.Count() + Path2.Count();

			if (minLength != -1 && length >= minLength)
			{
				continue;
			}
				
			minLength = length;
			bestVertex = vertex;
		}
	}
}
