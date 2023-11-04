namespace Algorithms_Sedgewick.Sort;

using System.Collections;
using System.Runtime.CompilerServices;

using Deque;
using List;
using Pool;
using Queue;
using Support;

#if WITH_INSTRUMENTATION
using System.Diagnostics;
#endif

using static System.Diagnostics.Debug;
using static List.ListExtensions;
using static Support.WhiteBoxTesting;

public static class Sort
{
	public struct MergeSortConfig
	{
		public static MergeSortConfig Vanilla => new()
		{
			SkipMergeWhenSorted = false,
			SmallArraySortAlgorithm = SortAlgorithm.Merge,
			SmallArraySize = 0,
			UseFastMerge = false,
		};
		
		public static MergeSortConfig Optimized => new()
		{
			SkipMergeWhenSorted = true,
			SmallArraySortAlgorithm = SortAlgorithm.Insert,
			SmallArraySize = 8,
			UseFastMerge = true,
		};
		
		public enum SortAlgorithm
		{
			Small,
			Insert,
			Shell,
			Merge,
		};
		
		public bool SkipMergeWhenSorted;
		public SortAlgorithm SmallArraySortAlgorithm;
		public bool UseFastMerge;
		public int SmallArraySize;

		public MergeSortConfig()
		{
			SkipMergeWhenSorted = false;
			SmallArraySortAlgorithm = SortAlgorithm.Merge;
			SmallArraySize = 0;
			UseFastMerge = false;
		}
	}

	public struct QuickSortConfig
	{
		public enum PivotSelectionAlgorithm
		{
			First,
			MedianOfThreeFirst
		}

		public static readonly QuickSortConfig Vanilla = new()
		{
			PivotSelection = PivotSelectionAlgorithm.First
		};
		
		public PivotSelectionAlgorithm PivotSelection;
	}

