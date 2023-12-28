namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;
using Digraph;
using List;
using Queue;
using Support;

/// <summary>
/// A Bellman-Ford shortest path algorithm with a heuristic that skips relaxing a vertex if its parent is on the queue.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/// <remarks>I am not sure this is an improvement over the vanilla <see cref="BellmanFord{TWeight}"/>. In a graph with
/// around 1_000 to 4_000 vertices, with 95% of edges (compared to a complete graph), the relaxing of a vertex is
/// skipped on the order of 10 times.
///
/// In benchmarks, this algorithm is not measurably faster.
///
/// I do wonder if my implementation is correct. 
/// </remarks>
// 4.4.32
public class BellmanFordWithParentCheckingHeuristic<TWeight>
{
	private readonly TWeight[] distanceTo;
	private readonly DirectedEdge<TWeight>?[] edgeTo;
	private readonly bool[] onQueue;
	private readonly IQueue<int> queue;
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	private readonly TWeight maxValue;
	
	private int cost = 0;
	private IEnumerable<DirectedEdge<TWeight>>? cycle;
	
	public BellmanFordWithParentCheckingHeuristic(
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
		
		IterationGuard.Reset();
		WhiteBoxTesting.__ClearWhiteBoxContainers();
		
		while (!queue.IsEmpty && !HasNegativeCycle())
		{
			IterationGuard.Inc();
			WhiteBoxTesting.__AddIteration();
			int vertex = queue.Dequeue();
			onQueue[vertex] = false;
			
			if (edgeTo[vertex] != null && onQueue[edgeTo[vertex]!.Source])
			{
				WhiteBoxTesting.__AddPass();
				continue;
			}
			
			Relax(vertex, add);
			Tracer.Trace(nameof(distanceTo), distanceTo.Pretty());
			Tracer.Trace(nameof(edgeTo), edgeTo.Pretty());
		}
		
		WhiteBoxTesting.__WriteCounts();
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
	
	public IEnumerable<DirectedEdge<TWeight>> NegativeCycle() 
		=> HasNegativeCycle() ? cycle : throw new InvalidOperationException("No negative cycle.");
	
	private void Relax(int vertex, Func<TWeight, TWeight, TWeight> add)
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
	
	private void FindNegativeCycle()
	{
		int vertexCount = edgeTo.Length;
		var shortestPathTree = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(vertexCount, graph.Comparer);
		
		for (int vertex = 0; vertex < vertexCount; vertex++)
		{
			var parent = edgeTo[vertex];
			
			if (parent != null)
			{
				shortestPathTree.AddEdge(parent);
			}
		}

		var cycleFinder = new DirectedCycle(shortestPathTree);
		if (cycleFinder.HasCycle)
		{
			cycle = cycleFinder
				.Cycle()
				.SlidingWindow2()
				.Select(edgeVertices => graph.GetUniqueEdge(edgeVertices.first, edgeVertices.last));
		}
	}
	
	[MemberNotNullWhen(true, nameof(cycle))]
	private bool HasNegativeCycle() => cycle != null;
}
