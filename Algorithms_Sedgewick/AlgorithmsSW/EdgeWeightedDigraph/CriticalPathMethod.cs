namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;
using static System.Diagnostics.Debug;

public class CriticalPathMethod<TWeight>
{
	public IRandomAccessList<IRandomAccessList<DirectedEdge<TWeight>>> CriticalPaths { get; }
	
	public TWeight CriticalDistance { get; }

	public CriticalPathMethod(
		IReadonlyRandomAccessList<Job<TWeight>> jobs, 
		IComparer<TWeight> comparer, 
		Func<TWeight, TWeight, TWeight> add, 
		TWeight zero, 
		TWeight tolerance,
		TWeight minValue)
	{
		/*
			Note: In the textbook they use a different indexing scheme. I don't think it matters much. 
			However, I DO think it is necessary to have helper methods and not do the calculations inline to avoid 
			mistakes.  
		*/
		var graph = DataStructures.EdgeWeightedDigraph(jobs.Count * 2 + 2, comparer);
		int source = 0;
		int sink = jobs.Count * 2 + 1;
		
		for (int jobIndex = 0; jobIndex < jobs.Count; jobIndex++)
		{
			graph.AddEdge(JobStart(jobIndex), JobEnd(jobIndex), jobs[jobIndex].Duration);
			
			foreach (int dependency in jobs[jobIndex].Dependencies)
			{
				graph.AddEdge(JobEnd(dependency), JobStart(jobIndex), zero);
			}
			
			graph.AddEdge(source, JobStart(jobIndex), zero);
			graph.AddEdge(JobEnd(jobIndex), sink, zero);
		}
		
		AcyclicLongestPaths<TWeight> longestPaths = new(graph, source, add, zero, minValue);
		
		CriticalDistance = longestPaths.GetDistanceTo(sink);

		CriticalPaths = BackTrack(graph, source, sink, zero, tolerance, add);
	}

	private ResizeableArray<IRandomAccessList<DirectedEdge<TWeight>>> BackTrack(IEdgeWeightedDigraph<TWeight> graph,
		int source,
		int sink,
		TWeight zero,
		TWeight tolerance, 
		Func<TWeight, TWeight, TWeight> add)
	{
		var currentPath = DataStructures.Stack<DirectedEdge<TWeight>>();
		ResizeableArray<IRandomAccessList<DirectedEdge<TWeight>>> paths = [];
		
		Backtrack(
			source, 
			zero);

		return paths;
		
		void Backtrack(
			int currentVertex,
			TWeight currentDistance)
		{
			foreach (var nextEdge in graph.GetIncidentEdges(currentVertex))
			{
				var newDistance = add(currentDistance, nextEdge.Weight);
				Assert(graph.Comparer.Compare(newDistance, CriticalDistance) <= 0);
				currentPath.Push(nextEdge);
			
				if (nextEdge.Target == sink && graph.Comparer.ApproximatelyEqual(newDistance, CriticalDistance, tolerance, add))
				{
					paths.Add(currentPath.ToResizableArray());
				}
				else
				{
					Assert(graph.Comparer.Compare(newDistance, CriticalDistance) < 0);
					Backtrack(nextEdge.Target, newDistance);
				}
				
				currentPath.Pop();
			}	
		}
	}
	
	private static int JobStart(int jobIndex) => jobIndex * 2 + 1;
	
	private static int JobEnd(int jobIndex) => jobIndex * 2 + 2;
}
