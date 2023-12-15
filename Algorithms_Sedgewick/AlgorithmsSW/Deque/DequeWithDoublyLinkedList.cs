using System.Collections;
using AlgorithmsSW.List;
using Support;

namespace AlgorithmsSW.Deque;

public class DequeWithDoublyLinkedList<T> : IDeque<T>
{
	private readonly DoublyLinkedList<T> items = new();
	
	public int Count => items.Count;
	
	public T PeekLeft => items.First.Item;
	
	public T PeekRight => items.Last.Item;

	public void Clear() => items.Clear();

	public IEnumerator<T> GetEnumerator() => items.GetEnumerator();
	
	public T PopLeft() => items.RemoveFromFront().Item;
	
	public T PopRight() => items.RemoveFromBack().Item;

	public void PushLeft(T item) => items.InsertAtFront(item);
	
	public void PushRight(T item) => items.InsertAtBack(item);

	public override string ToString() => this.Pretty();
	
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
