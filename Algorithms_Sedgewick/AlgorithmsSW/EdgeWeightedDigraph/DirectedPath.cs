namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;
using Support;

public record DirectedPath<TWeight>
{
	public ResizeableArray<DirectedEdge<TWeight>> Edges { get; }

	public int SourceVertex { get; }
	
	public int TargetVertex { get; }
	
	public IRandomAccessList<int> Vertexes { get; }

	public TWeight Distance { get; }

	public DirectedPath(ResizeableArray<DirectedEdge<TWeight>> Edges, TWeight Distance)
	{
		this.Edges = Edges;
		SourceVertex = Edges[0].Source;
		TargetVertex = Edges[^1].Target;
		this.Distance = Distance;
		
		Vertexes = Edges
			.Select(edge => edge.Source)
			.Append(TargetVertex)
			.ToResizableArray();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedPath{TWeight}"/> class from a list of edges.
	/// </summary>
	/// <param name="Edges">The edges to initialize the path with.</param>
	/// <param name="add">The function to add two weights.</param>
	public DirectedPath(ResizeableArray<DirectedEdge<TWeight>> Edges, Func<TWeight, TWeight, TWeight> add)
		: this(Edges, Edges.Select(edge => edge.Weight).Aggregate(add))
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
	/// <param name="add">A function to add two weights.</param>
	/// <returns>A new path that is the combination of this path and the other path.</returns>
	/// <exception cref="ArgumentException">The target vertex of this path is not the source vertex of the other path.</exception>
	/// <remarks>
	/// This path is not modified.
	/// </remarks>
	public DirectedPath<TWeight> Combine(DirectedPath<TWeight> other, Func<TWeight, TWeight, TWeight> add)
	{
		if (TargetVertex != other.SourceVertex)
		{
			throw new ArgumentException("Paths are not connected", nameof(other));
		}
		
		ResizeableArray<DirectedEdge<TWeight>> newEdges = [..Edges, ..other.Edges];
		var newDistance = add(Distance, other.Distance);
		
		return new(newEdges, newDistance);
	}
	
	/// <summary>
	/// Combines this path with an edge.
	/// </summary>
	/// <param name="edge">The edge to combine with this path. The <see cref="DirectedEdge{TWeight}.Source"/>
	/// of the edge must equal the <see cref="TargetVertex"/> of this path.</param>
	/// <param name="add">The function to add two weights.</param>
	/// <returns>A new path that is the combination of this path and the edge.</returns>
	/// <exception cref="ArgumentException">The target vertex of this path is not the source vertex of the edge.
	/// </exception>
	/// <remarks>
	/// This path is not modified.
	/// </remarks>
	public DirectedPath<TWeight> Combine(DirectedEdge<TWeight> edge, Func<TWeight, TWeight, TWeight> add)
	{
		if (TargetVertex != edge.Source)
		{
			throw new ArgumentException("Paths are not connected", nameof(edge));
		}
		
		ResizeableArray<DirectedEdge<TWeight>> newEdges = [..Edges, edge];
		var newDistance = add(Distance, edge.Weight);
		
		return new(newEdges, newDistance);
	}

	/// <inheritdoc/>
	public override string ToString() => Edges.Pretty();
	
	public DirectedPath<TWeight> Take(int count, Func<TWeight, TWeight, TWeight> add) => new(Edges.Take(count).ToResizableArray(), add);
	
	public DirectedPath<TWeight> Skip(int count, Func<TWeight, TWeight, TWeight> add) => new(Edges.Skip(count).ToResizableArray(), add);
}
