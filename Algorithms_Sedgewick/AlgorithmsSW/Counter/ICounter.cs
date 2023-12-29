namespace AlgorithmsSW.Counter;

/// <summary>
/// A data structure that keeps track of the number of times an item is added or removed.
/// </summary>
/// <remarks>This is also sometimes called a bag (not to be confused with <see cref="Bag"/>), or multiset
/// (although a multiset can also refer to an other data structure).
/// </remarks>
public interface ICounter<T>
{
	IEnumerable<T> Keys { get; }
	
	int this[T item] { get; }
	
	void Add(T item);
	
	void Remove(T item);

	void Clear();
}
