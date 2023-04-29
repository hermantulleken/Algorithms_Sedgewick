﻿namespace Algorithms_Sedgewick.GapBuffer;

/// <summary>
/// Represents a gap buffer, a data structure that allows efficient insertions and deletions at arbitrary positions.
/// </summary>
/// <typeparam name="T">The type of elements in the gap buffer.</typeparam>
/// <remarks>
/// A gap buffer is a dynamic array that supports efficient insertions and deletions near a "cursor".
/// Elements can be added or removed before or after the cursor. The cursor can be moved to any position in the buffer.
/// </remarks>
public interface IGapBuffer<T>
{
	/// <summary>
	/// Gets the number of elements in the gap buffer. 
	/// </summary>
	public int Count { get; }

	/// <summary>
	/// Gets the current position of the cursor within the gap buffer.
	/// </summary>
	public int CursorIndex { get; }

	/// <summary>
	/// Adds an element after the cursor.
	/// </summary>
	/// <param name="item">The element to add.</param>
	public void AddAfter(T item);

	/// <summary>
	/// Adds an element before the cursor.
	/// </summary>
	/// <param name="item">The element to add.</param>
	public void AddBefore(T item);

	/// <summary>
	/// Moves the cursor to the specified index.
	/// </summary>
	/// <param name="newCursorIndex">The new index for the cursor.</param>
	public void MoveCursor(int newCursorIndex);

	/// <summary>
	/// Removes an element after the cursor.
	/// </summary>
	/// <returns>
	/// The removed element.
	/// </returns>
	public T RemoveAfter();

	/// <summary>
	/// Removes an element before the cursor.
	/// </summary>
	/// <returns>
	/// The removed element.
	/// </returns>
	public T RemoveBefore();
}