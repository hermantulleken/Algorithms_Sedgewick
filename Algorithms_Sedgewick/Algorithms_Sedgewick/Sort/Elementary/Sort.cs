#define WHITE_BOX_TESTING

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Algorithms_Sedgewick.List;
using Support;

namespace Algorithms_Sedgewick.Sort;

public static class Sort
{
	public static void Selection<T>(IRandomAccessList<T> list) where T : IComparable
	{
		int length = list.Count;
		
		for (int i = 0; i < length; i++)
		{ 
			// Exchange a[i] with smallest entry in a[i+1...N).
			int minIndex = i; // index of minimal entry.
			
			for (int j = i + 1; j < length; j++)
			{
				if (LessAt(list, j, minIndex))
				{
					minIndex = j;
				}
			}
			
			SwapAt(list, i, minIndex);
		}
	}
	
	public static void Insertion<T>(IRandomAccessList<T> list) where T : IComparable
	{ 
		int length = list.Count;
		
		for (int i = 1; i < length; i++)
		{ 
			// Insert a[i] among a[i-1], a[i-2], a[i-3]... ..
			for (int j = i; j > 0 && LessAt(list, j, j - 1); j--)
			{
				SwapAt(list, j, j - 1);
			}
		}
	}

	public static void Shell<T>(IRandomAccessList<T> list) where T : IComparable
	{
		int length = list.Count;
		int h = 1;
		
		while (h < length / 3)
		{
			h = 3 * h + 1;
		}

		while (h >= 1)
		{
			for (int i = 0; i < length; i++)
			{
				for (int j = i; j >= h && LessAt(list, j, j-h); j -= h)
				{
					SwapAt(list, j, j-h);
				}
			}

			h /= 3;
		}
	}

	// Implements Ex 2.1.14 in Sedgewick
	//This seems to be a  version of gnome sort
	public static void Dequeue<T>(IRandomAccessList<T> list) where T : IComparable
	{
		var deque = new DequeWithDoublyLinkedList<T>();

		foreach (var item in list)
		{
			deque.PushRight(item);
		}
		Console.WriteLine(deque);
		Dequeue(deque);
		
		

		for (int i = 0; i < list.Count; i++)
		{
			list[i] = deque.PopLeft();
		}
	}
	
	// Implements Ex 2.1.14 in Sedgewick
	// This seems to be a  version of gnome sort
	public static void Dequeue<T>(IDeque<T> deque) where T : IComparable
	{
		#if WHITE_BOX_TESTING
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckBottomSortedDescending(DequeueSortHelper<T> deck, int n)
			=> Debug.Assert(IsSortedDescending(deck.TopN(n)), nameof(CheckBottomSortedDescending));

		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopSortedDescending(DequeueSortHelper<T> deck, int n)
			=> Debug.Assert(n == 0 || IsSortedDescending(deck.BottomN(n - 1)), nameof(CheckTopSortedDescending));
		
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopIsSmallerThanBottom(DequeueSortHelper<T> deck, int bottomCount)
			=> Debug.Assert(bottomCount == 0 || deck.BottomN(bottomCount).Min().CompareTo(deck.Top) >= 0, nameof(CheckTopIsSmallerThanBottom));

		
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopBiggerThanTop(DequeueSortHelper<T> deck, int topCount)
			=> Debug.Assert(deck.TopN(topCount).Max().CompareTo(deck.Top) >= 0, nameof(CheckTopBiggerThanTop));
			
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckResultIsSorted(IRandomAccessList<T> list)
			=> Debug.Assert(IsSorted(list), nameof(CheckResultIsSorted));
		#endif

		int count = deque.Count;
		var deck = new DequeueSortHelper<T>(deque);
		
		void GetNthSmallestOnTop(int n)
		{
			int stepCount = count - n;
			Console.WriteLine(stepCount);
			for (int i = 0; i < stepCount; i++)
			{
				var (top, belowTop) = deck.PeekTop2();
				if (Less(top, belowTop))
				{
					deck.ExchangeTop();
					Console.WriteLine("..." + deck);
				}
				
				deck.TopToBottom();
				Console.WriteLine("..." + deck);
			}
		}

		void NTopToBottom(int n)
		{
			for (int i = 0; i < n; i++)
			{
				deck.TopToBottom();
			}
		}

		for (int i = 0; i < count; i++)
		{
			GetNthSmallestOnTop(i);
			
			Console.WriteLine(deck);
			CheckBottomSortedDescending(deck, i);
			CheckTopIsSmallerThanBottom(deck, count - i - 1);
			CheckTopBiggerThanTop(deck, i + 1);
			
			NTopToBottom(i + 1);
			
			Console.WriteLine(deck);
			CheckTopSortedDescending(deck, i + 1);
			
			Console.WriteLine("----");
		}
	}

