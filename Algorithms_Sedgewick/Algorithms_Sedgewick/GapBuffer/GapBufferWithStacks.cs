namespace Algorithms_Sedgewick.GapBuffer;

using Stack;

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
	
	public IGapBuffer<T> @this => this;

	public GapBufferWithStacks(Func<IStack<T>> stackFactory)
	{
		itemsBeforeCursor = stackFactory();
		itemsAfterCursor = stackFactory();
	}

	public void AddAfter(T item) => itemsAfterCursor.Push(item);

	public void AddBefore(T item) => itemsBeforeCursor.Push(item);

	public void MoveCursor(int newCursorIndex)
	{
		int offset = newCursorIndex - CursorIndex;

		MoveCursorBy(offset);
	}

	public void MoveCursorBy(int offset)
	{
		int newCursorIndex = CursorIndex + offset;

		((IGapBuffer<T>)this).ValidateCursor(newCursorIndex);
		
		void MoveCursorLeft() => itemsAfterCursor.Push(itemsBeforeCursor.Pop());
		void MoveCursorRight() => itemsBeforeCursor.Push(itemsAfterCursor.Pop());
		
		if (offset > 0)
		{
			for (int i = 0; i < offset; i++)
			{
				MoveCursorRight();
			}
		}
		else
		{
			for (int i = 0; i < -offset; i++)
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
