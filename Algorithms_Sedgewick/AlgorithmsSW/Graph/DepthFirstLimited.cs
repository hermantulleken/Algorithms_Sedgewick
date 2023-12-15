namespace AlgorithmsSW.Graphs;

// 4.1.26
public class DepthFirstLimited
{
	private bool[] marked;
	private int[] distanceTo;
	private readonly int[] sourceNodes;
	
	private IEnumerable<int> GetPath(int targetVertex)
	{
		var path = new Stack<int>();
		
		for (int vertex = targetVertex; vertex != -1; vertex = sourceNodes[vertex])
		{
			path.Push(vertex);
		}

		return path;
	}

	public DepthFirstLimited(IGraph graph, int sourceVertex, int maxDistance)
	{
		marked = new bool[graph.VertexCount];
		distanceTo = new int[graph.VertexCount];
		sourceNodes = new int[graph.VertexCount];
		
		distanceTo[sourceVertex] = 0;
		Array.Fill(sourceNodes, -1);
		
		var queue = new Queue<int>(graph.VertexCount);
		
		queue.Enqueue(sourceVertex);

		while (queue.Any())
		{
			int item = queue.Dequeue();
			int distance = distanceTo[item];

			if (distance > maxDistance)
			{
				continue;
			}
			
			marked[item] = true;

			var adjacents = graph.GetAdjacents(item);
			
			foreach (int adjacent in adjacents)
			{
				if (marked[adjacent])
				{
					continue;
				}
				
				distanceTo[adjacent] = distance + 1;
				sourceNodes[sourceVertex] = item;
				queue.Enqueue(adjacent);
			}
		}
	}
}