	public static void GnomeSort<T>(IRandomAccessList<T> list) where T : IComparable
	{
		int i = 0;
		while (i < list.Count)
		{
			if (i == 0 ||  LessAt(list, i-1, i))
			{
				i++;
			}
			else
			{
				SwapAt(list, i - 1, i);
				i--;
			}
		}
	}
	
	private sealed class DequeueSortHelper<T>
	{
		private readonly IDeque<T> deque;

		public DequeueSortHelper(IDeque<T> deque)
		{
			this.deque = deque;
		}
		
		public void ExchangeTop()
		{
			var card1 = deque.PopRight();
			var card2 = deque.PopRight();
			
			deque.PushRight(card1);
			deque.PushRight(card2);
		}

		public T Top => deque.PeekRight;
		
		public (T card1, T card2) PeekTop2()
		{
			var card1 = deque.PopRight();
			var card2 = deque.PeekRight;
			
			deque.PushRight(card1);

			return (card1, card2);
		}

		public void TopToBottom()
		{
			var top = deque.PopRight();
			deque.PushLeft(top);
		}

#if WHITE_BOX_TESTING

		public T[] ToArray() => deque.ToArray();
		public T[] ToReverseArray() => deque.Reverse().ToArray(); //We reverse the list so the top is at 0

		public T[] TopN(int n) => ToReverseArray().Take(n).ToArray();

		public T[] BottomN(int n) => ToReverseArray().TakeLast(n).ToArray();

		
#endif
		
		public override string ToString() => deque.ToString();
	}

	private static bool IsSortedAscending<T>(T[] array) where T : IComparable
	{
		for (int i = 1; i < array.Length; i++)
		{
			if (array[i].CompareTo(array[i - 1]) < 0)
			{
				return false;
			}
		}

		return true;
	}
	
	public static bool IsSortedDescending<T>(T[] array) where T : IComparable
	{
		for (int i = 1; i < array.Length; i++)
		{
			if (array[i].CompareTo(array[i - 1]) > 0)
			{
				return false;
			}
		}

		return true;
	}
	
	/// <summary>
	/// Checks whether a list is sorted.
	/// </summary>
	/// <param name="list">A list of elements to check. Cannot be null.</param>
	/// <param name="comparer">A comparer to use to compare elements. If not supplied
	/// or null, <see cref="Comparer{T}.Default"/> is used.</param>
	/// <typeparam name="T"></typeparam>
	/// <returns><see langword="true"/> the list is sorted, <see langword="false"/> otherwise.</returns>
	public static bool IsSorted<T>([NotNull] IRandomAccessList<T> list, [AllowNull] IComparer<T> comparer = null)
	{
		if (list == null)
		{
			throw new ArgumentNullException(nameof(list));	
		}
		
		comparer ??= Comparer<T>.Default;
		
		if (list.Count <= 1)
		{
			return true;
		}
		
		//We have at least two elements
		Debug.Assert(list.Count >= 2);
		
		for (int i = 1; i < list.Count; i++)
		{
			//Negative indexes are impossible
			Debug.Assert(i - 1 >= 0);
			
			var item0 = list[i - 1];
			var item1 = list[i];
			
			if(comparer.Compare(item0, item1) > 0)
			{
				return false;
			}
			
			//All items up to i are sorted
		}
		
		//All items up to the last index are sorted
		return true;
	}
	
	internal static bool Less<T>(T v, T w) where T : IComparable => v.CompareTo(w) < 0;
	internal static bool LessOrEqual<T>(T v, T w) where T : IComparable => v.CompareTo(w) <= 0;

	internal static bool LessAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable => Less(list[i], list[j]);
	internal static bool LessOrEqualAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable => Less(list[i], list[j]);
	
	internal static void SwapAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable => (list[i], list[j]) = (list[j], list[i]);
}
