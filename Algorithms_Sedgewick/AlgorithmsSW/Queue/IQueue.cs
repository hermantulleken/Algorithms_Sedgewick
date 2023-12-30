namespace AlgorithmsSW.Queue;


/// <summary>
/// Represents a generic queue of items.
/// </summary>
/// <typeparam name="T">The type of items contained in the queue.</typeparam>
[PageReference(121)]
public interface IQueue<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the number of items contained in the <see cref="IQueue{T}"/>.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Gets a value indicating whether the <see cref="IQueue{T}"/> is empty.
	/// </summary>
	bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets a value indicating whether the <see cref="IQueue{T}"/> contains only one item.
	/// </summary>
	bool IsSingleton => Count == 1;

	/// <summary>
	/// Gets the item at the beginning of the <see cref="IQueue{T}"/> without removing it.
	/// </summary>
	T Peek { get; }

	/// <summary>
	/// Removes all items from the <see cref="IQueue{T}"/>.
	/// </summary>
	void Clear();

	/// <summary>
	/// Removes and returns the item at the beginning of the <see cref="IQueue{T}"/>.
	/// </summary>
	/// <returns>The item at the beginning of the <see cref="IQueue{T}"/>.</returns>
	T Dequeue();

	/// <summary>
	/// Adds an item to the end of the <see cref="IQueue{T}"/>.
	/// </summary>
	/// <param name="item">The item to add to the <see cref="IQueue{T}"/>.</param>
	void Enqueue(T item);
}
