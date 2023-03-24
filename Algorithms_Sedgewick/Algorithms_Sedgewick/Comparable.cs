namespace Algorithms_Sedgewick;

public static class Comparable
{
	public static Comparable<T> ToComparable<T>(this T item, IComparer<T> comparer) 
		=> new(item, comparer);
}

public class Comparable<T> : IComparable<Comparable<T>>
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
