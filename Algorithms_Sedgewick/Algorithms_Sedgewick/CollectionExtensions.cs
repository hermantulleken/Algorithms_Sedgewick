using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick;

public static class CollectionExtensions
{
	public static TStack Copy<TStack, T>(this TStack stack) where TStack : IStack<T>, new()
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
}
