namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using List;
using Support;

public record DirectedPath<TWeight>
	where TWeight : INumber<TWeight>
{
	public ResizeableArray<DirectedEdge<TWeight>> Edges { get; }
	
	public IEnumerable<(int, int)> WeightlessEdges => Edges.Select(edge => (edge.Source, edge.Target));

	public int SourceVertex { get; }
	
	public int TargetVertex { get; }
	
	public IRandomAccessList<int> Vertexes { get; }

	public TWeight Distance { get; }

	private DirectedPath(ResizeableArray<DirectedEdge<TWeight>> edges, TWeight distance, int sourceNode, int targetNode)
	{
		Edges = edges;
		SourceVertex = sourceNode;
		TargetVertex = targetNode;
		Distance = distance;
		
		Vertexes = edges
			.Select(edge => edge.Source)
			.Append(TargetVertex)
			.ToResizableArray();
	}
	
	public DirectedPath(int singleVertex)
		: this([], TWeight.Zero, singleVertex, singleVertex)
	{
	}
	
	public DirectedPath(ResizeableArray<DirectedEdge<TWeight>> edges, TWeight distance)
	{
		Edges = edges;
		SourceVertex = edges[0].Source;
		TargetVertex = edges[^1].Target;
		Distance = distance;
		
		Vertexes = edges
			.Select(edge => edge.Source)
			.Append(TargetVertex)
			.ToResizableArray();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedPath{TWeight}"/> class from a list of edges.
	/// </summary>
	/// <param name="edges">The edges to initialize the path with.</param>
	public DirectedPath(ResizeableArray<DirectedEdge<TWeight>> edges)
		: this(edges, edges.Select(edge => edge.Weight).Aggregate(Add))
	{
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedPath{TWeight}"/> class with a single edge.
	/// </summary>
	/// <param name="edge">The edge to initialize the path with.</param>
	public DirectedPath(DirectedEdge<TWeight> edge) 
		: this([edge], edge.Weight)
	{
	}
	
	/// <summary>
	/// Combines two paths into one.
	/// </summary>
	/// <param name="other">The path to combine with this path. The <see cref="TargetVertex"/> of this path must
	/// equal the <see cref="SourceVertex"/> of the other path.</param>
	/// <returns>A new path that is the combination of this path and the other path.</returns>
	/// <exception cref="ArgumentException">The target vertex of this path is not the source vertex of the other path.</exception>
	/// <remarks>
	/// This path is not modified.
	/// </remarks>
	public DirectedPath<TWeight> Combine(DirectedPath<TWeight> other)
	{
		if (TargetVertex != other.SourceVertex)
		{
			throw new ArgumentException("Paths are not connected", nameof(other));
		}
		
		ResizeableArray<DirectedEdge<TWeight>> newEdges = [..Edges, ..other.Edges];
		var newDistance = Distance + other.Distance;
		
		return new(newEdges, newDistance);
	}
	
	/// <summary>
	/// Combines this path with an edge.
	/// </summary>
	/// <param name="edge">The edge to combine with this path. The <see cref="DirectedEdge{TWeight}.Source"/>
	/// of the edge must equal the <see cref="TargetVertex"/> of this path.</param>
	/// <returns>A new path that is the combination of this path and the edge.</returns>
	/// <exception cref="ArgumentException">The target vertex of this path is not the source vertex of the edge.
	/// </exception>
	/// <remarks>
	/// This path is not modified.
	/// </remarks>
	public DirectedPath<TWeight> Combine(DirectedEdge<TWeight> edge)
	{
		if (TargetVertex != edge.Source)
		{
			throw new ArgumentException("Paths are not connected", nameof(edge));
		}
		
		ResizeableArray<DirectedEdge<TWeight>> newEdges = [..Edges, edge];
		var newDistance = Distance + edge.Weight;
		
		return new(newEdges, newDistance);
	}

	/// <inheritdoc/>
	public override string ToString() => Edges.Pretty();
	
	public DirectedPath<TWeight> Take(int count)
	{
		var edges = Edges.Take(count).ToResizableArray();
		
		return edges.IsEmpty 
			? new(Edges[0].Source) 
			: new(edges);
	}

	public DirectedPath<TWeight> Skip(int count)
	{
		var edges = Edges.Skip(count).ToResizableArray();
		// TODO: write code if we skip beyond one vertex
		return edges.IsEmpty 
			? new(Edges[^1].Target) 
			: new(edges);
	}
	
	public bool HasEqualVertices(DirectedPath<TWeight> other) => Vertexes.SequenceEqual(other.Vertexes);
	
	private static TFloat Add<TFloat>(TFloat x, TFloat y) 
		where TFloat : INumber<TFloat> 
		=> x + y;
}
