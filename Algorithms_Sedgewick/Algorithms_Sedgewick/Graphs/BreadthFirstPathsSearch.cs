namespace Algorithms_Sedgewick.Graphs;

public sealed class BreadthFirstPathsSearch : GraphPathsSearch
{
	private readonly int[] distances;

	public IReadOnlyList<int> Distances => distances;

	public BreadthFirstPathsSearch(IGraph graph, int sourceVertex) 
		: base(graph, sourceVertex)
	{
		distances = new int[graph.VertexCount];

		for (int i = 0; i < distances.Length; i++)
		{
			distances[i] = -1;
		}
	}

	public static BreadthFirstPathsSearch Build(IGraph graph, int sourceVertex)
	{
		var search = new BreadthFirstPathsSearch(graph, sourceVertex);
		search.Search(graph, sourceVertex);
		
		return search;
	}

	private void Search(IGraph graph, int vertex)
	{
		var vertexQueue = new Queue<int>(graph.VertexCount);
		vertexQueue.Enqueue(vertex);

		var distanceQueue = new Queue<int>(graph.VertexCount);
		distanceQueue.Enqueue(0);

		while (vertexQueue.Any())
		{
			int nextVertex = vertexQueue.Dequeue();
			int nextDistance = distanceQueue.Dequeue();
			
			Marked[nextVertex] = true;
			distances[nextVertex] = nextDistance;
			vertexQueue.Enqueue(nextVertex);

			foreach (int adjacent in graph.GetAdjacents(nextVertex))
			{
				if (!Marked[adjacent])
				{
					EdgeOnPathFromSourceTo[adjacent] = nextVertex;
					vertexQueue.Enqueue(adjacent);
					vertexQueue.Enqueue(nextDistance + 1);
				}
			}
		}
	}
}
