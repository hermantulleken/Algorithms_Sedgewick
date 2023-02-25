#if DEBUG
#define WHITE_BOX_TESTING
#endif

using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Algorithms_Sedgewick.List;

#if WHITE_BOX_TESTING
using Support;
#endif

namespace Algorithms_Sedgewick.Sort;



public static class Sort
{
		private sealed class DequeueSortHelperWithDeque<T>
	{
		private readonly IDeque<T> deque;

		public DequeueSortHelperWithDeque(IDeque<T> deque)
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

	private sealed class DequeSortHelperWithQueue<T> : IEnumerable<T> where T : IComparable
	{
		private readonly IQueue<T> queue;

		private int Count => queue.Count + 1;

		private T Peek1 { get; set; }

		private T Peek2 => queue.Peek;
		
		public DequeSortHelperWithQueue(IQueue<T> queue)
		{
			if (queue.IsEmpty)
			{
				//We need at least one element.
				throw new InvalidOperationException(ContainerErrorMessages.ContainerEmpty);
			}
			
			this.queue = queue;
			DequeueToRight();
		}

		private void Enqueue(T item) => queue.Enqueue(item);

		public IEnumerable<T> Items
		{
			get
			{
				yield return Peek1;

				foreach (var item in queue)
				{
					yield return item;
				}
			}
		}

		private void DequeueToRight() => Peek1 = queue.Dequeue();

		private void Rotate()
		{
			Enqueue(Peek1);
			DequeueToRight();
		}

		private void Rotate(int n)
		{
			for (int i = 0; i < n; i++)
			{
				Rotate();
			}
		}

		private void RotateSmallest()
		{
			if (Less(Peek2, Peek1))
			{
				queue.Enqueue(queue.Dequeue());
			}
			else
			{
				Rotate();
			}
		}

		private void RotateLargest(int n)
		{
			for (int i = 0; i < n; i++)
			{
				RotateSmallest();
			}
		}

