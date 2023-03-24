namespace Algorithms_Sedgewick.Buffer;

public interface IBuffer<T> : IEnumerable<T>
{
	public int Count { get; }

	public int Capacity { get; }
    
	public void Insert(T item);
	
	public void Clear();
	
	public T First { get; }
	
	public T Last { get; }
}
