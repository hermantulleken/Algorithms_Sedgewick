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
}
