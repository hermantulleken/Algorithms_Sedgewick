namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics;
using Counter;
using Digraph;
using List;
using Queue;
using Support;
using static System.Diagnostics.Debug;

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

	#if DEBUG
	private bool hasNegativeEdges;
	#endif
	
	public BellmanFord(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int sourceVertex, 
		Func<TWeight, TWeight, TWeight> add,
		TWeight zero, 
		TWeight maxValue)
	{
		graph.ThrowIfNull();
		add.ThrowIfNull();
		
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
		
		while (!queue.IsEmpty && !HasNegativeCycle())
		{
			WhiteBoxTesting.__AddIteration();
			IterationGuard.Inc();
			int vertex = queue.Dequeue();
			onQueue[vertex] = false;
			Relax(vertex, add, zero);
		}
		
		WhiteBoxTesting.__WriteCounts();
		WhiteBoxTesting.__ClearWhiteBoxContainers();
		AssertConsistency(sourceVertex, zero, add);
	}
	
	[Conditional(Diagnostics.DebugDefine)]
	private void AssertConsistency(int sourceVertex, TWeight zero, Func<TWeight, TWeight, TWeight> add)
	{
		ValidatePositiveWeights(zero);

		if (HasNegativeCycle())
		{
			return;
		}
		
		Assert(graph.Comparer.Equal(distanceTo[sourceVertex], zero));
		Assert(IsDistanceConsistent(add));
		Assert(AreEdgesConsistent(sourceVertex));
	}

	[Conditional(Diagnostics.DebugDefine)]
	private void ValidatePositiveWeights(TWeight zero)
	{
#if DEBUG
		if (hasNegativeEdges)
		{
			return;
		}
#endif
		
		foreach (var vertex in graph.Vertexes)
		{
			Assert(graph.Comparer.LessOrEqual(zero, distanceTo[vertex]));
		}
	}

	private void Relax(int vertex, Func<TWeight, TWeight, TWeight> add, TWeight zero)
	{
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			CheckIsNegative(edge, zero);
			
			int target = edge.Target;
			
			if (graph.Comparer.Less(add(distanceTo[vertex], edge.Weight), distanceTo[target]))
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

	[Conditional(Diagnostics.DebugDefine)]
	private void CheckIsNegative(DirectedEdge<TWeight> edge, TWeight zero)
	{
		if (graph.Comparer.Less(edge.Weight, zero))
		{
#if DEBUG
			hasNegativeEdges = true;
#endif
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
	
	public bool HasNegativeCycle() => cycle != null;
	
	public IEnumerable<DirectedEdge<TWeight>> NegativeCycle() 
		=> HasNegativeCycle() ? cycle : throw new InvalidOperationException("No negative cycle.");
	
	private bool IsDistanceConsistent(Func<TWeight, TWeight, TWeight> add)
	{
		foreach (var edge in graph.WeightedEdges)
		{
			if (graph.Comparer.Compare(distanceTo[edge.Target], add(distanceTo[edge.Source], edge.Weight)) > 0)
			{
				return false;
			}
		}
		return true;
	}
	
	private bool AreEdgesConsistent(int sourceVertex)
	{
		for (int v = 0; v < edgeTo.Length; v++)
		{
			if (v == sourceVertex)
			{
				continue;
			}
			if (graph.Comparer.Less(distanceTo[v], maxValue) && edgeTo[v] == null)
			{
				return false;
			}
		}
		
		return true;
	}
}
