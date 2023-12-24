namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;
using Support;
using static System.Diagnostics.Debug;

// TODO: Make paths store edges instead. 
public record Path<TWeight>(ResizeableArray<int> Vertices, TWeight Distance)
{
	public override string ToString() => Vertices.Pretty();

	public Path<TWeight> Combine(Path<TWeight> other, Func<TWeight, TWeight, TWeight> add)
	{
		ResizeableArray<int> newVertices = [..Vertices, ..other.Vertices];
		var newDistance = add(Distance, other.Distance);
		
		return new(newVertices, newDistance);
	}
}

public class PathComparer<TWeight>(IComparer<TWeight> weightComparer)
	: IComparer<Path<TWeight>>
{
	public int Compare(Path<TWeight>? x, Path<TWeight>? y)
	{
		Assert(x != null);
		Assert(y != null);
			
		return weightComparer.Compare(x.Distance, y.Distance);
	}
}

public record DirectedPath<TWeight>(ResizeableArray<DirectedEdge<TWeight>> edges, TWeight distance)
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DirectedPath{TWeight}"/> class from a list of edges.
	/// </summary>
	/// <param name="edges">The edges to initialize the path with.</param>
	/// <param name="add">The function to add two weights.</param>
	public DirectedPath(ResizeableArray<DirectedEdge<TWeight>> edges, Func<TWeight, TWeight, TWeight> add)
		: this(edges, edges.Select(edge => edge.Weight).Aggregate(add))
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
	/// <param name="other">The path to combine with this path.</param>
	/// <param name="add">A function to add two weights.</param>
	/// <returns>A new path that is the combination of this path and the other path.</returns>
	/// <exception cref="ArgumentException">The lst vertex of this path is not the first vertex of the other path.</exception>
	/// <remarks>
	/// This path is not modified.
	/// </remarks>
	public DirectedPath<TWeight> Combine(DirectedPath<TWeight> other, Func<TWeight, TWeight, TWeight> add)
	{
		if (edges[^1].Target != other.edges[0].Source)
		{
			throw new ArgumentException("Paths are not connected", nameof(other));
		}
		
		ResizeableArray<DirectedEdge<TWeight>> newEdges = [..edges, ..other.edges];
		var newDistance = add(distance, other.distance);
		
		return new(newEdges, newDistance);
	}

	/// <inheritdoc/>
	public override string ToString() => edges.Pretty();
}
