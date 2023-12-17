namespace AlgorithmsSW.EdgeWeightedGraph;

using List;

public class ConditionalStack<T>
{
	private ResizeableArray<T> list = new();
	
	public int Count => list.Count;
	
	public void Push(T item)
	{
		list.Add(item);
	}
	
	public T Pop(Func<T, bool> predicate)
	{
		int index = list
			.IndexWhere(predicate)
			.First();

		return RemoveAt(index);
	}

	public T PopLast() => list.RemoveLast();

	public T PopFirst() => RemoveAt(0);
	
	public T Peek(Func<T, bool> predicate)
	{
		return list.First(predicate);
	}
	
	private T RemoveAt(int index)
	{
		ListExtensions.SwapAt(list, index, list.Count - 1);
		var result = list[^1];
		list.RemoveLast();
		return result;
	}
}
