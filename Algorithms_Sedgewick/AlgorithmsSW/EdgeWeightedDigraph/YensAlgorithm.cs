namespace AlgorithmsSW.EdgeWeightedGraph;

using EdgeWeightedDigraph;
using List;
using PriorityQueue;
using static System.Diagnostics.Debug;

public interface IKShortestPaths<TWeight>
{
	bool HasPath(int k);
	
	DirectedPath<TWeight> GetPath(int i);
}

public class YensAlgorithm : IKShortestPaths<double>
{
	private Func<double, double, double> add = (x, y) => x + y;

	private IComparer<double> comparer = Comparer<double>.Default;
	private double zero = 0.0;
	private double maxValue = double.MaxValue;
	private DirectedPath<double>?[] A;
	
	public YensAlgorithm(IEdgeWeightedDigraph<double> digraph, int source, int target, int K)
	{
		A = new DirectedPath<double>[K];
		var dijsktra = new Dijkstra<double>(digraph, source, add, zero, maxValue);
		
		if(!dijsktra.HasPathTo(target))
		{
			return;
		}
		
		var path = dijsktra.GetPathTo(target);
		ResizeableArray<DirectedEdge<double>> removedEdges = [];
		A[0] = path;
		var B = DataStructures.PriorityQueue(100, new DirectedPathComparer<double>(comparer));
		
		for (int k = 1; k < K; k++)
		{
			var previousPath = A[k - 1];
			Assert(previousPath != null);
			
			for (int i = 0; i < previousPath.Vertexes.Count() - 2; i++)
			{
				int spurNode = previousPath.Vertexes[i];
				var rootPath = Enumerable.Take(previousPath.Vertexes, i + 1);
				Assert(rootPath.Last() == spurNode);
				
				foreach (var p in A.Take(k))
				{
					Assert(p != null);


					if (k < A.Length - 1)
					{
						Assert(A[k + 1] == null);
					}

					if (rootPath.SequenceEqual(Enumerable.Take(p.Vertexes, i + 1)))
					{
						var edge = digraph.RemoveUniqueEdge(p.Vertexes[i], p.Vertexes[i + 1]);
						removedEdges.Add(edge);
					}
				}

				foreach (var rootPahNode in rootPath)
				{
					if(rootPahNode == spurNode)
					{
						continue;
					}
					
					var edgesToRemove = digraph
						.GetIncidentEdges(rootPahNode)
						.Concat(digraph.Edges.Where(edge => edge.Target == rootPahNode))
						.ToResizableArray();

					foreach (var edge in edgesToRemove)
					{
						digraph.RemoveEdge(edge);
						removedEdges.Add(edge);
					}
						
				}
				
				dijsktra = new Dijkstra<double>(digraph, spurNode, add, zero, maxValue);
				
				if(dijsktra.HasPathTo(target))
				{
					var spurPath = dijsktra.GetPathTo(target);
					Assert(rootPath.Last() != spurPath.Vertexes.Skip(1).First()); //If not we need to skip a vert
					var totalPath = rootPath.Concat(spurPath.Vertexes.Skip(1));
					
					foreach (var edge in removedEdges)
					{
						digraph.AddEdge(edge);
					} 
				
					removedEdges.Clear();
					
					if (!B.Any(path => path.Vertexes.SequenceEqual(totalPath)))
					{
						ResizeableArray<DirectedEdge<double>> edges = [];

						foreach (var pairList in ListExtensions.SlidingWindow2(totalPath))
						{
							var pair = pairList.ToList();
							var edge = digraph.GetUniqueEdge(pair[0], pair[1]);
							edges.Add(edge);
						}
					
						B.Push(new(edges, add));
					}
				}
				else
				{
					foreach (var edge in removedEdges)
					{
						digraph.AddEdge(edge);
					} 
				
					removedEdges.Clear();
				}
			}

			if (B.IsEmpty())
			{
				// This handles the case of there being no spur paths, or no spur paths left.
				// This could happen if the spur paths have already been exhausted (added to A), 
				// or there are no spur paths at all - such as when both the source and sink vertices 
				// lie along a "dead end".
				
				break;
			}

			A[k] = B.PopMin();
		}
	}

	public bool HasPath(int k)
	{
		return k >= 0 && k < A.Length && A[k] != null;
	}

	public DirectedPath<double> GetPath(int i)
	{
		if (!HasPath(i))
		{
			throw new InvalidOperationException("No path exists");
		}
		
		return A[i]!;
	}
}
