namespace Algorithms_Sedgewick.List;

public interface IReadonlyRandomAccessList<T> : IEnumerable<T>
{
	int Count { get; }
	bool IsEmpty { get; }
	T this[int index] { get; set; }
	IReadonlyRandomAccessList<T> Copy();
}
