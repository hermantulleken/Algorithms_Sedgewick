namespace AlgorithmsSW.Graph;

public class GraphProperties
{
	private readonly int[] eccentricities;

	public int Radius { get; private set; }

	public int Diameter { get; private set; }
	
	public int Center { get; private set; }
	
	public int Girth { get; private set; }

	private GraphProperties(IGraph graph)
	{
		eccentricities = new int[graph.VertexCount];
	}

	public static GraphProperties Build(IGraph graph)
	{
		var graphProperties = new GraphProperties(graph);
		graphProperties.CalculateEccentricities(graph);
		graphProperties.CalculateGirth(graph);

		return graphProperties;
	}

	public int GetEccentricity(int vertex)
	{
		vertex.ThrowIfOutOfRange(eccentricities.Length);
		
		return eccentricities[vertex];
	}

	private static int CalculateEccentricity(IGraph graph, int sourceVertex)
	{
		var graphPaths = BreadthFirstPathsSearch.Build(graph, sourceVertex);
		int longestDistance = 0;

		foreach (int targetVertex in graph.Vertexes)
		{
			if (!graphPaths.HasPathTo[targetVertex])
			{
				continue;
			}

			// This is the shortest path because we are using breadth first search.
			if (!graphPaths.TryGetPathTo(targetVertex, out var path))
			{
				continue;
			}

			int distance = path.Count();

			if (distance > longestDistance)
			{
				longestDistance = distance;
			}
		}

		return longestDistance;
	}
	
	// 4.1.16
	private void CalculateEccentricities(IGraph graph)
	{
		Radius = graph.VertexCount; // Note: Always bigger than maximum distance
		Diameter = 0; 
		
		foreach (int sourceVertex in graph.Vertexes)
		{
			int eccentricity = CalculateEccentricity(graph, sourceVertex);
			eccentricities[sourceVertex] = eccentricity;

			// Question: This gives us a radius of 0 if there is any isolated vertex. Is this correct?
			if (eccentricity < Radius)
			{
				Radius = eccentricity;
				Center = sourceVertex;
			}

			if (eccentricity > Diameter)
			{
				Diameter = eccentricity;
			}
		}
	}

	// 4.1.18
	private void CalculateGirth(IGraph graph)
	{
		Girth = FindCycleLength(graph, 0);

		for (int i = 1; i < graph.VertexCount; i++)
		{
			int newGirth = FindCycleLength(graph, i);

			if (newGirth < Girth)
			{
				Girth = newGirth;
			}
		}
	}
	
	private int FindCycleLength(IGraph graph, int sourceVertex)
	{
		bool[] marked = new bool[graph.VertexCount];
		var queue = new Queue<int>();
		var distanceQueue = new Queue<int>();
		
		queue.Enqueue(sourceVertex);
		marked[sourceVertex] = true;
		distanceQueue.Enqueue(0);

		while (queue.Any())
		{
			int currentVertex = queue.Dequeue();
			int distance = distanceQueue.Dequeue() + 1;
			
			foreach (var adjacent in graph.GetAdjacents(currentVertex))
			{
				if (adjacent == sourceVertex)
				{
					return distance;
				}

				if (marked[adjacent])
				{
					continue;
				}
				
				marked[adjacent] = true;
				queue.Enqueue(adjacent);
				distanceQueue.Enqueue(distance);
			}
		}

		return graph.VertexCount + 1; // Represents infinity; larger than all possible cycles. 
	}
}
