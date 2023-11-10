namespace Algorithms_Sedgewick.Buffer;

/// <summary>
/// Represents a generic buffer interface.
/// </summary>
/// <typeparam name="T">The type of elements in the buffer.</typeparam>
public interface IBuffer<T> : IEnumerable<T>
{
	/// <summary>
	/// Gets the maximum number of elements the buffer can hold.
	/// </summary>
	public int Capacity { get; }

	/// <summary>
	/// Gets the current number of elements in the buffer.
	/// </summary>
	public int Count { get; }

	/// <summary>
	/// Gets the first element in the buffer.
	/// </summary>
	public T First { get; }

	/// <summary>
	/// Gets a value indicating whether the buffer is full.
	/// </summary>
	public bool IsFull => Capacity == Count;

	/// <summary>
	/// Gets the last element in the buffer.
	/// </summary>
	public T Last { get; }

	/// <summary>
	/// Clears all items from the buffer.
	/// </summary>
	public void Clear();

	/// <summary>
	/// Inserts an item into the buffer.
	/// </summary>
	/// <param name="item">The item to insert into the buffer.</param>
	public void Insert(T item);
}
