namespace Algorithms_Sedgewick.List;

using Support;

public class ParallelArrays<TKey, TValue>
{
	private readonly ResizeableArray<TKey> keys;
	private readonly ResizeableArray<TValue> values;
	
	public bool IsEmpty => Count == 0;

	public int Capacity => keys.Capacity;

	public int Count => keys.Count;
	
	public bool IsFull => Count == Capacity;
	
	public IReadonlyRandomAccessList<TKey> Keys => keys;
	
	public IReadonlyRandomAccessList<TValue> Values => values;

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

	public void Add(TKey key, TValue value)
	{
		keys.Add(key);
		values.Add(value);
	}

	public void Get(int index, out TKey key, out TValue value)
	{
		key = keys[index];
		value = values[index];
	}
	
	public TKey GetKey(int index) => keys[index];
	
	public TValue GetValue(int index) => values[index];

	public void Set(int index, TKey key, TValue value)
	{
		keys[index] = key;
		values[index] = value;
	}
	
	public ParallelArrays<TKey, TValue> Copy()
		=> new ParallelArrays<TKey, TValue>(this);

	public void InsertAt(int index, TKey key, TValue value)
	{
		keys.InsertAt(key, index);
		values.InsertAt(value, index);
	}

	public void DeleteAt(int index)
	{
		keys.DeleteAt(index);
		values.DeleteAt(index);
	}

	public void RemoveLast(out TKey key, out TValue value)
	{
		key = keys.RemoveLast();
		value = values.RemoveLast();
	}
	
	public void RemoveLast(int n)
	{
		keys.RemoveLast(n);
		values.RemoveLast(n);
	}

	public override string ToString()
		=> keys.Zip(values).Select(t => t.Pretty()).Pretty();

	public void Clear()
	{
		keys.Clear();
		values.Clear();
	}
}
