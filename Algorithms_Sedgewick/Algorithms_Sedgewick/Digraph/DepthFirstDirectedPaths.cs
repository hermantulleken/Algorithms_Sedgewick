namespace Algorithms_Sedgewick.Digraphs;

public class DepthFirstDirectedPaths
{
	private	readonly bool[] marked;
	private readonly int[] edgeTo;
	private readonly int source;
	
	public DepthFirstDirectedPaths(IDigraph digraph, int source)
	{
		digraph.ThrowIfNull();
		
		marked = new bool[digraph.VertexCount];
		edgeTo = new int[digraph.VertexCount];
		this.source = source;
		
		Search(digraph, source);
	}
	
	public bool HasPathTo(int vertex) => marked[vertex];
	
	public IEnumerable<int> PathTo(int vertex)
	{
		if (!HasPathTo(vertex))
		{
			throw new InvalidOperationException();
		}
		
		var path = DataStructures.Stack<int>();
		
		for (int current = vertex; current != source; current = edgeTo[current])
		{
			path.Push(current);
		}
		
		path.Push(source);
		
		return path;
	}
	
	private void Search(IDigraph digraph, int vertex)
	{
		marked[vertex] = true;
		
		foreach (int adjacent in digraph.GetAdjacents(vertex))
		{
			if (!marked[adjacent])
			{
				edgeTo[adjacent] = vertex;
				Search(digraph, adjacent);
			}
		}
	}
}
