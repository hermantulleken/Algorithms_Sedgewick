namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using List;
using PriorityQueue;
using Set;
using static System.Diagnostics.Debug;

/// <summary>
/// A modified version of Dijkstra's algorithm that finds the shortest path from a source to a sink.
/// </summary>
/// <remarks>
/// This implementation can be faster than vanilla Dijkstra since we can stop early once we processed the sink.
/// </remarks>
// Note: only implemented for weights of type double.
[ExerciseReference(4, 4, 23)]
public class DijkstraBidirectional<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	private readonly TWeight? distance;
	private readonly IReadonlyRandomAccessList<DirectedEdge<TWeight>>? path;
	
	private readonly TWeight? sourceDistance;
	private readonly IReadonlyRandomAccessList<DirectedEdge<TWeight>>? sourcePath;
	private bool sourcePathExists;
	
	private readonly TWeight? sinkDistance;
	private readonly IReadonlyRandomAccessList<DirectedEdge<TWeight>>? sinkPath;
	private bool sinkPathExists;
	
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
	/// Initializes a new instance of the <see cref="DijkstraBidirectional{TWeight}"/> class.
	/// </summary>
	/// <param name="graph">The graph to find the shortest path in.</param>
	/// <param name="source">The source vertex to find the shortest path from.</param>
	/// <param name="sink">The sink vertex to find the shortest path to.</param>
	/// <exception cref="ArgumentException"><paramref name="graph"/> contains negative weights.</exception>
	public DijkstraBidirectional(
		IEdgeWeightedDigraph<TWeight> graph,
		int source, 
		int sink)
	{
		/*var distanceTo = new TWeight[graph.VertexCount];
		var edgeTo = new DirectedEdge<TWeight>?[graph.VertexCount];*/
		
		var sourceDistanceTo = new TWeight[graph.VertexCount];
		var sourceEdgeTo = new DirectedEdge<TWeight>?[graph.VertexCount];
		sourceDistanceTo.Fill(TWeight.MaxValue);
		sourceDistanceTo[source] = TWeight.Zero;
		sourceEdgeTo[source] = null;
		var sourceClosedList = DataStructures.Set(Comparer<int>.Default);
	
		var sinkDistanceTo = new TWeight[graph.VertexCount];
		var sinkEdgeTo = new DirectedEdge<TWeight>?[graph.VertexCount];
		sinkDistanceTo.Fill(TWeight.MaxValue);
		sinkDistanceTo[source] = TWeight.Zero;
		sinkEdgeTo[source] = null;
		var sinkClosedList = DataStructures.Set(Comparer<int>.Default);
		
		var sourceQueue = DataStructures.IndexedPriorityQueue(graph.VertexCount, Comparer<TWeight>.Default);
		sourceQueue.Insert(source, TWeight.Zero);
		
		var sinkQueue = DataStructures.IndexedPriorityQueue(graph.VertexCount, Comparer<TWeight>.Default);
		sinkQueue.Insert(source, TWeight.Zero);
		
		// TODO: when shoudl we stop? 
		while (true/*!queue.IsEmpty*/)
		{
			var (nextSourceNode, distanceToSource) = sourceQueue.PopMin();
			var (nextSinkNode, distanceToSink) = sinkQueue.PeekMin();

			// TODO: Where should this be added?
			sourceClosedList.Add(nextSourceNode);
			sinkClosedList.Add(nextSinkNode);
			
			if (PathExists && distanceToSource + distanceToSource > distance)
			{
				/*	We can break here because all paths that will be found will be longer than distanceToSource, 
					so the current smallest path is indeed the shortest.
				*/
				break; 
			}
			
			Expand(
				nextSourceNode, 
				distanceToSource,
				sinkEdgeTo, 
				sourceDistanceTo, 
				sourceQueue, 
				sinkClosedList,
				ref sourcePathExists, 
				ref sourceDistance,
				ref sourcePath,
				edge => edge.Target);
			
			Expand(
				nextSinkNode, 
				distanceToSink,
				sinkEdgeTo, 
				sinkDistanceTo, 
				sinkQueue, 
				sourceClosedList,
				ref sinkPathExists, 
				ref sinkDistance,
				ref sinkPath,
				edge => edge.Source);
		}

		void Expand(
			int nextNode, 
			TWeight distanceToNode, 
			DirectedEdge<TWeight>?[] edgeTo,
			TWeight[] distanceTo,
			IndexPriorityQueue<TWeight> queue,
			ISet<int> visitedOnOtherSide,
			ref bool nodeDistanceExists,
			ref TWeight nodeDistance,
			ref IReadonlyRandomAccessList<DirectedEdge<TWeight>> nodePath,
			Func<DirectedEdge<TWeight>, int> getExtreme)
		{
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
					distanceTo[edge.Target] = distanceToNode + edge.Weight;
					queue.Insert(edge.Target, distanceTo[edge.Target]);
				}
				else if (distanceToNode + edge.Weight < distanceTo[edge.Target])
				{
					// Found a shorter path.
					edgeTo[edge.Target] = edge;
					distanceTo[edge.Target] = distanceToNode + edge.Weight;

					// Not sure if this check is correct
					if (queue.Contains(edge.Target))
					{
						queue.UpdateValue(edge.Target, distanceTo[edge.Target]);
					}
				}

				if (visitedOnOtherSide.Contains(getExtreme(edge)))
				{
					nodeDistanceExists = true;
					nodeDistance = distanceTo[getExtreme(edge)];
					nodePath = GetPath(edgeTo, sink);
					
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
