using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Algorithms_Sedgewick.List;
using Support;

namespace Algorithms_Sedgewick;



public static class Sort
{

	public struct MergeSortConfig
	{
		public static MergeSortConfig Vanilla => new()
		{
			SkipMergeWhenSorted = false,
			SmallArraySortAlgorithm = SortAlgorithm.Merge,
			SmallArraySize = 0,
			UseFastMerge = false
		};
		
		public static MergeSortConfig Optimized => new()
		{
			SkipMergeWhenSorted = true,
			SmallArraySortAlgorithm = SortAlgorithm.Insert,
			SmallArraySize = 12,
			UseFastMerge = true
		};
		
		public enum SortAlgorithm
		{
			Small,
			Insert,
			Shell,
			Merge
		};
		
		public bool SkipMergeWhenSorted = true;
		public SortAlgorithm SmallArraySortAlgorithm = SortAlgorithm.Small;
		public bool UseFastMerge = true;
		public int SmallArraySize = 12;

		public MergeSortConfig()
		{
		}
	}
	
#if WHITEBOXTESTING
	public static readonly Counter<string> Counter = new Counter<string>();	
#endif
	
	private static readonly int[] CiuraGaps = { 1, 4, 10, 23, 57, 132, 301, 701 };

	//From: https://stackoverflow.com/a/50470237/335144
	private static readonly int[][] SmallArrayGaps =
	{
		new[] { 4, 1 }, //for 6 elements
		new[] { 5, 1 }, //7
		new[] { 6, 1 }, //8
		new[] { 9, 6, 1 },//9
		new[] { 10, 6, 1 },//10
		new[] { 5, 1 }//10
	};
	
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

#if WHITEBOXTESTING
		public T[] ToArray() => deque.ToArray();
		public T[] ToReverseArray() => deque.Reverse().ToArray(); //We reverse the list so the top is at 0
		public T[] TopN(int n) => ToReverseArray().Take(n).ToArray();
		public T[] BottomN(int n) => ToReverseArray().TakeLast(n).ToArray();
#endif
		
		public override string ToString() => deque.ToString();
	}

	private sealed class DequeSortHelperWithQueue<T> : IEnumerable<T> where T : IComparable<T>
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
	
	public static void SelectionSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
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

	public static void InsertionSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
		InsertionSort(list, 0, list.Count);
	}

	public static void InsertionSort<T>(IRandomAccessList<T> list, int start, int end) where T : IComparable<T>
	{ 
		//int length = list.Count;
		
		for (int i = start + 1; i < end; i++)
		{ 
			// Insert a[i] among a[i-1], a[i-2], a[i-3]... ..
			for (int j = i; j > start && LessAt(list, j, j - 1); j--)
			{
				SwapAt(list, j, j - 1);
			}
		}
	}

	public static void ShellSortWithPrattSequence<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
		int length = list.Count;
		int stepSize = 1;
		
		while (stepSize < length / 3)
		{
			stepSize = 3 * stepSize + 1;
		}

		while (stepSize >= 1)
		{
			for (int i = 0; i < length; i++)
			{
				for (int j = i; j >= stepSize && LessAt(list, j, j - stepSize); j -= stepSize)
				{
					SwapAt(list, j, j - stepSize);
				}
			}

			stepSize /= 3;
		}
	}
	
	public static void ShellSort<T>(IRandomAccessList<T> list, int[] stepSizes) where T : IComparable<T>
	{
		ClearCounter();
		int length = list.Count;

		for (int stepSizeIndex = 0; stepSizeIndex <= stepSizes.Length; stepSizeIndex++)
		{
			int stepSize = stepSizes[stepSizeIndex];
			
			for (int i = 0; i < length; i++)
			{
				for (int j = i; j >= stepSize && LessAt(list, j, j - stepSize); j -= stepSize)
				{
					SwapAt(list, j, j - stepSize);
				}
			}
		}
	}

	public static void SortSmall<T>(IRandomAccessList<T> list)where T : IComparable<T>
	{
		ClearCounter();
		int length = list.Count;
		
		switch (length)
		{
			case <= 1:
				return;
			case <= 5:
				InsertionSort(list);
				break;
			case < 12:
				ShellSort(list, SmallArrayGaps[length-6]);
				break;
			default:
				ShellSortWithPrattSequence(list);
				break;
		}
	}

	// Implements Ex 2.1.14 in Sedgewick
	//This seems to be a  version of gnome sort
	public static void DequeueSortWithDeque<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
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
	public static void DequeueSortWithQueue<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
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
	public static void DequeueSortWithDeque<T>(IDeque<T> deque) where T : IComparable<T>
	{
		ClearCounter();
		
		#if WHITEBOXTESTING
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckBottomSortedDescending(DequeueSortHelperWithDeque<T> helper, int n)
			=> Debug.Assert(IsSortedDescending(helper.TopN(n).ToRandomAccessList()), nameof(CheckBottomSortedDescending));

		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopSortedDescending(DequeueSortHelperWithDeque<T> helper, int n)
			=> Debug.Assert(n == 0 || IsSortedDescending(helper.BottomN(n - 1).ToRandomAccessList()), nameof(CheckTopSortedDescending));
		
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopIsSmallerThanBottom(DequeueSortHelperWithDeque<T> helper, int bottomCount)
			=> Debug.Assert(bottomCount == 0 || helper.BottomN(bottomCount).Min().CompareTo(helper.Top) >= 0, nameof(CheckTopIsSmallerThanBottom));

		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckTopBiggerThanTop(DequeueSortHelperWithDeque<T> helper, int topCount)
			=> Debug.Assert(helper.TopN(topCount).Max().CompareTo(helper.Top) >= 0, nameof(CheckTopBiggerThanTop));
			
		[Conditional(Diagnostics.WhiteBoxTestingDefine)]
		static void CheckResultIsSorted(DequeueSortHelperWithDeque<T> helper)
			=> Debug.Assert(IsSortedAscending(helper.ToArray().ToRandomAccessList()), nameof(CheckResultIsSorted));
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
#if WHITEBOXTESTING
			CheckBottomSortedDescending(helper, i);
			CheckTopIsSmallerThanBottom(helper, count - i - 1);
			CheckTopBiggerThanTop(helper, i + 1);
#endif
			NTopToBottom(i + 1);
		
#if WHITEBOXTESTING
			CheckTopSortedDescending(helper, i + 1);
#endif
		}
		
		deque.Clear();
		
