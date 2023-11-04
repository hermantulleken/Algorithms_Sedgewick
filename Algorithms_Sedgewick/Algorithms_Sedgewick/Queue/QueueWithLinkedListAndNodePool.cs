using System.Collections;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick.Queue;

/// <summary>
/// Queue implementation using a linked list with a node pool.
/// </summary>
/// <typeparam name="T"></typeparam>
public class QueueWithLinkedListAndPooledNodes<T> : IQueue<T>
{
	private const bool ToStringShowsContents = false;
	
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();

	private readonly LinkedListWithPooledNodes<T> items;

	/// <summary>
	/// Gets the Id for this instance.
	/// </summary>
	/// <remarks>Ids are unique for among instances that share the static <see cref="IdGenerator"/>.
	/// </remarks>
	public int Id { get; } = IdGenerator.GetNextId();
	
	/// <inheritdoc />
	public int Count => items.Count;

	/// <inheritdoc />
	public bool IsEmpty => items.IsEmpty;

	/// <inheritdoc />
	public T Peek
	{
		get
		{
			ValidateNotEmpty();
			return items.First();
		}
	}
	
	public QueueWithLinkedListAndPooledNodes(int capacity)
	{
		items = new LinkedListWithPooledNodes<T>(capacity);
	}

	public void Clear() => items.Clear();

	public T Dequeue()
	{
		var item = items.First.Item;
		items.RemoveFromFront();
		
		return item;
	}

	public void Enqueue(T item) => items.InsertAtBack(item);
	
	public override int GetHashCode() => Id.GetHashCode();

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

#pragma warning disable CS0162
	
	/// <inheritdoc/>
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
}
