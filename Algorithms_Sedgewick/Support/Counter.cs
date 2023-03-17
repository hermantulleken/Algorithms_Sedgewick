namespace Support;

public class Counter<T>
{
	private readonly IDictionary<T, int> counts = new Dictionary<T, int>();

	public int this[T key] => counts[key];

	public IEnumerable<T> Keys => counts.Keys;
	public IEnumerable<KeyValuePair<T, int>> Counts => counts;
	public void Add(T key)
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

	public void Clear() => counts.Clear();

	public override string ToString() => counts.Pretty();
}
