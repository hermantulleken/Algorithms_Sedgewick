namespace Support;

/// <summary>
/// A thread save container that can be used to count things. 
/// </summary>
/// <typeparam name="T">The things that can be counted.</typeparam>
public class Counter<T>
{
	private readonly IDictionary<T, int> counts = new Dictionary<T, int>();

	public IEnumerable<KeyValuePair<T, int>> Counts => counts;

	public int this[T key] => counts[key];

	public IEnumerable<T> Keys => counts.Keys;

	public void Add(T key)
	{
		lock (counts)
		{
			if (counts.ContainsKey(key))
			{
				counts[key]++;
			}
			else
			{
				counts[key] = 1;
			}
		}
	}

	public void Clear()
	{
		lock (counts)
		{
			counts.Clear();
		}
	}

	public void Remove(T key)
	{
		lock (counts)
		{
			if (counts.ContainsKey(key))
			{
				if (counts[key] == 1)
				{
					counts.Remove(key);
				}
				else
				{
					counts[key]--;
				}
			}
			else
			{
				counts[key] = -1;
			}
		}
	}

	/// <inheritdoc/>
	public override string ToString() => counts.Pretty();
}
