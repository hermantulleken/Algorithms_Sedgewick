namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics;
using System.Numerics;
using Digraph;
using List;
using Queue;
using Support;
using static System.Diagnostics.Debug;

public class BellmanFord<TWeight> : IShortestPath<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private readonly TWeight[] distanceTo;
	private readonly DirectedEdge<TWeight>?[] edgeTo;
	private readonly bool[] onQueue;
	private readonly IQueue<int> queue;
	private int cost = 0;
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	private IEnumerable<DirectedEdge<TWeight>>? cycle;

	#if DEBUG
	private bool hasNegativeEdges;
	#endif
	
	public BellmanFord(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int sourceVertex)
	{
		graph.ThrowIfNull();
		
		this.graph = graph;
		distanceTo = new TWeight[graph.VertexCount];
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		onQueue = new bool[graph.VertexCount];
		queue = DataStructures.Queue<int>();

		foreach (int vertex in graph.Vertexes)
		{
			distanceTo[vertex] = TWeight.MaxValue;
		}
		
		distanceTo[sourceVertex] = TWeight.Zero;
		queue.Enqueue(sourceVertex);
		onQueue[sourceVertex] = true;

		IterationGuard.Reset();
		
		while (!queue.IsEmpty && !HasNegativeCycle())
		{
			WhiteBoxTesting.__AddIteration();
			IterationGuard.Inc();
			int vertex = queue.Dequeue();
			onQueue[vertex] = false;
			Relax(vertex);
		}
		
		WhiteBoxTesting.__WriteCounts();
		WhiteBoxTesting.__ClearWhiteBoxContainers();
		AssertConsistency(sourceVertex);
	}
	
	[Conditional(Diagnostics.DebugDefine)]
	private void AssertConsistency(int sourceVertex)
	{
		ValidatePositiveWeights();

		if (HasNegativeCycle())
		{
			return;
		}
		
		Assert(distanceTo[sourceVertex] == TWeight.Zero);
		Assert(IsDistanceConsistent());
		Assert(AreEdgesConsistent(sourceVertex));
	}

	[Conditional(Diagnostics.DebugDefine)]
	private void ValidatePositiveWeights()
	{
#if DEBUG
		if (hasNegativeEdges)
		{
			return;
		}
#endif
		
		foreach (int vertex in graph.Vertexes)
		{
			Assert(!TWeight.IsNegative(distanceTo[vertex]));
		}
	}

	private void Relax(int vertex)
	{
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			CheckIsNegative(edge);
			
			int target = edge.Target;
			
			if (distanceTo[vertex] + edge.Weight < distanceTo[target])
			{
				distanceTo[target] = distanceTo[vertex] + edge.Weight;
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
	private void CheckIsNegative(DirectedEdge<TWeight> edge)
	{
		if (TWeight.IsNegative(edge.Weight))
		{
#if DEBUG
			hasNegativeEdges = true;
#endif
		}
	}

	public TWeight GetDistanceTo(int vertex) => distanceTo[vertex];
	
	public bool HasPathTo(int vertex) => distanceTo[vertex] < TWeight.MaxValue;

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
		var spt = new EdgeWeightedDigraphWithAdjacencyLists<TWeight>(vertexCount);
		
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
	
	private bool IsDistanceConsistent()
	{
		foreach (var edge in graph.WeightedEdges)
		{
			if (distanceTo[edge.Target] > distanceTo[edge.Source] + edge.Weight)
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
			if (distanceTo[v] < TWeight.MaxValue && edgeTo[v] == null)
			{
				return false;
			}
		}
		
		return true;
	}
}
