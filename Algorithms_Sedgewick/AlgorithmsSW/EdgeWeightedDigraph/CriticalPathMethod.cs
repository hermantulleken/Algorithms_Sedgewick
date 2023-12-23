﻿namespace AlgorithmsSW.EdgeWeightedDigraph;

using List;

public class Job<TWeight>
{
	public TWeight Duration { get; set; }
	
	public IEnumerable<int> Dependencies { get; private set; }
	
	public Job(TWeight duration, IEnumerable<int> dependencies)
	{
		Duration = duration;
		Dependencies = dependencies.ToList(); // Copy!
	}
}

public class Schedule<TWeight>
{
	public IRandomAccessList<TWeight> BestPath { get; }

	public Schedule(IReadonlyRandomAccessList<Job<TWeight>> jobs, IComparer<TWeight> comparer, Func<TWeight, TWeight, TWeight> add, TWeight zero, TWeight minValue)
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
			
			foreach (var dependency in jobs[jobIndex].Dependencies)
			{
				graph.AddEdge(JobEnd(dependency), JobStart(jobIndex), zero);
			}
			
			graph.AddEdge(source, JobStart(jobIndex), zero);
			graph.AddEdge(JobEnd(jobIndex), sink, zero);
		}
		
		AcyclicLongestPaths<TWeight> longestPaths = new(graph, source, add, zero, minValue);

		BestPath = Enumerable
			.Range(0, jobs.Count)
			.Select(jobIndex => longestPaths.DistanceTo(JobStart(jobIndex)))
			.ToResizableArray(jobs.Count);
	}
	
	private static int JobStart(int jobIndex) => jobIndex * 2 + 1;
	
	private static int JobEnd(int jobIndex) => jobIndex * 2 + 2;
}
