namespace AlgorithmsSW.EdgeWeightedDigraph;

using Digraph;

/// <summary>
/// Algorithm that finds the shortest paths in an edge weighted digraph with no negative cycles.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public class AcyclicShortestPaths<TWeight> : IShortestPath<TWeight>
{
	private readonly DirectedEdge<TWeight>[] edgeTo;
	private readonly TWeight[] distTo;
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	private readonly TWeight maxValue;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="AcyclicShortestPaths{T}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="source">The source vertex to find the shortest paths from.</param>
	/// <param name="add">The function to add two weights.</param>
	/// <param name="zero">The zero value for the weights.</param>
	/// <param name="maxValue">The maximum value for the weights.</param>
	public AcyclicShortestPaths(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source, 
		Func<TWeight, TWeight, TWeight> add,
		TWeight zero,
		TWeight maxValue)
	{
		this.graph = graph;
		this.maxValue = maxValue;
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		distTo = new TWeight[graph.VertexCount];
		
		for (int i = 0; i < graph.VertexCount; i++)
		{
			distTo[i] = maxValue;
		}
		
		distTo[source] = zero;
		var topological = new Topological(graph);
		
		foreach (var vertex in topological.Order)
		{
			Relax(vertex, add);
		}
	}
	
	private void Relax(int vertex, Func<TWeight, TWeight, TWeight> add)
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
		}
	}

	/// <inheritdoc />
	public TWeight GetDistanceTo(int vertex) => distTo[vertex];

	/// <inheritdoc />
	public bool HasPathTo(int vertex) => graph.Comparer.Compare(distTo[vertex], maxValue) != 0;

	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> GetPathTo(int target)
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
}
