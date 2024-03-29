﻿namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using PriorityQueue;

/// <summary>
/// Algorithm to find the shortest path from a source vertex to all other vertices in a edge weighted digraph.
/// </summary>
/// <typeparam name="TWeight">The type of the edge weights.</typeparam>
[AlgorithmReference(4, 9)]
public class Dijkstra<TWeight> : IShortestPath<TWeight>
	where TWeight : 
		INumber<TWeight>,
		IMinMaxValue<TWeight>
{
	private readonly IReadOnlyEdgeWeightedDigraph<TWeight> graph;
	private readonly DirectedEdge<TWeight>?[] edgeTo;
	private readonly TWeight[] distTo;
	private readonly IndexPriorityQueue<TWeight> priorityQueue;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Dijkstra{T}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest paths in.</param>
	/// <param name="source">The source vertex to find the shortest paths from.</param>
	public Dijkstra(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph, 
		int source)
	{
		this.graph = graph;
		edgeTo = new DirectedEdge<TWeight>[graph.VertexCount];
		distTo = new TWeight[graph.VertexCount];
		priorityQueue = new(graph.VertexCount, Comparer<TWeight>.Default);
		
		for (int i = 0; i < graph.VertexCount; i++)
		{
			distTo[i] = TWeight.MaxValue;
		}
		
		distTo[source] = TWeight.Zero;
		priorityQueue.Insert(source, TWeight.Zero);
		
		while (!priorityQueue.IsEmpty)
		{
			Relax(graph, priorityQueue.PopMin().index);
		}
	}

	/// <inheritdoc />
	public TWeight GetDistanceTo(int vertex) => distTo[vertex];
	
	/// <inheritdoc />
	// TODO: this is not a robust test!
	public bool HasPathTo(int vertex) => distTo[vertex] != TWeight.MaxValue;
	
	/// <inheritdoc />
	public IEnumerable<DirectedEdge<TWeight>> GetEdgesOfPathTo(int target)
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

	public DirectedPath<TWeight> GetPathTo(int target)
	{
		var path = GetEdgesOfPathTo(target).ToResizableArray();
		
		return new(path);
	}
	
	private void Relax(IReadOnlyEdgeWeightedDigraph<TWeight> graph, int vertex)
	{
		foreach (var edge in graph.GetIncidentEdges(vertex))
		{
			int target = edge.Target;

			if (distTo[target] <= distTo[vertex] + edge.Weight)
			{
				continue;
			}

			distTo[target] = distTo[vertex] + edge.Weight;
			edgeTo[target] = edge;
				
			if (priorityQueue.Contains(target))
			{
				priorityQueue.UpdateValue(target, distTo[target]);
			}
			else
			{
				priorityQueue.Insert(target, distTo[target]);
			}
		}
	}
}
