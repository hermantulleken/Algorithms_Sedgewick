namespace AlgorithmsSW;

/// <summary>
/// Provides extension methods related to <see cref="Comparable{T}"/>.
/// </summary>
public static class Comparable
{
	public static Comparable<T> ToComparable<T>(this T item, IComparer<T> comparer) 
		=> new(item, comparer);
}

/// <summary>
/// Wraps an item in a class so it can implement <see cref="IComparable"/>, implemented with a
/// <see cref="IComparer{T}"/>.
/// </summary>
/// <typeparam name="T">The type of item to wrap.</typeparam>
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
			? 1 // this is not null
			: comparer.Compare(Item, other.Item);
}
