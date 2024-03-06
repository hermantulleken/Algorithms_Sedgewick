namespace AlgorithmsSW.EdgeWeightedDigraph;

using System.Numerics;
using List;
using static System.Diagnostics.Debug;

public class CriticalPathMethod<TWeight>
	where TWeight : INumber<TWeight>, IMinMaxValue<TWeight>
{
	public IRandomAccessList<IRandomAccessList<DirectedEdge<TWeight>>> CriticalPaths { get; }
	
	public TWeight CriticalDistance { get; }

	public CriticalPathMethod(
		IReadonlyRandomAccessList<Job<TWeight>> jobs, 
		TWeight tolerance)
	{
		/*
			Note: In the textbook they use a different indexing scheme. I don't think it matters much. 
			However, I DO think it is necessary to have helper methods and not do the calculations inline to avoid 
			mistakes.  
		*/
		var graph = DataStructures.EdgeWeightedDigraph<TWeight>(jobs.Count * 2 + 2);
		int source = 0;
		int sink = jobs.Count * 2 + 1;
		
		for (int jobIndex = 0; jobIndex < jobs.Count; jobIndex++)
		{
			graph.AddEdge(JobStart(jobIndex), JobEnd(jobIndex), jobs[jobIndex].Duration);
			
			foreach (int dependency in jobs[jobIndex].Dependencies)
			{
				graph.AddEdge(JobEnd(dependency), JobStart(jobIndex), TWeight.Zero);
			}
			
			graph.AddEdge(source, JobStart(jobIndex), TWeight.Zero);
			graph.AddEdge(JobEnd(jobIndex), sink, TWeight.Zero);
		}
		
		AcyclicLongestPaths<TWeight> longestPaths = new(graph, source);
		
		CriticalDistance = longestPaths.GetDistanceTo(sink);

		CriticalPaths = BackTrack(graph, source, sink, tolerance);
	}

	private ResizeableArray<IRandomAccessList<DirectedEdge<TWeight>>> BackTrack(
		IReadOnlyEdgeWeightedDigraph<TWeight> graph,
		int source,
		int sink,
		TWeight tolerance)
	{
		var currentPath = DataStructures.Stack<DirectedEdge<TWeight>>();
		ResizeableArray<IRandomAccessList<DirectedEdge<TWeight>>> paths = [];
		
		Backtrack(source, TWeight.Zero);

		return paths;
		
		void Backtrack(
			int currentVertex,
			TWeight currentDistance)
		{
			foreach (var nextEdge in graph.GetIncidentEdges(currentVertex))
			{
				TWeight newDistance = currentDistance + nextEdge.Weight;
				Assert(newDistance <= CriticalDistance);
				currentPath.Push(nextEdge);
			
				if (nextEdge.Target == sink && MathX.ApproximatelyEqual(newDistance, CriticalDistance, tolerance))
				{
					paths.Add(currentPath.ToResizableArray());
				}
				else
				{
					Assert(newDistance < CriticalDistance);
					Backtrack(nextEdge.Target, newDistance);
				}
				
				currentPath.Pop();
			}	
		}
	}
	
	private static int JobStart(int jobIndex) => jobIndex * 2 + 1;
	
	private static int JobEnd(int jobIndex) => jobIndex * 2 + 2;
}
