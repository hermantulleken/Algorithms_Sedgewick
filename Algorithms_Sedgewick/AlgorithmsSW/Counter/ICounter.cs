namespace AlgorithmsSW.Counter;

/// <summary>
/// A data structure that keeps track of the number of times an item is added or removed.
/// </summary>
/// <remarks>This is also sometimes called a bag (not to be confused with <see cref="Bag"/>), or multiset
/// (although a multiset can also refer to an other data structure).
/// </remarks>
public interface ICounter<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the items that have been added (and not removed the same number of times).
	/// </summary>
	IEnumerable<T> Keys { get; }
	
	/// <summary>
	/// Gets the number of times an item has been added (minus the number of times it has been removed).
	/// </summary>
	/// <param name="item">The item to get the count of.</param>
	int this[T item] { get; }
	
	/// <summary>
	/// Increments the count of an item by 1.
	/// </summary>
	/// <param name="item">The item to add.</param>
	void Add(T item);
	
	/// <summary>
	/// Reduces the count of an item by 1.
	/// </summary>
	/// <param name="item">The item to remove.</param>
	void Remove(T item);

	/// <summary>
	/// Sets the count of all items to 0.
	/// </summary>
	void Clear();
}
