using System.Collections;

namespace Algorithms_Sedgewick.Buffer;

public sealed class FullCapacity2Buffer<T> : IBuffer<T>
{
	private T item1;
	private T item2;
	private bool firstIsItem1;
	public int Count { get; private set; }
	public int Capacity => 2;
    
	public T First 
		=> (Count == 0) ? throw ThrowHelper.ContainerEmptyException : FirstUnsafe;
    
	public T Last 
		=> (Count == 0) ? throw ThrowHelper.ContainerEmptyException : LastUnsafe;
    
	private T FirstUnsafe => firstIsItem1 ? item1 : item2;
	private T LastUnsafe => firstIsItem1 ? item2 : item1;
    
	public FullCapacity2Buffer() => Clear();
	public IEnumerator<T> GetEnumerator()
	{
		if(Count > 0) yield return FirstUnsafe;
		if(Count > 1) yield return LastUnsafe;
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	public void Insert(T item)
	{
		if (firstIsItem1)
		{
			item1 = item;
		}
		else
		{
			item2 = item;
		}
		if (Count < 2)
		{
			Count++;
		}
		firstIsItem1 = !firstIsItem1;
	}
	public void Clear()
	{
		Count = 0;
		item1 = default;
		item2 = default;
		firstIsItem1 = false; //true after first insertion
	}
}