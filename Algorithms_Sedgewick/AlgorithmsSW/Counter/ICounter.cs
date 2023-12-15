namespace AlgorithmsSW.Counter;

public interface ICounter<T>
{
	IEnumerable<T> Keys { get; }
	
	int this[T item] { get; }
	
	void Add(T item);
	
	void Remove(T item);

	void Clear();
}