		public void Sort()
		{
			for (int i = 0; i < Count; i++)
			{
				int m = Count - i;
				RotateLargest(m);
				Rotate(i + 1);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return Peek1;
			
			foreach (var item in queue)
			{
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
	
	public static void SelectionSort<T>(IRandomAccessList<T> list) where T : IComparable
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
	
	public static void InsertionSort<T>(IRandomAccessList<T> list) where T : IComparable
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

	public static void ShellSort<T>(IRandomAccessList<T> list) where T : IComparable
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
	public static void DequeueSortWithDeque<T>(IRandomAccessList<T> list) where T : IComparable
	{
		var deque = new DequeWithDoublyLinkedList<T>();

		foreach (var item in list)
		{
			deque.PushRight(item);
		}
		DequeueSortWithDeque(deque);
		
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = deque.PopLeft();
		}
	}
	
	// Implements Ex 2.1.14 in Sedgewick
	//This seems to be a  version of gnome sort
	public static void DequeueSortWithQueue<T>(IRandomAccessList<T> list) where T : IComparable
	{
		if (list.IsEmpty)
		{
			return; //Nothing to do
		}
		
		var queue = new QueueWithLinkedList<T>();

		foreach (var item in list)
		{
			queue.Enqueue(item);
		}
		
		var helper = new DequeSortHelperWithQueue<T>(queue);
		helper.Sort();

		int i = 0;
		foreach (var item in helper.Items)
		{
			list[i] = (item);
			i++;
		}
	}
	
	// Implements Ex 2.1.14 in Sedgewick
	// This seems to be a  version of gnome sort
	public static void DequeueSortWithDeque<T>(IDeque<T> deque) where T : IComparable
	{
		#if WHITE_BOX_TESTING
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckBottomSortedDescending(DequeueSortHelperWithDeque<T> helper, int n)
			=> Debug.Assert(IsSortedDescending(helper.TopN(n)), nameof(CheckBottomSortedDescending));

		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopSortedDescending(DequeueSortHelperWithDeque<T> helper, int n)
			=> Debug.Assert(n == 0 || IsSortedDescending(helper.BottomN(n - 1)), nameof(CheckTopSortedDescending));
		
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopIsSmallerThanBottom(DequeueSortHelperWithDeque<T> helper, int bottomCount)
			=> Debug.Assert(bottomCount == 0 || helper.BottomN(bottomCount).Min().CompareTo(helper.Top) >= 0, nameof(CheckTopIsSmallerThanBottom));

		
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopBiggerThanTop(DequeueSortHelperWithDeque<T> helper, int topCount)
			=> Debug.Assert(helper.TopN(topCount).Max().CompareTo(helper.Top) >= 0, nameof(CheckTopBiggerThanTop));
			
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckResultIsSorted(IRandomAccessList<T> list)
			=> Debug.Assert(IsSorted(list), nameof(CheckResultIsSorted));
		#endif

		int count = deque.Count;
		var helper = new DequeueSortHelperWithDeque<T>(deque);
		
		void GetNthSmallestOnTop(int n)
		{
			int stepCount = count - n;
			for (int i = 0; i < stepCount; i++)
			{
				var (top, belowTop) = helper.PeekTop2();
				if (Less(top, belowTop))
				{
					helper.ExchangeTop();
				}
				
				helper.TopToBottom();
			}
		}

		void NTopToBottom(int n)
		{
			for (int i = 0; i < n; i++)
			{
				helper.TopToBottom();
			}
		}

		for (int i = 0; i < count; i++)
		{
			GetNthSmallestOnTop(i);
#if WHITE_BOX_TESTING
			CheckBottomSortedDescending(helper, i);
			CheckTopIsSmallerThanBottom(helper, count - i - 1);
			CheckTopBiggerThanTop(helper, i + 1);
#endif
			NTopToBottom(i + 1);
		
#if WHITE_BOX_TESTING
			CheckTopSortedDescending(helper, i + 1);
#endif
		}
	}

	public static void GnomeSort<T>(IRandomAccessList<T> list) where T : IComparable
	{
		int i = 0;
		while (i < list.Count)
		{
			if (i == 0 ||  LessOrEqualAt(list, i - 1, i))
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

	public static void MergeSort<T>(IRandomAccessList<T> list) where T : IComparable
	{
		var helpList = new T[list.Count];
		
		void Sort(int start, int end)
		{
			if (end <= start + 1)
			{
				return;
			}

			int middle = start + (end - start) / 2;

			Sort(start, middle);
			Sort(middle, end);
			Merge(list, helpList, start, middle, end);
		}
		
		Sort(0, list.Count);
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

	internal static bool LessAt<T>(T[] list, int i, int j) where T : IComparable => Less(list[i], list[j]);
	
	internal static bool LessAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable => Less(list[i], list[j]);
	internal static bool LessOrEqualAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable => LessOrEqual(list[i], list[j]);
	
	internal static void SwapAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable => (list[i], list[j]) = (list[j], list[i]);

	// p.271
	//I made changes to the end and mid points, so that this is roughly equivalent to their (list, start, middle, end + 1)
	private static void Merge<T>(IRandomAccessList<T> list, T[] helpList, int start, int middle, int end) where T : IComparable
	{
		int i = start;
		int j = middle; //this is middle + 1 in original
		for (int k = start; k < end; k++)//this is <= in original
		{
			helpList[k] = list[k];
		}

		for (int k = start; k < end; k++) //this is <= in original
		{
			if (i >= middle) //This is > in the original
			{
				list[k] = helpList[j];
				j++;
			}
			else if (j >= end)
			{
				list[k] = helpList[i];
				i++;
			}
			else if (LessAt(helpList, j, i))
			{
				list[k] = helpList[j];
				j++;
			}
			else
			{
				list[k] = helpList[i];
				i++;
			}
		}
	}
}
