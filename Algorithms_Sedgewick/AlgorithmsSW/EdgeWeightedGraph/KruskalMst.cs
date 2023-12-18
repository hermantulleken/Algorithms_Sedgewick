﻿using AlgorithmsSW.PriorityQueue;

namespace AlgorithmsSW.EdgeWeightedGraph;

public class KruskalMst<T> : IMst<T>
{
	private readonly Set.ISet<Edge<T>> mst;
	
	public IEnumerable<Edge<T>> Edges => mst;

	public T Weight { get; }

	public KruskalMst(IEdgeWeightedGraph<T> graph)
	{
		IComparer<Edge<T>> comparer = new EdgeComparer<T>(graph.Comparer);
		mst = DataStructures.Set<Edge<T>>(comparer);
		
		IPriorityQueue<Edge<T>> priorityQueue = new FixedCapacityMinBinaryHeap<Edge<T>>(graph.EdgeCount, new EdgeComparer<T>(graph.Comparer));
		
		foreach (var edge in graph.Edges)
		{
			priorityQueue.Push(edge);
		}
		
		var unionFind = new UnionFind(graph.VertexCount);
		
		AddEdges(graph, priorityQueue, unionFind);
	}

	private void AddEdges(IEdgeWeightedGraph<T> graph, IPriorityQueue<Edge<T>> priorityQueue, UnionFind unionFind)
	{
		while (!priorityQueue.IsEmpty() && mst.Count < graph.VertexCount - 1)
		{
			var edge = priorityQueue.PopMin();
			int vertex0 = edge.Vertex0;
			int vertex1 = edge.Vertex1;
			
			if (unionFind.IsConnected(vertex0, vertex1))
			{
				continue;
			}
			
			unionFind.Union(vertex0, vertex1);
			mst.Add(edge);
		}
	}

	// 4.3.32
	public KruskalMst(EdgeWeightedGraphWithAdjacencyLists<T> graph, IEnumerable<Edge<T>> specifiedEdges)
	{
		IComparer<Edge<T>> comparer = new EdgeComparer<T>(graph.Comparer);
		mst = DataStructures.Set(comparer);
		var unionFind = new UnionFind(graph.VertexCount);
		
		foreach (var edge in specifiedEdges)
		{
			mst.Add(edge);
			unionFind.Union(edge.Vertex0, edge.Vertex1);
		}
		
		IPriorityQueue<Edge<T>> priorityQueue 
			= new FixedCapacityMinBinaryHeap<Edge<T>>(graph.EdgeCount, new EdgeComparer<T>(graph.Comparer));
		
		foreach (var edge in graph.Edges)
		{
			if (!mst.Contains(edge))
			{
				priorityQueue.Push(edge);
			}
		}
		
		AddEdges(graph, priorityQueue, unionFind);
	}
}
