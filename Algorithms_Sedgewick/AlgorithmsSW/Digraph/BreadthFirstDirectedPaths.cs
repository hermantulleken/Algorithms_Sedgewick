namespace AlgorithmsSW.Digraph;

public class BreadthFirstDirectedPaths
{
	private readonly bool[] marked;
	private readonly int[] edgeTo;
	private readonly int source;
	
	public BreadthFirstDirectedPaths(IDigraph digraph, int source)
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
		var queue = DataStructures.Queue<int>();
		
		marked[vertex] = true;
		queue.Enqueue(vertex);
		
		while (!queue.IsEmpty)
		{
			int current = queue.Dequeue();
			
			foreach (int adjacent in digraph.GetAdjacents(current))
			{
				if (marked[adjacent])
				{
					continue;
				}
				
				edgeTo[adjacent] = current;
				marked[adjacent] = true;
				queue.Enqueue(adjacent);
			}
		}
	}
}	
