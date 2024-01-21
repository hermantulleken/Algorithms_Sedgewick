namespace AlgorithmsSW.Set;

using System.Collections;
using List;

/// <summary>
/// Represents a set of unique items implemented with an array.
/// </summary>
/// <typeparam name="T">The type of the items in the set.</typeparam>
public class SetWithArray<T> : ISet<T>
{
	private readonly ResizeableArray<T> items;

	/// <inheritdoc />
	public IComparer<T> Comparer { get; }

	private readonly IEqualityComparer<T> equalityComparer;

	/// <summary>
	/// Initializes a new instance of the <see cref="SetWithArray{T}"/> class.
	/// </summary>
	/// <param name="initialCapacity">The initial capacity of the set.</param>
	/// <param name="comparer">The comparer used to compare items.</param>
	public SetWithArray(int initialCapacity, IComparer<T> comparer)
	{
		items = new ResizeableArray<T>(initialCapacity);
		Comparer = comparer;
		equalityComparer = comparer.ToEqualityComparer();
	}

	/// <inheritdoc />
	public void Add(T item)
	{
		if (!items.Contains(equalityComparer, item))
		{
			items.Add(item);
		}
	}

	/// <inheritdoc />
	public bool Remove(T item) => items.Remove(item, equalityComparer);

	/// <inheritdoc />
	public int Count => items.Count;

	/// <inheritdoc />
	public bool Contains(T item) => items.Contains(equalityComparer, item);

	/// <inheritdoc />
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
