namespace AlgorithmsSW.List;

/// <summary>
/// Represents a read-only random access list that provides indexed access to its elements.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public interface IReadonlyRandomAccessList<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the number of elements in the list.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Gets a value indicating whether the list is empty.
	/// </summary>
	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets or sets the element at the specified index in the list.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get or set.</param>
	/// <returns>The element at the specified index.</returns>
	T this[int index] { get; }
}

/// <summary>
/// Represents a read-only random access list that provides indexed access to its elements.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public interface IRandomAccessList<T> : IReadonlyRandomAccessList<T>
{
	/// <summary>
	/// sets the element at the specified index in the list.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get or set.</param>
	/// <returns>The element at the specified index.</returns>
	new T this[int index] { get; set; }
}
