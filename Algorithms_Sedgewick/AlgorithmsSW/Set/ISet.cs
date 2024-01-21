namespace AlgorithmsSW.Set;

/// <summary>
/// Represents a set of unique items.
/// </summary>
/// <typeparam name="T">The type of the items in the set.</typeparam>
public interface ISet<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the comparer used to compare items.
	/// </summary>
	IComparer<T> Comparer { get; }
	
	/// <summary>
	/// Adds an item to the set.
	/// </summary>
	/// <param name="item">The item to add.</param>
	public void Add(T item);
	
	/// <summary>
	/// Checks if the set contains an item.
	/// </summary>
	/// <param name="item">The item to check for.</param>
	/// <returns><see langword="true"/> if the set contains the item, <see langword="false"/> otherwise.</returns>
	public bool Contains(T item);
	
	/// <summary>
	/// Removes an item from the set.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	/// <returns><see langword="true"/> if the item was removed, <see langword="false"/> otherwise.</returns>
	public bool Remove(T item);

	/// <summary>
	/// Gets the number of items in the set.
	/// </summary>
	int Count { get; }
}
