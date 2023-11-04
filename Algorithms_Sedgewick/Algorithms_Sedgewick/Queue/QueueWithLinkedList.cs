using System.Collections;
using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick.Queue;

public sealed class QueueWithLinkedList<T> : IQueue<T>
{
	private const bool ToStringShowsContents = false;
	private static readonly IdGenerator IdGenerator = new();

	private readonly List.LinkedList<T> items = new();

	public int Id { get; } = IdGenerator.GetNextId();
	
	public int Count => items.Count;

	public bool IsEmpty => items.IsEmpty;

	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items.First();
		}
	}

	public void Clear() => items.Clear();

	public T Dequeue() => items.RemoveFromFront().Item;

	public void Enqueue(T item) => items.InsertAtBack(item);

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

#pragma warning disable CS0162 
	
	// We use the constant bool for easy switching when we debug.
	public override string ToString() => ToStringShowsContents ? items.ToString() : "Q: " + Id;
#pragma warning restore CS0162

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}

	public override int GetHashCode() => Id.GetHashCode();
}