	/// <summary>
	/// This data structure supports extra methods so it can be used to implement the
	/// deque sort algorithm.
	/// </summary>
	private sealed class DequeSortHelperWithQueue<T> : IEnumerable<T> 
		where T : IComparable<T>
	{
		private readonly IQueue<T> queue;

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

		private int Count => queue.Count + 1;

		private T Peek1 { get; set; }

		private T Peek2 => queue.Peek;

		public DequeSortHelperWithQueue(IQueue<T> queue)
		{
			if (queue.IsEmpty)
			{
				// We need at least one element.
				ThrowHelper.ThrowContainerEmpty();
			}
			
			this.queue = queue;
			DequeueToRight();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return Peek1;
			
			foreach (var item in queue)
			{
				yield return item;
			}
		}

		public void Sort()
		{
			for (int i = 0; i < Count; i++)
			{
				int m = Count - i;
				RotateLargest(m);
				Rotate(i);
			}
		}

		public override string ToString() => $"{Peek1} {queue}";

		private void DequeueToRight() => Peek1 = queue.Dequeue();

		private void Enqueue(T item) => queue.Enqueue(item);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

		private void RotateLargest(int n)
		{
			for (int i = 0; i < n; i++)
			{
				RotateSmallest();
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
	}

	/// <summary>
	/// This data structure supports extra methods so it can be used to implement the
	/// deque sort algorithm.
	/// </summary>
	private sealed class DequeueSortHelperWithDeque<T> : IEnumerable<T>
	{
		private readonly IDeque<T> deque;

		public T Top => deque.PeekRight;

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

		public IEnumerator<T> GetEnumerator() => deque.GetEnumerator();

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

		public override string ToString() => deque.ToString();
		
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

#if WITH_INSTRUMENTATION
		public T[] ToArray() => deque.ToArray();
		
		public T[] ToReverseArray() => deque.Reverse().ToArray(); // We reverse the list so the top is at 0
		
		public T[] TopN(int n) => ToReverseArray().Take(n).ToArray();
		
		public T[] BottomN(int n) => ToReverseArray().TakeLast(n).ToArray();
#endif
	}

	private struct LabeledItem<T> : IComparable<LabeledItem<T>> 
		where T : IComparable<T>
	{
		public T Item;
		public int Label;

		public int CompareTo(LabeledItem<T> other)
		{
			int res = Item.CompareTo(other.Item);

			return res == 0 ? Label - other.Label : res;
		}
	}

	private static readonly int[] CiuraGaps = { 1, 4, 10, 23, 57, 132, 301, 701 };

	// From: https://stackoverflow.com/a/50470237/335144
	private static readonly int[][] SmallArrayGaps =
	{
		new[] { 4, 1 }, // for 6 elements
		new[] { 5, 1 }, // 7
		new[] { 6, 1 }, // 8
		new[] { 9, 6, 1 }, // 9
		new[] { 10, 6, 1 }, // 10
		new[] { 5, 1 }, // 10
	};

	public static bool AreElementsEqual<T>(IReadonlyRandomAccessList<T> array1, IReadonlyRandomAccessList<T> array2, int start, int end) 
		where T : IComparable<T>
	{
		if (end > array1.Count)
		{
			throw new ArgumentOutOfRangeException(nameof(end));
		}
		
		if (end > array2.Count)
		{
			throw new ArgumentOutOfRangeException(nameof(end));
		}

		if (start < 0 || start > end)
		{
			throw new ArgumentOutOfRangeException(nameof(start));
		}

		for (int i = start; i < end; i++)
		{
			__AddCompareTo();
			if (array1[i].CompareTo(array2[i]) != 0) 
			{
				return false;
			}
		}

		return true;
	}

	// Implements Ex 2.1.14 in Sedgewick
	// This seems to be a  version of gnome sort
	public static void DequeueSortWithDeque<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
		}
		
		var deque = new DequeWithDoublyLinkedList<T>();

		foreach (var item in list)
		{
			deque.PushRight(item);
		}
		
		int countBefore = deque.Count;
		Assert(countBefore == list.Count);
		
		DequeueSortWithDeque(deque);

		int countAfter = deque.Count;
		
		Assert(countBefore == countAfter);

		
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = deque.PopLeft();
		}
	}

	// Implements Ex 2.1.14 in Sedgewick
	// This seems to be a  version of gnome sort
	public static void DequeueSortWithDeque<T>(IDeque<T> deque) 
		where T : IComparable<T>
	{
		#if WITH_INSTRUMENTATION
		[Conditional(Diagnostics.WithInstrumentationDefine)]
		static void CheckBottomSortedDescending(DequeueSortHelperWithDeque<T> helper, int n)
			=> Assert(IsSortedDescending(helper.TopN(n).ToRandomAccessList()), nameof(CheckBottomSortedDescending));

		[Conditional(Diagnostics.WithInstrumentationDefine)]
		static void CheckTopSortedDescending(DequeueSortHelperWithDeque<T> helper, int n)
			=> Assert(n == 0 || IsSortedDescending(helper.BottomN(n - 1).ToRandomAccessList()), nameof(CheckTopSortedDescending));
		
		[Conditional(Diagnostics.WithInstrumentationDefine)]
		static void CheckTopIsSmallerThanBottom(DequeueSortHelperWithDeque<T> helper, int bottomCount)
			=> Assert(bottomCount == 0 || helper.BottomN(bottomCount).Min().CompareTo(helper.Top) >= 0, nameof(CheckTopIsSmallerThanBottom));

		[Conditional(Diagnostics.WithInstrumentationDefine)]
		static void CheckTopBiggerThanTop(DequeueSortHelperWithDeque<T> helper, int topCount)
			=> Assert(helper.TopN(topCount).Max().CompareTo(helper.Top) >= 0, nameof(CheckTopBiggerThanTop));
			
		[Conditional(Diagnostics.WithInstrumentationDefine)]
		static void CheckIsSorted(IEnumerable<T> helper)
			=> Assert(IsSortedAscending(helper.ToArray().ToRandomAccessList()), nameof(CheckIsSorted));
		#endif

		int count = deque.Count;
		var helper = new DequeueSortHelperWithDeque<T>(deque);
		
		void GetNthSmallestOnTop(int n)
		{
			int stepCount = count - n - 1;
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
#if WITH_INSTRUMENTATION
			CheckBottomSortedDescending(helper, i);
			CheckTopIsSmallerThanBottom(helper, count - i - 1);
			CheckTopBiggerThanTop(helper, i + 1);
#endif
			NTopToBottom(i + 1);
		
#if WITH_INSTRUMENTATION
			CheckTopSortedDescending(helper, i + 1);
#endif
		}
		
#if WITH_INSTRUMENTATION
		CheckIsSorted(helper);
		CheckIsSorted(deque);
#endif
	}

	// Implements Ex 2.1.14 in Sedgewick
	// This seems to be a  version of gnome sort
	public static void DequeueSortWithQueue<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
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
			list[i] = item;
			i++;
		}
	}

	// From https://en.wikipedia.org/wiki/Gnome_sort
	public static void GnomeSort<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		int i = 0;
		while (i < list.Count)
		{
			if (i == 0 || LessOrEqualAt(list, i - 1, i))
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

	public static void HeapSort<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
		}
		
		int GetChildIndex(int index) => 2 * index + 1;
		// int GetParentIndex(int index) => (index - 1) / 2;
		
		void Sink(int k, int count)
		{
			Assert(list.Count != 1);

			int child = GetChildIndex(k);
			
			while (child < count)
			{
				if (child < count && LessAt(list, child, child+1))
				{
					child++;
				}

				if (!LessAt(list, k, child))
				{
					break;
				}
			
				SwapAt(list, k, child);
				k = child;
				child = GetChildIndex(child);
			}
		}

		int count = list.Count;
		Console.WriteLine(list.Pretty());
		
		for (int k = (count - 1) / 2; k >= 0; k--)
		{
			Sink(k, count);
			Console.WriteLine(list.Pretty());
		}

		int i = count-1;
		
		while (i >= 1)
		{
			SwapAt(list, 0, i);
			i--;
			Sink(0, i);
			Console.WriteLine(list.Pretty());
		}
	}

	public static void InsertionSort<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		InsertionSort(list, 0, list.Count);
	}

	public static void InsertionSort<T>(IRandomAccessList<T> list, int start, int end) 
		where T : IComparable<T>
	{
		for (int i = start + 1; i < end; i++)
		{ 
			// Insert a[i] among a[i-1], a[i-2], a[i-3]... ..
			for (int j = i; j > start && LessAt(list, j, j - 1); j--)
			{
				SwapAt(list, j, j - 1);
			}
		}
	}

	/// <summary>
	/// Checks whether a list is sorted.
	/// </summary>
	/// <param name="list">A list of elements to check. Cannot be null.</param>
	/// <param name="comparer">A comparer to use to compare elements. If not supplied
	/// or null, <see cref="Comparer{T}.Default"/> is used.</param>
	/// <returns><see langword="true"/> the list is sorted, <see langword="false"/> otherwise.</returns>
	public static bool IsSorted<T>(IReadonlyRandomAccessList<T> list, IComparer<T>? comparer = null)
	{
		list.ThrowIfNull();
		
		comparer ??= Comparer<T>.Default;
		
		if (list.Count <= 1)
		{
			return true;
		}
		
		// We have at least two elements
		Assert(list.Count >= 2);
		
		for (int i = 1; i < list.Count; i++)
		{
			// Negative indexes are impossible
			Assert(i - 1 >= 0);
			
			var item0 = list[i - 1];
			var item1 = list[i];
			
			if (comparer.Compare(item0, item1) > 0)
			{
				return false;
			}
			
			// All items up to i are sorted
		}
		
		// All items up to the last index are sorted
		return true;
	}

	public static bool IsSortedAscending<T>(IReadonlyRandomAccessList<T> array) 
	where T : IComparable<T> 
		=> IsSortedAscending(array, 0, array.Count);

	public static bool IsSortedAscending<T>(IReadonlyRandomAccessList<T> array, int start, int end) 
		where T : IComparable<T>
	{
		for (int i = start + 1; i < end; i++)
		{
			if (LessAt(array, i, i - 1))
			{
				return false;
			}
		}

		return true;
	}

	public static bool IsSortedDescending<T>(IReadonlyRandomAccessList<T> array) 
		where T : IComparable<T> 
		=> IsSortedDescending(array, 0, array.Count);


	public static bool IsSortedDescending<T>(IReadonlyRandomAccessList<T> array, int start, int end) 
		where T : IComparable<T>
	{
		for (int i = start + 1; i < end; i++)
		{
			if (LessAt(array, i - 1, i))
			{
				return false;
			}
		}

		return true;
	}

	public static T MedianOf5<T>(T a, T b, T c, T d, T e) 
		where T : IComparable<T>
	{
		if (Less(b, a))
		{
			(a, b) = (b, a);
		}

		if (Less(d, c))
		{
			(c, d) = (d, c);
		}
		
		if (Less(c, a))
		{
			(a, c) = (c, a);
			(b, d) = (d, b);
		}

		if (Less(e, b))
		{
			(b, e) = (e, b);
		}
		
		if (Less(b, a))
		{
			b = a; // (a, b) = (b, a);
			c = d; // (c, d) = (d, c);
		}

		if (Less(e, c))
		{
			c = e; // (c, e) = (e, c);
		}
		
		return Less(b, c) ? c : b;
	}

	/// <summary>
	/// Merge two sorted queues into a result queue.
	/// </summary>
	// Ex. 2.2.14
	public static void Merge<T>(IQueue<T> leftQueue, IQueue<T> rightQueue, IQueue<T> result) 
		where T : IComparable<T>
	{
		void TakeRight() => result.Enqueue(rightQueue.Dequeue());
		void TakeLeft() => result.Enqueue(leftQueue.Dequeue());

		while (!leftQueue.IsEmpty || !rightQueue.IsEmpty)
		{
			if (leftQueue.IsEmpty)
			{
				TakeRight();
			}
			else if (rightQueue.IsEmpty)
			{
				TakeLeft();
			}
			else if (Less(leftQueue.Peek, rightQueue.Peek))
			{
				TakeLeft();
			}
			else
			{
				TakeRight();
			}
		}
	}

	// Ex. 2.2.22
	public static void Merge3Sort<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		var helpList = new T[list.Count];
		
		void Sort(int start, int end)
		{
			if (end <= start + 1)
			{
				return;
			}

			int subListLength = (end - start + 2) / 3;
			int end0 = start + subListLength;
			int end1 = end0 + subListLength;
			
			Sort(start, end0);
			Sort(end0, end1);
			Sort(end1, end);
			
			Merge3(list, helpList, start, end0, end1, end);
		}
		
		Sort(0, list.Count);
	}

	// Ex. 2.25
	public static void MergeK<T>(
		IRandomAccessList<T> list,
		T[] helpList,
		int[] startIndices,
		int[] indexes) 
		where T : IComparable<T>
	{
		Assert(startIndices.Length == indexes.Length + 1);

		for (int k = startIndices[0]; k < startIndices[^1]; k++)
		{
			helpList[k] = list[k];
		}

		for (int i = 0; i < indexes.Length; i++)
		{
			indexes[i] = startIndices[i];
		}// last one is not copied - it is in fact the end

		for (int k = startIndices[0]; k < startIndices[^1]; k++)
		{
			int smallestElementIndex = -1;
				
			// Find the first non-edmpty list
			for (int i = 0; i < indexes.Length; i++)
			{
				if (indexes[i] < startIndices[i + 1])
				{
					smallestElementIndex = i;
					break;
				}
			}

			// Find the smallest element from the lists
			for (int i = smallestElementIndex + 1; i < indexes.Length; i++)
			{
				if (indexes[i] >= startIndices[i + 1]) continue;

				if (LessAt(helpList, indexes[i], indexes[smallestElementIndex]))
				{
					smallestElementIndex = i;
				}
			}

			list[k] = helpList[indexes[smallestElementIndex]];
			indexes[smallestElementIndex]++;
		}
	}

	// Ex. 2.2.22
	public static void MergeKSort<T>(IRandomAccessList<T> list, int k = 3) 
		where T : IComparable<T>
	{
		if (k <= 1)
		{
			throw new ArgumentException(null, nameof(k));
		}
		
		var helpList = new T[list.Count];

		void Sort(int start, int end)
		{
			if (end <= start + 1)
			{
				return;
			}
			
			int[] startIndexes = new int[k + 1];
			int[] indexes = new int[k];
			
			startIndexes[0] = start;
			startIndexes[^1] = end;
			
			int subListLength = (end - start + k - 1) / k;
			
			for (int i = 1; i < k; i++)
			{
				startIndexes[i] = Math.Min(startIndexes[i - 1] + subListLength, end);
			}

			for (int i = 1; i < k + 1; i++)
			{
				Sort(startIndexes[i - 1], startIndexes[i]);
			}
			
			MergeK(list, helpList, startIndexes, indexes);
		}
		
		Sort(0, list.Count);
	}

	public static void MergeKSortBottomUp<T>(IRandomAccessList<T> list, int k) 
		where T : IComparable<T>
	{
		var helpList = new T[list.Count];
		int[] startIndexes = new int[k + 1];
		int[] indexes = new int[k];
		
		Sort();

		void Sort()
		{
			int length = list.Count;

			for (int leftListSize = 1; leftListSize < length; leftListSize *= k)
			{
				int mergedListSize = leftListSize * k;
				
				for (int leftListStart = 0; leftListStart < length; leftListStart += mergedListSize)
				{
					int start = leftListStart;
					int end = Math.Min(leftListStart + mergedListSize, length);

					startIndexes[0] = start;
					startIndexes[^1] = end;

					for (int i = 1; i < k; i++)
					{
						startIndexes[i] = Math.Min(startIndexes[i - 1] + leftListSize, end);
					}
					
					MergeK(list, helpList, startIndexes, indexes);
				}
			}
		}
	}

	public static void MergeSort<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
		=> MergeSort(list, default);

	public static void MergeSort<T>(IRandomAccessList<T> list, MergeSortConfig config) 
		where T : IComparable<T>
	{
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
				InsertionSort(list, start, end);
				return;
			}

			Sort(start, middle);
			Sort(middle, end);

			// list[middle] > list[middle + 1]
			if (config.SkipMergeWhenSorted && LessOrEqualAt(list, middle - 1, middle)) return;
			
			if (config.UseFastMerge)
			{
				FastMerge(list, helpList, start, middle, end);
			}
			else
			{
				Merge(list, helpList, start, middle, end);
			}
		}
		
		Sort(0, list.Count);
	}

	// Ex. 2.2.15
	public static void MergeSortBottomsUpWithQueues<T>(
		IRandomAccessList<T> list,
		IFactory<IQueue<IQueue<T>>>? majorQueueFactory = null, 
		IFactory<IQueue<T>>? minorQueueFactory = null) 
		where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
		}
		majorQueueFactory ??= Factory.Create<IQueue<IQueue<T>>>(() => new QueueWithLinkedList<IQueue<T>>());
		minorQueueFactory ??= Factory.Create<IQueue<T>>(() => new QueueWithLinkedList<T>());
		
		var majorQueue = majorQueueFactory.GetNewInstance();
		
		for (int i = 0; i < list.Count - 1; i += 2)
		{
			var item1 = list[i];
			var item2 = list[i + 1];
			var minorQueue = minorQueueFactory.GetNewInstance();
			
			if (Less(item1, item2))
			{
				minorQueue.Enqueue(item1);
				minorQueue.Enqueue(item2);
			}
			else
			{
				minorQueue.Enqueue(item2);
				minorQueue.Enqueue(item1);
			}
			
			majorQueue.Enqueue(minorQueue);
		}

		if (list.Count % 2 == 1)
		{
			var minorQueue = minorQueueFactory.GetNewInstance();
			minorQueue.Enqueue(list[^1]);
			majorQueue.Enqueue(minorQueue);
		}

		var sortedQueue = minorQueueFactory.GetNewInstance();
		while (majorQueue.Count > 1)
		{
			var leftQueue = majorQueue.Dequeue();
			var rightQueue = majorQueue.Dequeue();
			
			Merge(leftQueue, rightQueue, sortedQueue);
			majorQueue.Enqueue(sortedQueue);
			
			leftQueue.Clear();
			sortedQueue = leftQueue;
		}

		var result = majorQueue.Dequeue();

		for (int i = 0; i < list.Count; i++)
		{
			list[i] = result.Dequeue();
		}
	}
	
	public static void MergeSortBottomsUpWithQueues<T>(
		IRandomAccessList<T> list, 
		FixedPreInitializedPool<IQueue<IQueue<T>>> majorQueues,
		FixedPreInitializedPool<IQueue<T>> minorQueues) 
		where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
		}
		
		var majorQueue = majorQueues.Get();
		
		for (int i = 0; i < list.Count - 1; i += 2)
		{
			var item1 = list[i];
			var item2 = list[i+1];
			var minorQueue = minorQueues.Get();
			
			if (Less(item1, item2))
			{
				minorQueue.Enqueue(item1);
				minorQueue.Enqueue(item2);
			}
			else
			{
				minorQueue.Enqueue(item2);
				minorQueue.Enqueue(item1);
			}
			
			majorQueue.Enqueue(minorQueue);
		}

		if (list.Count % 2 == 1)
		{
			var minorQueue = minorQueues.Get();
			minorQueue.Enqueue(list[^1]);
			majorQueue.Enqueue(minorQueue);
		}
		
		while (majorQueue.Count > 1)
		{
			var sortedQueue = minorQueues.Get();
			var leftQueue = majorQueue.Dequeue();
			var rightQueue = majorQueue.Dequeue();
			
			Merge(leftQueue, rightQueue, sortedQueue);
			majorQueue.Enqueue(sortedQueue);
			
			minorQueues.Return(leftQueue);
			minorQueues.Return(rightQueue);
		}

		var result = majorQueue.Dequeue();

		for (int i = 0; i < list.Count; i++)
		{
			list[i] = result.Dequeue();
		}
		
		minorQueues.Return(result);
		majorQueues.Return(majorQueue);
	}

	public static void MergeSortBottomUp<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
		=> MergeSortBottomUp(list, MergeSortConfig.Optimized);

	// P. 278
	public static void MergeSortBottomUp<T>(IRandomAccessList<T> list, MergeSortConfig config) 
		where T : IComparable<T>
	{
		var helpList = new T[list.Count];
		
		Sort();

		void Sort()
		{
			int length = list.Count;
			int mergeStartSize = 1;
			
			if (config.SmallArraySortAlgorithm == MergeSortConfig.SortAlgorithm.Insert)
			{
				__AddPass();
				mergeStartSize = config.SmallArraySize;
			
				for (int leftListStart = 0; leftListStart < length; leftListStart += mergeStartSize)
				{
					int leftListEnd = Math.Min(leftListStart + mergeStartSize, length);
					InsertionSort(list, leftListStart, leftListEnd);
				}
			}

			// mergedListSize2 is 1 if no algo is used instead of Merge for small lists
			for (int leftListSize = mergeStartSize; leftListSize < length; leftListSize += leftListSize)
			{
				__AddPass();
				
				int mergedListSize = leftListSize + leftListSize;
				
				for (int leftListStart = 0; leftListStart < length - leftListSize; leftListStart += mergedListSize)
				{
					int rightListStart = leftListStart + leftListSize;
					int rightListEnd = Math.Min(leftListStart + mergedListSize, length);

					if (config.SkipMergeWhenSorted && LessOrEqualAt(list, rightListStart - 1, rightListStart)) continue;
					
					if (config.UseFastMerge)
					{
						FastMerge(list, helpList, leftListStart, rightListStart, rightListEnd);
					}
					else
					{
						// Note: The indices are modified since I changed how the parameters of Merge are interpreted.
						Merge(list, helpList, leftListStart, rightListStart, rightListEnd);
					}
				}
			}
		}
	}

	// Ex. 2.2.16
	public static void MergeSortNatural<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		var helpList = new T[list.Count];
		
		Sort();

		void Sort()
		{
			int length = list.Count;

			if (length <= 1)
			{
				return;
			}

			while (true)
			{
				int start = 0;

				__AddPass();

				while (start < length)
				{
					int mid = start + 1;

					while (mid < length && LessOrEqualAt(list, mid - 1, mid))
					{
						mid++;
					}

					if (mid == length)
					{
						if (start == 0)
						{
							return;
						}

						break;
					}
				
					int end = mid + 1;
				
					while (end < length && LessOrEqualAt(list, end-1, end))
					{
						end++;
					}
				
					Merge(list, helpList, start, mid, end);

					if (start == 0 && end == length)
					{
						return;
					}
					
					start = end;
				}
			}
		}
	}

	// Note: This changes the order of the elements in the array.
	public static void MoveNLowestToLeft<T>(IRandomAccessList<T> list, int n, QuickSortConfig config) 
		where T : IComparable<T>
	{
		list.ThrowIfNull();
		n.ThrowIfOutOfRange(list.Count);
		
		list.Shuffle();
		
		int start = 0;
		int end = list.Count - 1;
		
		while (end > start)
		{
			int j = Partition(list, start, end, config);
			
			if (j == n)
			{
				return;
			}

			if (j > n)
			{
				end = j - 1;
			}
			else
			{
				Assert(j < n);
				start = j + 1;
			}
		}
	}

	public static int Partition<T>(IRandomAccessList<T> list, int start, int end, QuickSortConfig config) 
		where T : IComparable<T>
	{
		int i = start;
		int j = end + 1;

		int partitionIndex = config.PivotSelection switch
		{
			QuickSortConfig.PivotSelectionAlgorithm.MedianOfThreeFirst when end - start >= 2 
				=> MedianOfThree(list, start, start + 1, start + 2),
			QuickSortConfig.PivotSelectionAlgorithm.First 
				=> start,
			_
				=> start,
		};

		SwapAt(list, start, partitionIndex);
		var partitioningElement = list[start];
		
		while (true)
		{
			while (Less(list[++i], partitioningElement))
			{
				if (i == end)
				{
					break;
				}
			}

			// Note: The text says this should be LessOrEqual, but they use Less in their code
			while (Less(partitioningElement, list[--j]))
			{
				if (j == start)
				{
					break;
				}
			}

			if (i >= j)
			{
				break;
			}
			
			SwapAt(list, i, j);
		}
		
		SwapAt(list, start, j);
		return j;
	}

	public static void QuickSort<T>(IRandomAccessList<T> list, QuickSortConfig config) 
		where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
		}
		
		list.Shuffle(); // Prevents worse case scenario
		Sort(0, list.Count - 1);

		void Sort(int start, int end)
		{
			if (start < end)
			{
				int median = Partition(list, start, end, config);
				Sort(start, median - 1);
				Sort(median + 1, end);
			}
		}
	}

	public static void QuickTwoKey<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		// Case 1: small element at start, large element at end 
		void Case1(int start, int end)
		{
			int leftCounter = start;
			int rightCounter = end;
			
			while (true)
			{
				while (LessOrEqualAt(list, leftCounter, start))
				{
					leftCounter++;
				}
				
				while (LessOrEqualAt(list, end, rightCounter))
				{
					rightCounter--;
				}

				if (rightCounter >= leftCounter)
				{
					break;
				}
				
				SwapAt(list, leftCounter, rightCounter);
			}
		}
		
		void Case3(int start, int end)
		{
			int leftCounter = start;
			int rightCounter = end;
			
			while (LessOrEqualAt(list, leftCounter, start))
			{
				leftCounter++;
			}
				
			while (LessOrEqualAt(list, end, rightCounter))
			{
				rightCounter--;
			}

			if (rightCounter >= leftCounter)
			{
				return; // Nothing to sort! They are all the same!
			}

			if (LessAt(list, leftCounter, end))
			{
					Case1(leftCounter, end);
			}
			else
			{
				Case1(start, rightCounter);
			}
		}

		int start = 0;
		int end = list.Count - 1;
		
		if (LessAt(list, start, end))
		{
			Case1(start, end);
			return;
		}

		// Case 2
		if (LessAt(list, end, start))
		{
			SwapAt(list, start, end);
			Case1(start, end);
			return;
		}

		Case3(start, end);
	}

	public static void SelectionSort<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
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


	// Note: This changes the order of the elements in the array.
	public static IComparable<T> SelectNthLowest<T>(IRandomAccessList<T> list, int n, QuickSortConfig config)
		where T : IComparable<T>
	{
		MoveNLowestToLeft(list, n, config);
		return list[n];
	}

	public static void ShellSort<T>(IRandomAccessList<T> list, int[] stepSizes)
		where T : IComparable<T>
	{
		int length = list.Count;

		for (int stepSizeIndex = 0; stepSizeIndex < stepSizes.Length; stepSizeIndex++)
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

	public static void ShellSortWithPrattSequence<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
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

	public static void SortSmall<T>(IRandomAccessList<T> list)
		where T : IComparable<T>
	{
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

	// Cannot do this without using custom comparator
	public static void SortStable<T>(IRandomAccessList<T> list) 
		where T : IComparable<T>
	{
		var labelledList = list
			.Select((item, i) => new LabeledItem<T>(){Item = item, Label = i})
			.ToResizableArray(list.Count);
		
		QuickSort(labelledList, QuickSortConfig.Vanilla);
		
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = labelledList[i].Item;
		}
	}
	
	// Ex 2.2.10
	private static void FastMerge<T>(
		IRandomAccessList<T> list, 
		T[] helpList, 
		int leftStartIndex, 
		int rightStartIndex, 
		int rightEndIndex) 
		where T : IComparable<T>
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

	/*
	 public static T MedianOfThree<T>(T a, T b, T c) where T : IComparable<T> =>
		a.CompareTo(b) switch
		{
			< 0 when b.CompareTo(c) < 0 => b,
			< 0 => a.CompareTo(c) < 0 ? c : a,
			_ => a.CompareTo(c) < 0 ? a : b.CompareTo(c) < 0 ? c : b
		};
	*/

	/*
		Note: We use this algorithm instead of the shorter one above so that we can count the number of comparrisons.
			Should we use if statements instead?
	 */
	private static int MedianOfThree<T>(IReadonlyRandomAccessList<T> list, int a, int b, int c) 
		where T : IComparable<T> 
		=> LessAt(list, a, b)
			? LessAt(list, b, c)
				? b
				: LessAt(list, a, c)
					? c
					: a
			: LessAt(list, c, b)
				? b
				: LessAt(list, c, a)
					? c
					: a;

	// p.271
	// I made changes to the end and mid points, so that this is roughly equivalent to their (list, start, middle, end + 1)
	// rightStartIndex is also the leftEndIndex;
	private static void Merge<T>(
		IRandomAccessList<T> list, 
		T[] helpList, 
		int leftStartIndex, 
		int rightStartIndex, 
		int rightEndIndex) 
		where T : IComparable<T>
	{
		for (int k = leftStartIndex; k < rightEndIndex; k++) // this is <= in original
		{
			helpList[k] = list[k];
		}

		int leftIndex = leftStartIndex;
		int rightIndex = rightStartIndex; // this is middle + 1 in original
		
		for (int k = leftStartIndex; k < rightEndIndex; k++) // this is <= in original
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void TakeAt(ref int index)
			{
				list[k] = helpList[index];
				index++;
			}
			
			if (leftIndex >= rightStartIndex) // This is > in the original
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


	// Ex. 2.2.22
	private static void Merge3<T>(
		IRandomAccessList<T> list,
		T[] helpList,
		int list0Start,
		int list1Start,
		int list2Start,
		int list2End)
		where T : IComparable<T>
	{
		for (int k = list0Start; k < list2End; k++) // this is <= in original
		{
			helpList[k] = list[k];
		}

		int list0Index = list0Start;
		int list1Index = list1Start;
		int list2Index = list2Start;
		
		for (int i = list0Start; i < list2End; i++)
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			void TakeAt(ref int index)
			{
				list[i] = helpList[index];
				index++;
			}
			
			// Takes the smallest element at index0 or index1, if these indices are below the given end points
			// index0..end0 and index1..end1 represents two sublists,
			void TakesSmallestAt(ref int index0, int end0, ref int index1, int end1)
			{
				if (index0 >= end0)
				{
					TakeAt(ref index1);
				}
				else if (index1 >= end1)
				{
					TakeAt(ref index0);
				}
				else if (LessAt(helpList, index0, index1))
				{
					TakeAt(ref index0);
				}
				else
				{
					TakeAt(ref index1);
				}
			}
			
			if (list0Index >= list1Start) // list 0 is empty
			{
				TakesSmallestAt(ref list1Index, list2Start, ref list2Index, list2End);
			}
			else if (list1Index >= list2Start) // list 1 is empty
			{
				TakesSmallestAt(ref list0Index, list1Start, ref list2Index, list2End);
			}
			else if (list2Index >= list2End) // list 2 is empty
			{
				TakesSmallestAt(ref list0Index, list1Start, ref list1Index, list2Start);
			}
			else if (LessAt(helpList, list0Index, list1Index) && LessAt(helpList, list0Index, list2Index))
			{ // 0 1 2 or 0 2 1
				TakeAt(ref list0Index);
			}
			else if (LessAt(helpList, list1Index, list2Index))
			{ // 1 0 2 or 1 2 0
				TakeAt(ref list1Index);
			}
			else 
			{ // 2 0 1 or 2 1 0
				TakeAt(ref list2Index);
			}
		}
	}
}
