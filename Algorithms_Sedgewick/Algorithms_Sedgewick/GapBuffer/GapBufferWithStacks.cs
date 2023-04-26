using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick.GapBuffer;

/// <summary>
/// A gap buffer implemented with two stacks. 
/// </summary>
/// <inheritdoc cref="IGapBuffer{T}"/>
public class GapBufferWithStacks<T> : IGapBuffer<T>
{
	private readonly IStack<T> itemsAfterCursor;
	private readonly IStack<T> itemsBeforeCursor;

	public int Count => itemsBeforeCursor.Count + itemsAfterCursor.Count;

	public int CursorIndex => itemsBeforeCursor.Count;

	public T this[int index]
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public GapBufferWithStacks(Func<IStack<T>> stackFactory)
	{
		itemsBeforeCursor = stackFactory();
		itemsAfterCursor = stackFactory();
	}

	public void AddAfter(T item) => itemsAfterCursor.Push(item);

	public void AddBefore(T item) => itemsBeforeCursor.Push(item);

	public void MoveCursor(int newCursorIndex)
	{
		void MoveCursorLeft() => itemsAfterCursor.Push(itemsBeforeCursor.Pop());
		void MoveCursorRight() => itemsBeforeCursor.Push(itemsAfterCursor.Pop());
	
		newCursorIndex.ThrowIfOutOfRange(0, Count + 1);

		int difference = newCursorIndex - CursorIndex;

		if (difference > 0)
		{
			for (int i = 0; i < difference; i++)
			{
				MoveCursorRight();
			}
		}
		else
		{
			for (int i = 0; i < -difference; i++)
			{
				MoveCursorLeft();
			}
		}
	}

	public T RemoveAfter()
	{
		if (itemsAfterCursor.IsEmpty)
		{
			ThrowHelper.ThrowGapAtEnd();
		}

		return itemsAfterCursor.Pop();
	}

	public T RemoveBefore()
	{
		if (itemsBeforeCursor.IsEmpty)
		{
			ThrowHelper.ThrowGapAtBeginning();
		}

		return itemsBeforeCursor.Pop();
	}
}
