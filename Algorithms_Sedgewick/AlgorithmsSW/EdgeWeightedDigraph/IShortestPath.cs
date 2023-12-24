namespace AlgorithmsSW.EdgeWeightedDigraph;

/// <summary>
/// Algorithm to find the shortest path from a source vertex to all other vertices in a edge weighted digraph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
public interface IShortestPath<TWeight>
{
	/// <summary>
	/// Gets the distance from the source vertex to the given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to find the distance to.</param>
	/// <returns>The distance from the source vertex to the given vertex.</returns>
	TWeight GetDistanceTo(int vertex);
	
	/// <summary>
	/// Whether there is a path from the source vertex to the given vertex.
	/// </summary>
	/// <param name="vertex">The vertex to check if there is a path to.</param>
	/// <returns><see langword="true"/> if there is a path from the source vertex to the given vertex; otherwise, <see langword="false"/>.</returns>
	bool HasPathTo(int vertex);
	
	/// <summary>
	/// Gets the path from the source vertex to the given vertex.
	/// </summary>
	/// <param name="target">The vertex to find the path to.</param>
	/// <returns>An enumerable of edges representing the path from the source vertex to the given vertex.</returns>
	/// <exception cref="InvalidOperationException">+there is no path from the source vertex to the given vertex.</exception>
	IEnumerable<DirectedEdge<TWeight>> GetEdgesOfPathTo(int target);
}
