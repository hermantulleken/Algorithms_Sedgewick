using AlgorithmsSW.List;

namespace AlgorithmsSW.Graph;

public sealed class BreadthFirstPathsSearch : GraphPathsSearch
{
	private readonly ResizeableArray<int> distances;

	public IReadonlyRandomAccessList<int> Distances => distances;

	public BreadthFirstPathsSearch(IReadOnlyGraph graph, int sourceVertex) 
		: base(graph, sourceVertex)
	{
		distances = new ResizeableArray<int>(graph.VertexCount);
		distances.SetCount(graph.VertexCount);
		distances.Fill(-1);
	}

	public static BreadthFirstPathsSearch Build(IReadOnlyGraph graph, int sourceVertex)
	{
		var search = new BreadthFirstPathsSearch(graph, sourceVertex);
		search.Search(graph, sourceVertex);
		
		return search;
	}

	private void Search(IReadOnlyGraph graph, int vertex)
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

			foreach (int adjacent in graph.GetAdjacents(nextVertex))
			{
				if (!Marked[adjacent])
				{
					EdgeOnPathFromSourceTo[adjacent] = nextVertex;
					vertexQueue.Enqueue(adjacent);
					distanceQueue.Enqueue(nextDistance + 1);
				}
			}
		}
	}
}
