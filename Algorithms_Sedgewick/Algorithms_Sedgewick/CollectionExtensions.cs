﻿namespace Algorithms_Sedgewick;

using List;
using Stack;

public static class CollectionExtensions
{
	public static TStack Copy<TStack, T>(this TStack stack) 
		where TStack : IStack<T>, new()
	{
		var copy = new TStack();

		foreach (var item in stack)
		{
			copy.Push(item);
		}

		return copy;
	}

	public static ResizeableArray<T> Copy<T>(this ResizeableArray<T> array)
	{
		var newArray = new ResizeableArray<T>(array.Capacity);
		
		foreach (var item in array)
		{
			newArray.Add(item);
		}

		return newArray;
	}
	
	public static ResizeableArray<T> ToResizableArray<T>(this IEnumerable<T> items, int capacity)
	{
		var array = new ResizeableArray<T>(capacity);
			
		foreach (var item in items)
		{
			array.Add(item);
		}

		return array;
	}
}
