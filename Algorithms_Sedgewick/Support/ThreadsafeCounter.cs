namespace Support;

using System.Collections.Concurrent;

/// <summary>
/// A thread save container that can be used to count things. 
/// </summary>
/// <typeparam name="T">The things that can be counted.</typeparam>
/*
	Note: This counter has been specifically implemented to support instrumentation
	while running algorithms in parallel. It may affect performance.	
*/ 
public class ThreadsafeCounter<T> 
	where T : notnull 
{
	private readonly ConcurrentDictionary<T, int> counts;

	public ThreadsafeCounter(IEqualityComparer<T> comparer)
	{
		counts = new ConcurrentDictionary<T, int>(comparer);
	}

	public IEnumerable<T> Keys => counts.Keys;

	public int this[T item] => counts[item];

	public void Add(T item)
	{
		if (!counts.ContainsKey(item))
		{
			counts[item] = 1;
		}
		else
		{
			counts[item]++;
		}
	}

	public int GetCount(T item) => counts.ContainsKey(item) ? counts[item] : 0;

	public void Clear() => counts.Clear();

	/// <inheritdoc/>
	public override string ToString() => counts.Pretty();
}
