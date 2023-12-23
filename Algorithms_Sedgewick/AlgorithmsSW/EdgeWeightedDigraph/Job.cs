namespace AlgorithmsSW.EdgeWeightedDigraph;

/// <summary>
/// Represents a job with a duration and dependencies on other jobs.
/// </summary>
/// <typeparam name="TDuration">The type of duration.</typeparam>
/*
	Note: Job does not feel abstract enough, but I could not think of a better name. 
*/
public class Job<TDuration> // TDuration is TWeight in graph algorithms.
{
	public TDuration Duration { get; set; }
	
	public IEnumerable<int> Dependencies { get; private set; }
	
	public Job(TDuration duration, IEnumerable<int> dependencies)
	{
		Duration = duration;
		Dependencies = dependencies.ToList(); // Copy!
	}
}