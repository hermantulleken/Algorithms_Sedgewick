namespace AlgorithmsSW;

public static class Comparable
{
	public static Comparable<T> ToComparable<T>(this T item, IComparer<T> comparer) 
		=> new(item, comparer);
}

#pragma warning disable SA1402
public class Comparable<T> : IComparable<Comparable<T>>
#pragma warning restore SA1402
{
	private readonly IComparer<T> comparer;

	public T Item { get; }

	public Comparable(T item, IComparer<T> comparer)
	{
		Item = item;
		this.comparer = comparer;
	}

	public int CompareTo(Comparable<T>? other)
		=> other == null 
			? 1 
			: comparer.Compare(Item, other.Item);
}
