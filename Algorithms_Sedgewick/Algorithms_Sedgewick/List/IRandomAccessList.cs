namespace Algorithms_Sedgewick.List;

public interface IRandomAccessList<T> : IEnumerable<T>
{
	int Count { get; }
	bool IsEmpty { get; }
	T this[int index] { get; set; }
	IRandomAccessList<T> Copy();
}
