﻿namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using List;
using static System.Diagnostics.Debug;

/// <summary>
/// A modified version of Dijkstra's algorithm that finds the shortest path from a source to a sink.
/// </summary>
/// <remarks>
/// This implementation can be faster than vanilla Dijkstra since we can stop early once we processed the sink.
/// </remarks>
// Note: only implemented for weights of type double.
[ExerciseReference(4, 4, 23)]
public class DijkstraSourceSink<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private readonly TWeight? distance;
	private readonly IReadonlyRandomAccessList<DirectedEdge<TWeight>>? path;
	
	/// <summary>
	/// Gets a value indicating whether a path exists from the source to the sink.
	/// </summary>
	[MemberNotNullWhen(true, nameof(path))]
	[MemberNotNullWhen(true, nameof(Path))]
	[MemberNotNullWhen(true, nameof(distance))]
	public bool PathExists { get; }

	/// <summary>
	/// Gets the path from the source to the sink.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="PathExists"/> is <see langword="false"/>.</exception>
	public IReadonlyRandomAccessList<DirectedEdge<TWeight>>? Path
		=> PathExists ? path : throw new InvalidOperationException("No path exists.");

	/// <summary>
	/// Gets the distance from the source to the sink.
	/// </summary>
	/// <exception cref="InvalidOperationException"><see cref="PathExists"/> is <see langword="false"/>.</exception>
	public TWeight Distance => PathExists ? distance : throw new InvalidOperationException("No path exists.");
	
	/// <summary>
	/// Initializes a new instance of the <see cref="DijkstraSourceSink{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest path in.</param>
	/// <param name="source">The source vertex to find the shortest path from.</param>
	/// <param name="sink">The sink vertex to find the shortest path to.</param>
	/// <exception cref="ArgumentException"><paramref name="graph"/> contains negative weights.</exception>
	public DijkstraSourceSink(
		IEdgeWeightedDigraph<TWeight> graph,
		int source, 
		int sink)
	{
		var distanceTo = new TWeight[graph.VertexCount];
		var edgeTo = new DirectedEdge<TWeight>?[graph.VertexCount];
		
		distanceTo.Fill(TWeight.MaxValue);
		distanceTo[source] = TWeight.Zero;
		edgeTo[source] = null;
		
		var queue = DataStructures.IndexedPriorityQueue(graph.VertexCount, Comparer<TWeight>.Default);
		queue.Insert(source, TWeight.Zero);
		
		while (!queue.IsEmpty)
		{
			var (nextNode, distanceToSource) = queue.PopMin();

			if (PathExists && distanceToSource > distance)
			{
				/*	We can break here because all paths that will be found will be longer than distanceToSource, 
					so the current smallest path is indeed the shortest.
				*/
				break; 
			}
			
			foreach (var edge in graph.GetIncidentEdges(nextNode))
			{
				Assert(edge.Target != source); // Because of the priority queue, this should never happen.
				
				if (edge.Weight < TWeight.Zero)
				{
					throw new ArgumentException("Negative weights are not allowed.", nameof(graph));
				}
				
				if (edgeTo[edge.Target] == null) 
				{
					// Not visited.
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = distanceToSource + edge.Weight;
					queue.Insert(edge.Target, distanceTo[edge.Target]);
				}
				else if (distanceToSource + edge.Weight < distanceTo[edge.Target])
				{
					// Found a shorter path.
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = distanceToSource + edge.Weight;

					// Not sure if this check is correct
					if (queue.Contains(edge.Target))
					{
						queue.UpdateValue(edge.Target, distanceTo[edge.Target]);
					}
				}

				if (edge.Target == sink)
				{
					PathExists = true;
					distance = distanceTo[edge.Target];
					path = GetPath(edgeTo, sink);
					
					/*	The first time we have a path two the sync it is not necessarily the shortest
						Example: A-------(20)------B
								 \--(1)--C--(1)--/
								 
						Therefore we cannot break here. 
					*/
				}
			}
		}
	}

	private IReadonlyRandomAccessList<DirectedEdge<TWeight>> GetPath(DirectedEdge<TWeight>?[] edgeTo, int sink)
	{
		Assert(PathExists);
		var stack = new Stack<DirectedEdge<TWeight>>();
		
		for (var edge = edgeTo[sink]; edge != null; edge = edgeTo[edge.Source])
		{
			stack.Push(edge);
		}

		return stack.ToResizableArray();
	}
}
