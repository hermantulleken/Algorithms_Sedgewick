using System.Collections;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Object;

namespace Algorithms_Sedgewick.Queue;

/// <summary>
/// Queue implementation using a linked list with a node pool.
/// </summary>
/// <typeparam name="T">The type of elements that can be inserted into the queue.</typeparam>
public class QueueWithLinkedListAndNodePool<T> : IQueue<T>
{
	private const bool ToStringShowsContents = false;
	
	// ReSharper disable once StaticMemberInGenericType
	private static readonly IdGenerator IdGenerator = new();

	private readonly LinkedListWithPooledClassNodes<T> items;

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
			return items.First.Item!;
		}
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="QueueWithLinkedListAndNodePool{T}"/> class.
	/// </summary>
	/// <param name="capacity">The capacity of this queue.</param>
	public QueueWithLinkedListAndNodePool(int capacity)
	{
		items = new LinkedListWithPooledClassNodes<T>(capacity);
	}

	/// <inheritdoc/>
	public void Clear() => items.Clear();

	/// <inheritdoc/>
	public T Dequeue()
	{
		var item = items.First.Item;
		items.RemoveFromFront();
		
		return item!;
	}

	/// <inheritdoc/>
	public void Enqueue(T item)
	{
		item.ThrowIfNull();
		items.InsertAtBack(item);
	}

	/// <inheritdoc/>
	public override int GetHashCode() => Id.GetHashCode();

	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator() => items.GetEnumerator()!;

#pragma warning disable CS0162
	
	/// <inheritdoc/>
	// ReSharper disable once HeuristicUnreachableCode
	// We use the constant bool for easy switching when we debug.
	public override string ToString() => ToStringShowsContents ? items.ToString() : "Q: " + Id;

#pragma warning restore CS0162

	/// <inheritdoc/>
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private void ValidateNotEmpty()
	{
		if (IsEmpty)
		{
			ThrowHelper.ThrowContainerEmpty();
		}
	}
}
