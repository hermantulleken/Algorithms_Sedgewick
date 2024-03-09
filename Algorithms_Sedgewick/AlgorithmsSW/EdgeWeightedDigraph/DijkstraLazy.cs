namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using PriorityQueue;

/// <summary>
/// Algorithm to find the shortest path from a source vertex to all other vertices in a edge weighted digraph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
[ExerciseReference(4, 4, 39)]
public class DijkstraLazy<TWeight> : IShortestPath<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private class NodeComparer : IComparer<(int node, TWeight weight)>
	{
		public int Compare((int node, TWeight weight) x, (int node, TWeight weight) y)
		{
			int item1Comparison = Comparer<int>.Default.Compare(x.node, y.node);
			
			if (item1Comparison != 0)
			{
				return item1Comparison;
			}

			return Comparer<TWeight>.Default.Compare(x.weight, y.weight);
		}
	}
	
	private readonly DirectedEdge<TWeight>?[] edgeTo;
	private readonly TWeight[] distTo;
	private readonly IPriorityQueue<(int node, TWeight weight)> priorityQueue;

	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraLazy{T}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="source">The source vertex to find the shortest paths from.</param>
	public DijkstraLazy(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source)
	{
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		distTo = new TWeight[graph.VertexCount];
		priorityQueue = DataStructures.PriorityQueue(graph.VertexCount, new NodeComparer());
		var expandedNodes = DataStructures.Set(Comparer<int>.Default);
		
		for (int i = 0; i < graph.VertexCount; i++)
		{
			distTo[i] = TWeight.MaxValue;
		}
		
		distTo[source] = TWeight.Zero;
		priorityQueue.Push((source, TWeight.Zero));
		
		while (!priorityQueue.IsEmpty())
		{
			int nextNode = priorityQueue.PopMin().node;

			if (expandedNodes.Contains(nextNode))
			{
				continue;
			}
			
			expandedNodes.Add(nextNode);
			
			Relax(graph, nextNode);
		}
	}

	/// <inheritdoc />
	public TWeight GetDistanceTo(int vertex) => distTo[vertex];
	
	/// <inheritdoc />
	// TODO: this is not a robust test!
	public bool HasPathTo(int vertex) => distTo[vertex] != TWeight.MaxValue;
	
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

	public DirectedPath<TWeight> GetPathTo(int target)
	{
		var path = GetEdgesOfPathTo(target).ToResizableArray();
		
		return new(path);
	}
	
	private void Relax(IReadOnlyEdgeWeightedDigraph<TWeight> graph, int vertex)
	{
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int target = edge.Target;

			if (distTo[target] <= distTo[vertex] + edge.Weight)
			{
				continue;
			}

			distTo[target] = distTo[vertex] + edge.Weight;
			edgeTo[target] = edge;
				
			priorityQueue.Push((target, distTo[target]));
		}
	}
}
