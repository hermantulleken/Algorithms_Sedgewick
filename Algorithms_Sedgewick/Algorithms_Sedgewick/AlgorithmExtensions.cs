using System.Diagnostics;
using Algorithms_Sedgewick.List;

namespace Algorithms_Sedgewick;

public static class AlgorithmExtensions
{
	public static T Last<T>(this IRandomAccessList<T> source)
	{
		if (source.IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}

		return source[^1];
	}
	
	public static T First<T>(this IRandomAccessList<T> source)
	{
		if (source.IsEmpty)
		{
			throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
		}

		return source[0];
	}
	
	public static LinkedList<T>.Node Nth<T>(this LinkedList<T> list, int n) => list.Nodes.Skip(n).First();
	
	public static LinkedList<T>.Node RemoveNth<T>(this LinkedList<T> list, int n)
	{
		if (n < 0 || n >= list.Count)
		{
			throw new ArgumentException(null, nameof(n));
		}
		
		if (n == 0)
		{
			return list.RemoveFromFront();
		}

		var previous = list.Nth(n - 1);
		return list.RemoveAfter(previous);
	}

	public static IEnumerable<LinkedList<T>.Node> NodesBeforeThat<T>(this LinkedList<T> list, Func<LinkedList<T>.Node, bool> predicate)
	{
		if (list.IsEmpty || list.Count == 1)
		{
			return Array.Empty<LinkedList<T>.Node>();
		}
		
		return list.Nodes
			.SkipLast(1)
			.Where(node => predicate(node.NextNode));
	}

	public static void RemoveIf<T>(this LinkedList<T> list, Func<LinkedList<T>.Node, bool> predicate)
	{
		if (list.IsEmpty)
		{
			return;
		}

		var current = list.First;

		while (current != null && predicate(current))
		{
			list.RemoveFromFront();
			current = list.First;
		}

		if (list.IsEmpty)
		{
			return;
		}

		Debug.Assert(current != null);
		
		while (current.NextNode != null)
		{
			if (predicate(current.NextNode))
			{
				list.RemoveAfter(current);
			}
			
			current = current.NextNode;
		}
	}
	
	public static IEnumerable<T> JosephusSequence<T>(this IEnumerable<T> list, int m)
	{
		var queue = new QueueWithLinkedList<T>();

		int i = 0;
		foreach (var item in list)
		{
			if (i % m == 0)
			{
				yield return item;
			}
			else
			{
				queue.Enqueue(item);
			}

			i++;
		}

		while (queue.Count > 1)
		{
			var item = queue.Dequeue();

			if (i % m == 0)
			{
				yield return item;
			}
			else
			{
				queue.Enqueue(item);
			}

			i++;
		}

		if (queue.Count == 1)
		{
			yield return queue.Dequeue();
		}
	}

}
