using AlgorithmsSW.Graph;
using AlgorithmsSW.List;
using AlgorithmsSW.PriorityQueue;
using static System.Diagnostics.Debug;

namespace AlgorithmsSW.EdgeWeightedGraph;

using System.Numerics;

public class Mst
{
	/// <summary>
	/// Finds a new spanning tree if we delete the given edge from a graph, given an existing MST.
	/// </summary>
	/// <param name="graph">The graph to delete the edge from.</param>
	/// <param name="mst">The existing MST.</param>
	/// <param name="edge">The edge to delete.</param>
	/// <typeparam name="T">The type of the weight.</typeparam>
	/// <returns>A new MST.</returns>
	[ExerciseReference(4, 3, 14)]
	public EdgeWeightedGraphWithAdjacencyLists<T> DeleteEdge<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, IMst<T> mst, Edge<T> edge)
		where T : INumber<T>
	{
		var newMst = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount, mst.Edges);
		graph.RemoveEdge(edge);

		var (component0, component1) = GetComponents(newMst);
		var minEdge = FindMinimumEdgeThatConnectTwoComponents(newMst, component0, component1);
		newMst.AddEdge(minEdge);

		return newMst;
	}
	
	/// <summary>
	/// Finds a new spanning tree if we add an edge the given edge from a graph, given an existing MST.
	/// </summary>
	/// <param name="graph">The graph to delete the edge from.</param>
	/// <param name="mst">The existing MST.</param>
	/// <param name="edge">The edge to delete.</param>
	/// <typeparam name="T">The type of the weight.</typeparam>
	/// <returns>A new MST.</returns>
	[ExerciseReference(4, 3, 15)]
	public EdgeWeightedGraphWithAdjacencyLists<T> AddEdge<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, IMst<T> mst, Edge<T> edge)
		where T : INumber<T>
	{
		var mstGraph = new GraphWithAdjacentsLists(graph.VertexCount);
		foreach (var e in mst.Edges)
		{
			mstGraph.AddEdge(e.Vertex0, e.Vertex1);
		}
		
		var pathSearch = new DepthFirstPathsSearch(mstGraph, edge.Vertex0);
		var path = pathSearch.GetPathTo(edge.Vertex1);
		graph.AddEdge(edge);

		var maxEdge = FindMaxEdgeInCycle(graph, path);

		if (maxEdge == edge)
		{
			var newMst = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount, mst.Edges);
			return newMst;
		}
		else
		{
			var newEdges = mst.Edges.Where(e => e != maxEdge).Append(edge);
			var newMst = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount, newEdges);
			
			return newMst;
		}
	}
	
	/// <summary>
	/// Finds a new spanning tree if we add an edge the given edge from a graph, given an existing MST.
	/// </summary>
	/// <param name="graph">The graph to delete the edge from.</param>
	/// <param name="mst">The existing MST.</param>
	/// <param name="edge">The edge to delete.</param>
	/// <typeparam name="T">The type of the weight.</typeparam>
	/// <returns>A new MST.</returns>
	[ExerciseReference(4, 3, 16)]
	public T FindMaxWeightThat<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, IMst<T> mst, (int vertex0, int vertex1) edge)
		where T : INumber<T>
	{
		var mstGraph = new GraphWithAdjacentsLists(graph.VertexCount);
		foreach (var e in mst.Edges)
		{
			mstGraph.AddEdge(e.Vertex0, e.Vertex1);
		}
		
		var pathSearch = new DepthFirstPathsSearch(mstGraph, edge.vertex0);
		var path = pathSearch.GetPathTo(edge.vertex1);

		var maxEdge = FindMaxEdgeInCycle(graph, path);
		return maxEdge.Weight;
	}

	private (Set.ISet<int> component0, Set.ISet<int> component1) GetComponents<T>(EdgeWeightedGraphWithAdjacencyLists<T> newMst)
	{
		var components = new Components<T>(newMst);
		var component0 = DataStructures.Set(Comparer<int>.Default);
		var component1 = DataStructures.Set(Comparer<int>.Default);

		foreach (int vertex in newMst.Vertexes)
		{
			if (components.GetComponentId(vertex) == 0)
			{
				component0.Add(vertex);
			}
			else
			{
				Assert(components.GetComponentId(vertex) == 1);
				component1.Add(vertex);
			}
		}

		return (component0, component1);
	}
	
	private Edge<T> FindMinimumEdgeThatConnectTwoComponents<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, Set.ISet<int> component0, Set.ISet<int> component1)
		where T : IComparisonOperators<T, T, bool>
	{
		Edge<T>? minEdge = null;

		foreach (var e in graph.WeightedEdges)
		{
			if ((!component0.Contains(e.Vertex0) || !component1.Contains(e.Vertex1)) &&
				(!component1.Contains(e.Vertex0) || !component0.Contains(e.Vertex1)))
			{
				continue;
			}

			if (minEdge != null && e.Weight >= minEdge.Weight)
			{
				continue;
			}

			minEdge = e;
		}

		Assert(minEdge != null);

		return minEdge;
	}

	private Edge<T> FindMaxEdgeInCycle<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, IEnumerable<int> cycle) 
		=> cycle
			.CircularSlidingWindow2()
			.Select(edge => edge.ToList())
			.Select(edge => graph.GetUniqueEdge(edge[0], edge[1]))
			.MaxBy(edge => edge.Weight)!;

	[ExerciseReference(4, 3, 22)]
	public static IEnumerable<IMst<T>> MstForest<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph)
		where T : INumber<T>
	{
		var componentsAlgo = new Components<T>(graph);
		var components = new EdgeWeightedGraphWithAdjacencyLists<T>?[componentsAlgo.ComponentCount];		
		
		foreach (var edge in graph.WeightedEdges)
		{
			int componentIndex = componentsAlgo.GetComponentId(edge.Vertex0);

			components[componentIndex] ??= new(graph.VertexCount);
			components[componentIndex]!.AddEdge(edge);
		}
		
		return components.Select( c => new KruskalMst<T>(c));
	}
	
	// May not work art all.
	public static EdgeWeightedGraphWithAdjacencyLists<T> MysteryMst<T>(IEdgeWeightedGraph<T> graph)
	{
		var mstEdges = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount);
		var unionFind = new UnionFind(graph.VertexCount);
		var sortedEdges = graph.WeightedEdges.OrderBy(edge => edge.Weight);

		foreach (var edge in sortedEdges)
		{
			int componentIndex0 = unionFind.GetComponentIndex(edge.Vertex0);
			int componentIndex1 = unionFind.GetComponentIndex(edge.Vertex1);

			if (componentIndex0 == componentIndex1)
			{
				continue;
			}
			
			mstEdges.AddEdge(edge);
			unionFind.Union(componentIndex0, componentIndex1);
		}

		return mstEdges;
	}
	
	public static bool IsBridge<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, int vertex0, int vertex1)
	{
		graph.ThrowIfNull();

		if (!graph.ContainsEdge(vertex0, vertex1))
		{
			return false;
		}

		var edge = graph.GetUniqueEdge(vertex0, vertex1);
		graph.RemoveEdge(edge);
		var connectivity = new Components<T>(graph);
		bool connected = !connectivity.AreConnected(vertex0, vertex1);
		graph.AddEdge(edge);
		
		return !connected;
	}
	
	public EdgeWeightedGraphWithAdjacencyLists<T> Mst_ReverseDelete<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph)
		where T : IComparisonOperators<T, T, bool>
	{
		var mst = new EdgeWeightedGraphWithAdjacencyLists<T>(graph.VertexCount);
		var priorityQueue = DataStructures.PriorityQueue(graph.EdgeCount, new EdgeComparer<T>());
		
		while (priorityQueue.IsEmpty())
		{
			var edge = priorityQueue.PopMin();
			bool canDelete = !IsBridge(mst, edge.Vertex0, edge.Vertex1);
			
			if (canDelete)
			{
				mst.AddEdge(edge);
			}
		}
		return mst;
	}

	[ExerciseReference(4, 3, 33)]
	private bool IsMst<T>(EdgeWeightedGraphWithAdjacencyLists<T> graph, IMst<T> mst)
		where T : INumber<T>
	{
		var comparer = Comparer<int>.Default;
		var edgeComparer = new EdgeComparer<T>();

		var consistentPartitions =
			from subset in Algorithms.PowerSet(graph.Vertexes)
			where subset.Count != 0 && subset.Count != graph.VertexCount
			let partition0 = subset.ToSet(comparer)
			let partition1 = graph.Vertexes.Except(subset).ToSet(comparer)
			where IsMstConsistentWithCut(graph, mst, partition0, partition1, edgeComparer)
			select partition0;
		
		return !consistentPartitions.Any();
	}

	private bool IsMstConsistentWithCut<T>(
		EdgeWeightedGraphWithAdjacencyLists<T> graph, 
		IMst<T> mst,  
		Set.ISet<int> partition0,
		Set.ISet<int> partition1,
		EdgeComparer<T> comparer) 
		where T : INumber<T>
	{
		var joiningEdges = DataStructures.Set(comparer);
		
		foreach (var edge in graph.WeightedEdges)
		{
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;
			
			if (partition0.Contains(vertex0) && partition1.Contains(vertex1))
			{
				joiningEdges.Add(edge);
			}
			
			if (partition1.Contains(vertex0) && partition0.Contains(vertex1))
			{
				joiningEdges.Add(edge);
			}
		}
		
		var edgesInMst = DataStructures.Set(comparer);

		foreach (var edge in joiningEdges)
		{
			if (mst.Edges.Contains(edge))
			{
				edgesInMst.Add(edge);
			}
		}
		
		if (edgesInMst.Count() != 1)
		{
			return false;
		}
		
		var minEdge = joiningEdges.MinBy(edge => edge.Weight);

		return edgesInMst.First() == minEdge;
	}
}