#if WHITEBOXTESTING
		CheckResultIsSorted(helper);
#endif
		// TODO: We need to copy the elements into the original deck (right?)
	}

	public static void GnomeSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
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

	public static void MergeSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
		=> MergeSort(list, default);
	
	public static void MergeSort<T>(IRandomAccessList<T> list, MergeSortConfig config) where T : IComparable<T>
	{
		ClearCounter();
		var helpList = new T[list.Count];
		
		void Sort(int start, int end)
		{
			if (end <= start + 1)
			{
				return;
			}
			
			int middle = start + (end - start) / 2;

			if (config.SmallArraySortAlgorithm == MergeSortConfig.SortAlgorithm.Insert && end - start < 12)
			{
				InsertionSort(list, start, middle);
				InsertionSort(list, start, middle);
			}
			else
			{
				Sort(start, middle);
				Sort(middle, end);

			}
			
			// list[middle] > list[middle + 1]
			if (!config.SkipMergeWhenSorted || ((middle + 1 < end) && LessAt(list, middle + 1, middle))) //Improvement 2 on p. 275
			{
				if (config.UseFastMerge)
				{
					FastMerge(list, helpList, start, middle, end);
				}
				else
				{
					Merge(list, helpList, start, middle, end);
				}
			}
		}
		
		Sort(0, list.Count);
	}
	
	//P. 278
	public static void MergeSortBottomUp<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearCounter();
		
		var helpList = new T[list.Count];
		
		Sort();

		void Sort()
		{
			int length = list.Count;

			for (int leftListSize = 1; leftListSize < length; leftListSize += leftListSize)
			{
				int mergedListSize = leftListSize + leftListSize;
				
				for (int leftListStart = 0; leftListStart < length - leftListSize; leftListStart += mergedListSize)
				{
					int rightListStart = leftListStart + leftListSize;
					int rightListEnd = Math.Min(leftListStart + mergedListSize, length);
					
					//Note: The indices are modified since I changed how the parameters of Merge are interpreted.
					Merge(list, helpList, leftListStart, rightListStart, rightListEnd); 
				}
			}
		}
	}

	public static bool IsSortedAscending<T>(IRandomAccessList<T> array) where T : IComparable<T> 
		=> IsSortedAscending(array, 0, array.Count);
	
	private static bool IsSortedAscending<T>(IRandomAccessList<T> array, int start, int end) where T : IComparable<T>
	{
		for (int i = start + 1; i < end; i++)
		{
			if (array[i].CompareTo(array[i - 1]) < 0)
			{
				return false;
			}
		}

		return true;
	}

	public static bool IsSortedDescending<T>(IRandomAccessList<T> array) where T : IComparable<T> 
		=> IsSortedDescending(array, 0, array.Count);

	
	public static bool IsSortedDescending<T>(IRandomAccessList<T> array, int start, int end) where T : IComparable<T>
	{
		for (int i = start + 1; i < end; i++)
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
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool Less<T>(T v, T w) where T : IComparable<T>
	{
		AddCompareTo();
		return v.CompareTo(w) < 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessAt<T>(T[] list, int i, int j) where T : IComparable<T>
	{
		AddCompareTo();
		return Less(list[i], list[j]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable<T>
	{
		AddCompareTo();
		return Less(list[i], list[j]);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessOrEqual<T>(T v, T w) where T : IComparable<T>
	{
		AddCompareTo();
		return v.CompareTo(w) <= 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool LessOrEqualAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable<T>
	{
		AddCompareTo();
		return LessOrEqual(list[i], list[j]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void SwapAt<T>(IRandomAccessList<T> list, int i, int j) where T : IComparable<T>
	{
		AddSwap();
		(list[i], list[j]) = (list[j], list[i]);
	}

	// p.271
	//I made changes to the end and mid points, so that this is roughly equivalent to their (list, start, middle, end + 1)
	//rightStartIndex is also the leftEndIndex;
	private static void Merge<T>(
		IRandomAccessList<T> list, 
		T[] helpList, 
		int leftStartIndex, 
		int rightStartIndex, 
		int rightEndIndex) where T : IComparable<T>
	{
		for (int k = leftStartIndex; k < rightEndIndex; k++)//this is <= in original
		{
			helpList[k] = list[k];
		}

		int leftIndex = leftStartIndex;
		int rightIndex = rightStartIndex; //this is middle + 1 in original
		
		for (int k = leftStartIndex; k < rightEndIndex; k++) //this is <= in original
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void TakeAt(ref int index)
			{
				list[k] = helpList[index];
				index++;
			}
			
			if (leftIndex >= rightStartIndex) //This is > in the original
			{
				TakeAt(ref rightIndex);
			}
			else if (rightIndex >= rightEndIndex)
			{
				TakeAt(ref leftIndex);
			}
			else if (LessAt(helpList, rightIndex, leftIndex))
			{
				TakeAt(ref rightIndex);
			}
			else
			{
				TakeAt(ref leftIndex);
			}
		}
	}
	
	// Ex 2.2.10
	private static void FastMerge<T>(
		IRandomAccessList<T> list, 
		T[] helpList, 
		int leftStartIndex, 
		int rightStartIndex, 
		int rightEndIndex) where T : IComparable<T>
	{
		for (int k = leftStartIndex; k < rightStartIndex; k++)
		{
			helpList[k] = list[k];
		}

		int lastIndex = rightEndIndex - 1;
		
		for (int k = rightStartIndex; k < rightEndIndex; k++)
		{
			int offsetFromStart = k - rightStartIndex;
			helpList[k] = list[lastIndex - offsetFromStart];
		}

		int leftIndex = leftStartIndex;
		int rightIndex = lastIndex;
		
		for (int k = leftStartIndex; k < rightEndIndex; k++) 
		{
			if (LessAt(helpList, leftIndex, rightIndex))
			{
				list[k] = helpList[leftIndex];
				leftIndex++;
			}
			else
			{
				list[k] = helpList[rightIndex];
				rightIndex--;
			}
		}
	}
	
	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void WriteCounts()
	{
#if WHITEBOXTESTING
		Console.WriteLine(Counter.Counts.Pretty());
#endif
	}
	
	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	private static void AddCompareTo()
	{
#if WHITEBOXTESTING
		Counter.Add("CompareTo");
#endif
	}
	
	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	private static void AddSwap()
	{
#if WHITEBOXTESTING
		Counter.Add("Swap");
#endif
	}
	
	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	private static void ClearCounter()
	{
#if WHITEBOXTESTING
		Counter.Clear();
#endif
	}
}
