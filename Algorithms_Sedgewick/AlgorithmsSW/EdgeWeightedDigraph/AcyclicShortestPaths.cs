namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using Digraph;

/// <summary>
/// Algorithm that finds the shortest paths in an edge weighted digraph with no negative cycles.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class AcyclicShortestPaths<TWeight> : IShortestPath<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private readonly DirectedEdge<TWeight>[] edgeTo;
	private readonly TWeight[] distTo;
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="AcyclicShortestPaths{T}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="source">The source vertex to find the shortest paths from.</param>
	public AcyclicShortestPaths(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source)
	{
		this.graph = graph;
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		distTo = new TWeight[graph.VertexCount];
		
		for (int i = 0; i < graph.VertexCount; i++)
		{
			distTo[i] = TWeight.MaxValue;
		}
		
		distTo[source] = TWeight.Zero;
		var topological = new Topological(graph);
		
		foreach (var vertex in topological.Order)
		{
			Relax(vertex);
		}
	}

	/// <inheritdoc />
	public TWeight GetDistanceTo(int vertex) => distTo[vertex];

	/// <inheritdoc />
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
	
	private void Relax(int vertex)
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
		}
	}
}
