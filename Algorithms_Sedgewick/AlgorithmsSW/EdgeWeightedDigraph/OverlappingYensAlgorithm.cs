namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using System.Runtime.CompilerServices;
using Digraph;
using Graph;
using List;
using PriorityQueue;
using Set;
using static System.Diagnostics.Debug;

/// <summary>
/// This is like <see cref="YensAlgorithm{TWeight}"/>, but instead of looking for a fixed number of shortest paths, it
/// looks for shortest paths until the intersection among all paths found is empty. This particular
/// version is used to implement a more efficient version of finding critical edges. 
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/*	Question: Can we unify this with the original? Should we?
	One potential way to do this is to provide the shortest paths lazily. So the user can look for k paths or stop when 
	some other condition is met.
	
	TODO: Do this ^.
*/
[ExerciseReference(4, 4, 7)]
public class OverlappingYensAlgorithm<TWeight> : IKShortestPaths<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private readonly ResizeableArray<DirectedPath<TWeight>> shortestPaths;
	
	// Null when there are no paths between the target and source node.
	public ISet<(int, int)>? CriticalEdges { get; private set; }
	
	public int PathRank { get; private set; } = -1;

	/// <inheritdoc/>
	public bool HasPath(int k) => k >= 0 && k < shortestPaths.Count;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="OverlappingYensAlgorithm{TWeight}"/> class.
	/// </summary>
	/// <param name="digraph">The directed graph in which to find the paths.</param>
	/// <param name="source">The source vertex from where paths originate.</param>
	/// <param name="target">The target vertex to which paths should lead.</param>
	public OverlappingYensAlgorithm(
		IEdgeWeightedDigraph<TWeight> digraph, 
		int source, 
		int target)
	{
		ValidateGraph(digraph);
		
		digraph.ValidateVertex(source);
		digraph.ValidateVertex(target);
		
		shortestPaths = [];
		
		var dijkstra = new Dijkstra<TWeight>(digraph, source);
		
		if (!dijkstra.HasPathTo(target))
		{
			return; // Nothing left to do
		}
		
		var path = dijkstra.GetPathTo(target);
		var intersection = path.WeightlessEdges.ToSet(Comparer<(int, int)>.Default);

		ResizeableArray<DirectedEdge<TWeight>> removedEdges = [];
		shortestPaths.Add(path);
		var queue = DataStructures.PriorityQueue(100, new DirectedPathComparer<TWeight>(Comparer<TWeight>.Default));
		int shortestPathIndex = 1;
		
		while (true)
		{
			FindPotentialShortestPaths(shortestPathIndex);
				
			if (queue.IsEmpty())
			{
				// This handles the case of there being no spur paths, or no spur paths left.
				// This could happen if the spur paths have already been exhausted (added to A), 
				// or there are no spur paths at all - such as when both the source and sink vertices 
				// lie along a "dead end".
				
				break;
			}

			var newPath = queue.PopMin();
			shortestPaths.Add(newPath);

			var potentialEdges = intersection.Difference(newPath.WeightlessEdges.ToSet(Comparer<(int, int)>.Default));

			if (potentialEdges.Count == 0)
			{
				break;
			}

			CriticalEdges = potentialEdges;
			PathRank = shortestPathIndex;
			intersection = intersection.Intersection(newPath.WeightlessEdges);
			
			shortestPathIndex++;
		}
		
		return;

		void RestoreGraph()
		{
			foreach (var edge in removedEdges)
			{
				digraph.AddEdge(edge);
			}

			removedEdges.Clear();
		}

		void AddPotentialPath(DirectedPath<TWeight> rootPath)
		{
			/*	Suppose we have a path from A to B via C.
				We are looking for an alternative path from B to C; this is called the spur path. B is called the
				spur vertex. (Spur in this context simply means alternative.)
				The part between A and B is called the root path. 
				The total path is the root path combined with the spur path. 
			*/ 
			var spurPath = dijkstra.GetPathTo(target);
					
			// These asserts confirm how the paths connect. 
			Assert(rootPath.Vertexes.Last() == spurPath.Vertexes.First());
			Assert(rootPath.Vertexes.Last() != spurPath.Vertexes.Skip(1).First());
					
			var totalPath = rootPath.Combine(spurPath);

			if (!queue.Any(p => p.HasEqualVertices(totalPath)))
			{
				queue.Push(new(totalPath.Edges));
			}
		}
		
		void RemoveOverlappingEdges(DirectedPath<TWeight> rootPath, int pathIndex, int previousPathVertexIndex)
		{
			foreach (var shortestPath in shortestPaths.Take(pathIndex))
			{
				Assert(shortestPath != null);
				Assert(pathIndex >= shortestPaths.Count - 1 || shortestPaths[pathIndex + 1] == null);

				if (rootPath.HasEqualVertices(shortestPath.Take(previousPathVertexIndex)))
				{
					var edge = shortestPath.Edges[previousPathVertexIndex];
					bool wasRemoved = digraph.RemoveEdge(edge);
					
					if (wasRemoved)
					{
						removedEdges.Add(edge);
					}
				}
			}
		}
		
		void RemoveRootPathVertices(DirectedPath<TWeight> rootPath)
		{
			foreach (int rootPathVertex in rootPath.Vertexes.SkipLast(1))
			{
				removedEdges.AddRange(digraph.RemoveVertex(rootPathVertex));
			}
		}

		void FindPotentialShortestPaths(int pathIndex)
		{
			var previousPath = shortestPaths[pathIndex - 1];
			Assert(previousPath != null);
			
			/*	Why do we skip the last two?
				The last vertex in the path is the target. There is no need to find a spur path from the target to
				itself.
				The second-to-last vertex does not offer a new spur path that could lead to an alternative route to the
				target. Any path from this vertex would essentially be part of the already discovered shortest path.
			*/
			for (int previousPathVertexIndex = 0; previousPathVertexIndex < previousPath.Vertexes.Count() - 2; previousPathVertexIndex++)
			{
				int spurVertex = previousPath.Vertexes[previousPathVertexIndex];
				var rootPath = previousPath.Take(previousPathVertexIndex);
				
				Assert(rootPath.Vertexes.Count() == previousPathVertexIndex + 1);
				Assert(rootPath.Vertexes.Last() == spurVertex);

				RemoveOverlappingEdges(rootPath, pathIndex, previousPathVertexIndex);
				RemoveRootPathVertices(rootPath);
				
				dijkstra = new(digraph, spurVertex);
				
				if (dijkstra.HasPathTo(target))
				{
					AddPotentialPath(rootPath);
				}
				
				RestoreGraph();
			}
		}
	}

	private void ValidateGraph(
		IReadOnlyDigraph digraph, 
		[CallerArgumentExpression(nameof(digraph))] string? digraphArgName = null)
	{
		var graph = digraph.AsGraph();

		if (graph.HasParallelEdges())
		{
			throw new ArgumentException("Algorithm does not support parallel edges.");
		}

		if (graph.HasSelfLoops())
		{
			throw new ArgumentException("Algorithm does not support self loops.");
		}
	}

	/// <summary>
	/// Gets the kth shortest path, if it exist, starting from 0, among all shortest paths that have a non-zero
	/// intersection. 
	/// </summary>
	/// <param name="rank">The rank (lowest rank 0 is the shortest) of the path to get. </param>
	/// <returns>The path with the given rank.</returns>
	/// <exception cref="InvalidOperationException">A path of the given rank does not exist.</exception>
	public DirectedPath<TWeight> GetPath(int rank)
	{
		if (!HasPath(rank))
		{
			throw new InvalidOperationException("No path exists");
		}
		
		return shortestPaths[rank];
	}
}
