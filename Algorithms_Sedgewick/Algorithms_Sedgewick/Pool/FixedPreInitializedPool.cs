using Algorithms_Sedgewick.Stack;
using static System.Diagnostics.Debug;

namespace Algorithms_Sedgewick.Pool;

public class FixedPreInitializedPool<T>
{
	private readonly FixedCapacityStack<T> pool;
	private readonly Set.HashSet<T> aliveElements;
	private readonly int elementCount;

	public FixedPreInitializedPool(IFactory<T> factory, int elementCount, IComparer<T> comparer)
	{
		this.elementCount = elementCount;
		pool = new FixedCapacityStack<T>(elementCount);
		aliveElements = new Set.HashSet<T>(elementCount, comparer);

		for (int i = 0; i < elementCount; i++)
		{
			pool.Push(factory.GetNewInstance());
		}
	}
	
	public T Get()
	{
		var element = pool.Pop();
		aliveElements.Add(element);
		
		Assert(CollectionCountInvariant);
		
		return element;
	}

	public void Return(T element)
	{
		aliveElements.Remove(element);
		pool.Push(element);
		
		Assert(CollectionCountInvariant);
	}

	private bool CollectionCountInvariant => pool.Count + aliveElements.Count == elementCount;
}
