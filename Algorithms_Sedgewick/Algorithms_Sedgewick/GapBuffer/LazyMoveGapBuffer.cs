namespace Algorithms_Sedgewick.GapBuffer;

/// <summary>
/// This gap buffer does not immediately copy elements around when moving the cursor; the copying
/// only happens once we remove or add elements. 
/// </summary>
/// <inheritdoc cref="IGapBuffer{T}"/>
/// <remarks>Suitable to use when moving the cursor a lot compared to adding or removing elements.</remarks>
/*
	I would like to make this class "sealed except for RandomAccessLazyMoveGapBuffer.
	
	I considered several alternatives:
	
		-	Making both classes private inner classes of a static class that provides
			factory methods. This would be my preferred approach but is not consistent 
			with the way we create other containers.
			
		-	Duplicating the functionality. It seems better to leave this class open and 
			share the common implementation to reduce bugs and make the code more 
			maintainable. 
			
		-	In RandomAccessLazyMoveGameBuffer, to use an internal LazyMoveGapBuffer, and list
			and delegate all operations to either object. However, we will need to expose
			the list in LazyMoveGapBuffer, or make it possible to construct LazyMoveGapBuffer 
			from a list so that it can be shared between the RandomAccessLazyMoveGameBuffer and
			internal LazyMoveGapBuffer (otherwise they will go out of sync). Neither option
			is desirable, because it allows the list to be modified by other code too. 
			
	In the end, all the other approaches had problems, so I settled on this. 
*/
public class LazyMoveGapBuffer<T> : IGapBuffer<T>
{
	private readonly IGapBuffer<T> eagerBuffer;

	private int cursorIndex;

	public int Count => eagerBuffer.Count;

	public int CursorIndex => cursorIndex;

	public LazyMoveGapBuffer(Func<IGapBuffer<T>> eagerBufferFactory)
    	: this(eagerBufferFactory())
    {
    }

	protected LazyMoveGapBuffer(int initialCapacity) 
    	: this(() => new GapBufferWithArray<T>(initialCapacity))
    {
    }

	protected LazyMoveGapBuffer(IGapBuffer<T> eagerBuffer) => this.eagerBuffer = eagerBuffer;

	public void AddAfter(T item)
    {
    	UpdateCursorPosition();
    	eagerBuffer.AddAfter(item);
    }

	public void AddBefore(T item)
    {
    	UpdateCursorPosition();
    	eagerBuffer.AddBefore(item);
    }

	public void MoveCursor(int newCursorIndex) => cursorIndex = newCursorIndex;

	public T RemoveAfter()
    {
    	UpdateCursorPosition();
    	return eagerBuffer.RemoveAfter();
    }

	public T RemoveBefore()
    {
    	UpdateCursorPosition();
    	return eagerBuffer.RemoveBefore();
    }

	private void UpdateCursorPosition()
    {
    	if (eagerBuffer.CursorIndex != cursorIndex)
    	{
    		eagerBuffer.MoveCursor(cursorIndex);
    	}
    }
}
