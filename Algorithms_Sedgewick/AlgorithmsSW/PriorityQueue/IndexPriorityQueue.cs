namespace AlgorithmsSW.PriorityQueue;

public class IndexPriorityQueue<T>
{
	private class IndexComparer(T[] items, IComparer<T> comparer) : IComparer<int>
	{
		public int Compare(int x, int y) => comparer.Compare(items[x], items[y]);
	}
	
	private readonly T[] items;
	private readonly bool[] contains;
	private readonly IPriorityQueue<int> indices;

	public int Count => indices.Count;
	
	public int PeekIndexOfMin{ get; }

	public IndexPriorityQueue(int maxIndex, IComparer<T> comparer)
	{
		items = new T[maxIndex];
		contains = new bool[maxIndex];
		
		indices = new FixedCapacityMinBinaryHeap<int>(maxIndex, new IndexComparer(items, comparer));
	}
	
	public void Insert(int index, T item)
	{
		items[index] = item;
		contains[index] = true;
		indices.Push(index);
	}
	
	public void Change(int index, T item) => items[index] = item;
	
	public bool Contains(int index) => contains[index];

	public int PopMin()
	{
		int index = indices.PopMin();
		contains[index] = false;
		items[index] = default;
		
		return index;
	}
	
	public bool IsEmpty() => indices.IsEmpty();
}
