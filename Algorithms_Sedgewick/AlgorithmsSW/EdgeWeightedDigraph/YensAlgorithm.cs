namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using List;
using PriorityQueue;
using static System.Diagnostics.Debug;

/// <summary>
/// An implementation of Yen's algorithm of finding the k shortest paths between vertices in a directed edge weighted
/// graph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
/*	Implementation note: This is an example of an algorithm that is too difficult (or obscure) for ChatGPT to implement,
 	as of Dec 2023.
*/
[ExerciseReference(4, 4, 7)]
public class YensAlgorithm<TWeight> 
	: IKShortestPaths<TWeight>
	where TWeight : IFloatingPoint<TWeight>, IMinMaxValue<TWeight>
{
	private readonly DirectedPath<TWeight>?[] shortestPaths;
	
	/// <inheritdoc/>
	public bool HasPath(int k) => k >= 0 && k < shortestPaths.Length && shortestPaths[k] != null;

	/// <summary>
	/// Initializes a new instance of the <see cref="YensAlgorithm{TWeight}"/> class.
	/// </summary>
	/// <param name="digraph">The directed graph in which to find the paths.</param>
	/// <param name="source">The source vertex from where paths originate.</param>
	/// <param name="target">The target vertex to which paths should lead.</param>
	/// <param name="shortestPathCount">The number of shortest paths to find.</param>
	public YensAlgorithm(IEdgeWeightedDigraph<TWeight> digraph, int source, int target, int shortestPathCount)
	{
		shortestPaths = new DirectedPath<TWeight>[shortestPathCount];
		var dijkstra = new Dijkstra<TWeight>(digraph, source);
		
		if (!dijkstra.HasPathTo(target))
		{
			return; // Nothing left to do
		}
		
		var path = dijkstra.GetPathTo(target);
		ResizeableArray<DirectedEdge<TWeight>> removedEdges = [];
		shortestPaths[0] = path;
		var queue = DataStructures.PriorityQueue(100, new DirectedPathComparer<TWeight>(Comparer<TWeight>.Default));
		
		for (int shortestPathIndex = 1; shortestPathIndex < shortestPathCount; shortestPathIndex++)
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

			shortestPaths[shortestPathIndex] = queue.PopMin();
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

		void RemoveOverlappingEdges(DirectedPath<TWeight> rootPath, int shortestPathIndex, int previousPathVertexIndex)
		{
			foreach (var shortestPath in shortestPaths.Take(shortestPathIndex))
			{
				Assert(shortestPath != null);
				Assert(shortestPathIndex >= shortestPaths.Length - 1 || shortestPaths[shortestPathIndex + 1] == null);

				if (rootPath.HasEqualVertices(shortestPath.Take(previousPathVertexIndex)))
				{
					var edge = shortestPath.Edges[previousPathVertexIndex];
					digraph.RemoveEdge(edge);
					removedEdges.Add(edge);
				}
			}
		}
		
		void RemoveRootPathVertices(DirectedPath<TWeight> rootPath)
		{
			foreach (int rootPahvertex in rootPath.Vertexes.SkipLast(1))
			{
				removedEdges.AddRange(digraph.RemoveVertex(rootPahvertex));
			}
		}

		void FindPotentialShortestPaths(int shortestPathIndex)
		{
			var previousPath = shortestPaths[shortestPathIndex - 1];
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

				RemoveOverlappingEdges(rootPath, shortestPathIndex, previousPathVertexIndex);
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

	public DirectedPath<TWeight> GetPath(int rank)
	{
		if (!HasPath(rank))
		{
			throw new InvalidOperationException("No path exists");
		}
		
		return shortestPaths[rank]!;
	}
}
