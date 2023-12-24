namespace AlgorithmsSW.EdgeWeightedDigraph;

using PriorityQueue;

/// <summary>
/// Algorithm to find the shortest path from a source vertex to all other vertices in a edge weighted digraph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
// Algorithm 4.9
public class Dijkstra<TWeight> : IShortestPath<TWeight>
{
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	private readonly DirectedEdge<TWeight>?[] edgeTo;
	private readonly TWeight[] distTo;
	private readonly IndexPriorityQueue<TWeight> priorityQueue;

	private readonly Func<TWeight, TWeight, TWeight> add;
	private readonly TWeight zero;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Dijkstra{T}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="source">The source vertex to find the shortest paths from.</param>
	/// <param name="add">The function to add two weights.</param>
	/// <param name="zero">The zero value for the weights.</param>
	/// <param name="maxValue">The maximum value for the weights.</param>
	public Dijkstra(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source,
		Func<TWeight, TWeight, TWeight> add, 
		TWeight zero,
		TWeight maxValue)
	{
		this.graph = graph;
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		distTo = new TWeight[graph.VertexCount];
		priorityQueue = new(graph.VertexCount, graph.Comparer);
		this.add = add;
		this.zero = zero;
		
		for (int i = 0; i < graph.VertexCount; i++)
		{
			distTo[i] = maxValue;
		}
		
		distTo[source] = zero;
		priorityQueue.Insert(source, zero);
		
		while (!priorityQueue.IsEmpty)
		{
			Relax(graph, priorityQueue.PopMin().index);
		}
	}

	/// <inheritdoc />
	public TWeight GetDistanceTo(int vertex) => distTo[vertex];
	
	/// <inheritdoc />
	public bool HasPathTo(int vertex) => graph.Comparer.Compare(distTo[vertex], zero) != 0;
	
	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> GetEdgesOfPathTo(int target)
	{
		if (!HasPathTo(target))
		{
			throw new InvalidOperationException($"No path to vertex {target}");
		}
		
		var path = new Stack<DirectedEdge<TWeight>>();
		
		for (var edge = edgeTo[target]; edge != null; edge = edgeTo[edge.Source])
		{
			path.Push(edge);
		}

		return path;
	}

	public Path<TWeight> GetPathTo(int target)
	{
		var path = GetEdgesOfPathTo(target);
		var vertices = path.Select(edge => edge.Source);
		return new([..vertices, target], GetDistanceTo(target));
	}
	
	private void Relax(IReadOnlyEdgeWeightedDigraph<TWeight> graph, int vertex)
	{
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int target = edge.Target;

			if (graph.Comparer.Compare(distTo[target], add(distTo[vertex], edge.Weight)) <= 0)
			{
				continue;
			}

			distTo[target] = add(distTo[vertex], edge.Weight);
			edgeTo[target] = edge;
				
			if (priorityQueue.Contains(target))
			{
				priorityQueue.UpdateValue(target, distTo[target]);
			}
			else
			{
				priorityQueue.Insert(target, distTo[target]);
			}
		}
	}
}
