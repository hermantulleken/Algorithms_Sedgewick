using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick.Pool;

public class FixedPreInitializedPool<T>
{
	private readonly FixedCapacityStack<T> pool;
	
	private readonly IFactory<T> factory;

	public int AliveElementCount => Capacity - AsleepElementCount;
	
	public int AsleepElementCount => pool.Count;
	
	public int Capacity { get; }

	public FixedPreInitializedPool(IFactory<T> factory, int capacity)
	{
		Capacity = capacity;
		pool = new FixedCapacityStack<T>(capacity);

		this.factory = factory;
		for (int i = 0; i < capacity; i++)
		{
			pool.Push(factory.GetNewInstance());
		}
	}
	
	public T Get()
	{
		var element = pool.Pop();
		
		return element;
	}

	public void Return(T element)
	{
		factory.Reset(element);
		pool.Push(element);
	}
}
