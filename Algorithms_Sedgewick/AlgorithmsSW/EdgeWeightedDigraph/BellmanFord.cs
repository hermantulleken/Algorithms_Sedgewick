namespace AlgorithmsSW.EdgeWeightedDigraph;

using Digraph;
using List;
using Queue;

public class BellmanFord<TWeight> : IShortestPath<TWeight>
{
	private readonly TWeight[] distanceTo;
	private readonly DirectedEdge<TWeight>?[] edgeTo;
	private readonly bool[] onQueue;
	private readonly IQueue<int> queue;
	private int cost = 0;
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	private readonly TWeight maxValue;
	private IEnumerable<DirectedEdge<TWeight>>? cycle;
	
	public BellmanFord(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int sourceVertex, 
		Func<TWeight, TWeight, TWeight> add,
		TWeight zero, 
		TWeight maxValue)
	{
		this.graph = graph;
		this.maxValue = maxValue;
		distanceTo = new TWeight[graph.VertexCount];
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		onQueue = new bool[graph.VertexCount];
		queue = DataStructures.Queue<int>();

		foreach (int vertex in graph.Vertexes)
		{
			distanceTo[vertex] = maxValue;
		}
		
		distanceTo[sourceVertex] = zero;
		queue.Enqueue(sourceVertex);
		onQueue[sourceVertex] = true;

		while (!queue.IsEmpty)
		{
			int vertex = queue.Dequeue();
			onQueue[vertex] = false;
			Relax(graph, vertex, add);
		}
	}

	private void Relax(IReadOnlyEdgeWeightedDigraph<TWeight> graph, int vertex, Func<TWeight, TWeight, TWeight> add)
	{
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int target = edge.Target;
			
			if (graph.Comparer.Compare(distanceTo[target], add(distanceTo[vertex], edge.Weight)) > 0)
			{
				distanceTo[target] = add(distanceTo[vertex], edge.Weight);
				edgeTo[target] = edge; 

				if (!onQueue[target])
				{
					queue.Enqueue(target);
					onQueue[target] = true;
				}
			}
			
			if (cost++ % graph.VertexCount == 0)
			{
				FindNegativeCycle();
			}
		}
	}
	
	public TWeight GetDistanceTo(int vertex) => distanceTo[vertex];
	
	public bool HasPathTo(int vertex) => graph.Comparer.Compare(distanceTo[vertex], maxValue) < 0;

	public IEnumerable<DirectedEdge<TWeight>> GetEdgesOfPathTo(int target)
	{
		if (!HasPathTo(target))
		{
			throw new InvalidOperationException($"No path to vertex {target}.");
		}
		
		var path = new Stack<DirectedEdge<TWeight>>();
		
		for (var edge = edgeTo[target]; edge != null; edge = edgeTo[edge.Source])
		{
			path.Push(edge);
		}

		return path;
	}
	
	private void FindNegativeCycle()
	{
		int vertexCount = edgeTo.Length;
		var spt = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(vertexCount, graph.Comparer);
		
		for (int vertex = 0; vertex < vertexCount; vertex++)
		{
			if (edgeTo[vertex] != null)
			{
				spt.AddEdge(edgeTo[vertex]);
			}
		}

		var cycleFinder = new DirectedCycle(spt);
		if (cycleFinder.HasCycle)
		{
			cycle = cycleFinder
				.Cycle()
				.SlidingWindow2()
				.Select(edgeVertices => graph.GetUniqueEdge(edgeVertices.first, edgeVertices.last));
		}
	}
	
	private bool HasNegativeCycle() => cycle != null;
	
	public IEnumerable<DirectedEdge<TWeight>> NegativeCycle() 
		=> HasNegativeCycle() ? cycle : throw new InvalidOperationException("No negative cycle.");
}
