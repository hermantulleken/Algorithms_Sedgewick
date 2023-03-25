namespace Algorithms_Sedgewick.Bag;

/// <summary>
/// A collection that supports adding elements, and iterating over them. 
/// </summary>
/// <typeparam name="T">The type of the items in the bag.</typeparam>
// p. 121
public interface IBag<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the number of elements in the bag.
	/// </summary>
	public int Count { get; }

	/// <summary>
	/// Gets a value indicating whether the bag is empty.
	/// </summary>
	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Adds an item to the bag. 
	/// </summary>
	/// <param name="item">The item to add.</param>
	public void Add(T item);
}
