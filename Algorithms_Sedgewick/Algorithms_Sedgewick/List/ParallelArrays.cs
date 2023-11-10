namespace Algorithms_Sedgewick.List;

using Support;

/// <summary>
/// Represents a list of key-value pairs using two parallel arrays.
/// </summary>
/// <typeparam name="TKey">The type of keys in the list.</typeparam>
/// <typeparam name="TValue">The type of values in the list.</typeparam>
public class ParallelArrays<TKey, TValue>
{
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;

	/// <summary>
	/// Gets the capacity of this instance.
	/// </summary>
	public int Capacity => keys.Capacity;

	/// <summary>
	/// Gets the number of key-value pairs this instance contains.
	/// </summary>
	public int Count => keys.Count;
	
	/// <summary>
	/// Gets a value indicating whether this instance is empty.
	/// </summary>
	public bool IsEmpty => Count == 0;

	/// <summary>
	/// Gets a value indicating whether this instance is full.
	/// </summary>
	public bool IsFull => Count == Capacity;

	/// <summary>
	/// Gets the keys of this instance.
	/// </summary>
	public IReadonlyRandomAccessList<TKey> Keys => keys;

	/// <summary>
	/// Gets the values of this instance.
	/// </summary>
	public IReadonlyRandomAccessList<TValue> Values => values;

	/// <summary>
	/// Initializes a new instance of the <see cref="ParallelArrays{TKey,TValue}"/> class.
	/// </summary>
	/// <param name="initialCapacity">The initial number of key-value pairs this instance can support.</param>
	public ParallelArrays(int initialCapacity)
	{
		keys = new ResizeableArray<TKey>(initialCapacity);
		values = new ResizeableArray<TValue>(initialCapacity);
	}

	// Copy constructor
	private ParallelArrays(ParallelArrays<TKey, TValue> arraysToCopy)
	{
		keys = (ResizeableArray<TKey>)arraysToCopy.keys.Copy();
		values = (ResizeableArray<TValue>)arraysToCopy.values.Copy();
	}

	/// <summary>
	/// Adds a key-value pair to this instance.
	/// </summary>
	/// <param name="key">The key to add.</param>
	/// <param name="value">The value associated with the key to add.</param>
	public void Add(TKey key, TValue value)
	{
		keys.Add(key);
		values.Add(value);
	}

	/// <summary>
	/// Removes all key-value pairs from this instance.
	/// </summary>
	public void Clear()
	{
		keys.Clear();
		values.Clear();
	}

	/// <summary>
	/// Creates a copy of this instance.
	/// </summary>
	public ParallelArrays<TKey, TValue> Copy() => new(this);

	/// <summary>
	/// Deletes the key-value pair at the specified index.
	/// </summary>
	public void DeleteAt(int index)
	{
		keys.DeleteAt(index);
		values.DeleteAt(index);
	}

	/// <summary>
	/// Gets the key-value pair at the specified index.
	/// </summary>
	/// <param name="index">The index of the key-value pair to get.</param>
	/// <param name="key">The key at the specified index.</param>
	/// <param name="value">The value at the specified index.</param>
	public void Get(int index, out TKey key, out TValue value)
	{
		key = keys[index];
		value = values[index];
	}

	/// <summary>
	/// Gets the key at the specified index.
	/// </summary>
	/// <param name="index">The index of the key to get.</param>
	public TKey GetKey(int index) => keys[index];

	/// <summary>
	/// Gets the value at the specified index.
	/// </summary>
	/// <param name="index">The index of the key to get.</param>
	public TValue GetValue(int index) => values[index];

	/// <summary>
	/// Inserts a key-value pair at the specified index.
	/// </summary>
	/// <param name="index">The index at which to insert the key-value pair.</param>
	/// <param name="key">The key to insert.</param>
	/// <param name="value">The value associated with the key to insert.</param>
	public void InsertAt(int index, TKey key, TValue value)
	{
		keys.InsertAt(key, index);
		values.InsertAt(value, index);
	}

	/// <summary>
	/// Removes the last key-value pair from this instance.
	/// </summary>
	/// <param name="key">The remove key.</param>
	/// <param name="value">The value associated with the removed key.</param>
	public void RemoveLast(out TKey key, out TValue value)
	{
		key = keys.RemoveLast();
		value = values.RemoveLast();
	}

	/// <summary>
	/// Removes the last <paramref name="n"/> key-value pairs from this instance.
	/// </summary>
	/// <param name="n">The number of key-value pairs to remove.</param>
	public void RemoveLast(int n)
	{
		keys.RemoveLast(n);
		values.RemoveLast(n);
	}

	/// <summary>
	/// Sets the key-value pair at the specified index.
	/// </summary>
	/// <param name="index">The index of the key-value pair to set.</param>
	/// <param name="key">The key to set.</param>
	/// <param name="value">The value associated with the key to set.</param>
	public void Set(int index, TKey key, TValue value)
	{
		keys[index] = key;
		values[index] = value;
	}

	/// <inheritdoc />
	public override string ToString()
		=> keys.Zip(values).Select(t => t.Pretty()).Pretty();
}
