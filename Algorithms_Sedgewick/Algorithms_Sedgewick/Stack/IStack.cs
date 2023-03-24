namespace Algorithms_Sedgewick.Stack;

public interface IStack<T> : IEnumerable<T>
{
	public int Count { get; }
	public bool IsEmpty => Count == 0;
	public T Peek { get; }

	public void Clear();
	public T Pop();

	public void Push(T item);
}
