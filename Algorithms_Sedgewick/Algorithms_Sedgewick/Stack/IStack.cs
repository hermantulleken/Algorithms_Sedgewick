namespace Algorithms_Sedgewick.Stack;

public interface IStack<T> : IEnumerable<T>
{
	public int Count { get; }
	public bool IsEmpty => Count == 0;
	public T Peek { get; }

	public void Push(T item);
	public T Pop();

	public void Clear();
}
