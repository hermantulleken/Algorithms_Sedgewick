using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Algorithms_Sedgewick.List;
using Support;

namespace Algorithms_Sedgewick.Sort;

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
			SmallArraySize = 8,
			UseFastMerge = true
		};
		
		public enum SortAlgorithm
		{
			Small,
			Insert,
			Shell,
			Merge
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

	private sealed class DequeSortHelperWithQueue<T> : IEnumerable<T> where T : IComparable<T>
	{
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
		private readonly IQueue<T> queue;

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

		public IEnumerator<T> GetEnumerator()
		{
			yield return Peek1;
			
			foreach (var item in queue)
			{
				yield return item;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Sort()
		{
			for (int i = 0; i < Count; i++)
			{
				int m = Count - i;
				RotateLargest(m);
				Rotate(i);
			}
		}

		public override string ToString() => $"{Peek1} {queue.ToString()}";

		private void DequeueToRight() => Peek1 = queue.Dequeue();

		private void Enqueue(T item) => queue.Enqueue(item);

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

	private sealed class DequeueSortHelperWithDeque<T> : IEnumerable<T>
	{
		public T Top => deque.PeekRight;
		private readonly IDeque<T> deque;

		public DequeueSortHelperWithDeque(IDeque<T> deque)
		{
			this.deque = deque;
		}

		public IEnumerator<T> GetEnumerator() => deque.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void ExchangeTop()
		{
			var card1 = deque.PopRight();
			var card2 = deque.PopRight();
			
			deque.PushRight(card1);
			deque.PushRight(card2);
		}

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

#if WHITEBOXTESTING
		public T[] ToArray() => deque.ToArray();
		public T[] ToReverseArray() => deque.Reverse().ToArray(); //We reverse the list so the top is at 0
		public T[] TopN(int n) => ToReverseArray().Take(n).ToArray();
		public T[] BottomN(int n) => ToReverseArray().TakeLast(n).ToArray();
#endif
	}

#if WHITEBOXTESTING
	public static readonly Counter<string> Counter = new();
	public static readonly HashSet<string> Events = new();
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

	public static bool AreElementsEqual<T>(IRandomAccessList<T> array1, IRandomAccessList<T> array2, int start, int end) where T : IComparable<T>
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
			if (array1[i].CompareTo(array2[i]) != 0) 
			{
				return false;
			}
		}

		return true;
	}

	// Implements Ex 2.1.14 in Sedgewick
	//This seems to be a  version of gnome sort
	public static void DequeueSortWithDeque<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
		
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
		Debug.Assert(countBefore == list.Count);
		
		DequeueSortWithDeque(deque);

		int countAfter = deque.Count;
		
		Debug.Assert(countBefore == countAfter);

		
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = deque.PopLeft();
		}
	}

	// Implements Ex 2.1.14 in Sedgewick
	// This seems to be a  version of gnome sort
	public static void DequeueSortWithDeque<T>(IDeque<T> deque) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
		
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
		static void CheckIsSorted(IEnumerable<T> helper)
			=> Debug.Assert(IsSortedAscending(helper.ToArray().ToRandomAccessList()), nameof(CheckIsSorted));
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
		
#if WHITEBOXTESTING
		CheckIsSorted(helper);
		CheckIsSorted(deque);
#endif
	}

	// Implements Ex 2.1.14 in Sedgewick
	//This seems to be a  version of gnome sort
	public static void DequeueSortWithQueue<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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
			list[i] = (item);
			i++;
		}
	}

	public static void GnomeSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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

	public static void InsertionSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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

	public static bool IsSortedAscending<T>(IRandomAccessList<T> array) where T : IComparable<T> 
		=> IsSortedAscending(array, 0, array.Count);

	public static bool IsSortedAscending<T>(IRandomAccessList<T> array, int start, int end) where T : IComparable<T>
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

	// Ex. 2.2.14
	public static void Merge<T>(IQueue<T> leftQueue, IQueue<T> rightQueue, IQueue<T> result) where T : IComparable<T>
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
	public static void Merge3Sort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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
		int[] indexes) where T : IComparable<T>
	{
		Debug.Assert(startIndices.Length == indexes.Length + 1);

		for (int k = startIndices[0]; k < startIndices[^1]; k++)
		{
			helpList[k] = list[k];
		}

		for (int i = 0; i < indexes.Length; i++)
		{
			indexes[i] = startIndices[i];
		}//last one is not copied - it is in fact the end

		for (int k = startIndices[0]; k < startIndices[^1]; k++)
		{
			int smallestElementIndex = -1;
				
			//Find the first non-edmpty list
			for (int i = 0; i < indexes.Length; i++)
			{
				if (indexes[i] < startIndices[i + 1])
				{
					smallestElementIndex = i;
					break;
				}
			}

			if (smallestElementIndex != 0)
			{
				Events.Add("smallestElementIndex is not always 0");
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
	public static void MergeKSort<T>(IRandomAccessList<T> list, int k = 3) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();

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

	public static void MergeKSortBottomUp<T>(IRandomAccessList<T> list, int k) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
		
		var helpList = new T[list.Count];
		var startIndexes = new int[k + 1];
		var indexes = new int[k];
		
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

	public static void MergeSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
		=> MergeSort(list, default);

	public static void MergeSort<T>(IRandomAccessList<T> list, MergeSortConfig config) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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
	public static void MergeSortBottomsUpWithQueues<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		if (list.Count <= 1)
		{
			return;
		}
		
		var majorQueue = new QueueWithLinkedList<QueueWithLinkedList<T>>();
		
		for (int i = 0; i < list.Count - 1; i += 2)
		{
			var item1 = list[i];
			var item2 = list[i+1];
			var minorQueue = new QueueWithLinkedList<T>();
			
			if(Less(item1, item2))
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
			var minorQueue = new QueueWithLinkedList<T>();
			minorQueue.Enqueue(list[^1]);
			majorQueue.Enqueue(minorQueue);
		}

		var sortedQueue = new QueueWithLinkedList<T>();
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

	public static void MergeSortBottomUp<T>(IRandomAccessList<T> list) where T : IComparable<T>
		=> MergeSortBottomUp(list, MergeSortConfig.Optimized);

	// P. 278
	public static void MergeSortBottomUp<T>(IRandomAccessList<T> list, MergeSortConfig config) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
		
		var helpList = new T[list.Count];
		
		Sort();

		void Sort()
		{
			int length = list.Count;
			int mergeStartSize = 1;
			
			if (config.SmallArraySortAlgorithm == MergeSortConfig.SortAlgorithm.Insert)
			{
				mergeStartSize = config.SmallArraySize;
			
				for (int leftListStart = 0; leftListStart < length; leftListStart += mergeStartSize)
				{
					int leftListEnd = Math.Min(leftListStart + mergeStartSize, length);
					InsertionSort(list, leftListStart, leftListEnd);
				}
			}

			//mergedListSize2 is 1 if no algo is used instead of Merge for small lists
			for (int leftListSize = mergeStartSize; leftListSize < length; leftListSize += leftListSize)
			{
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
						//Note: The indices are modified since I changed how the parameters of Merge are interpreted.
						Merge(list, helpList, leftListStart, rightListStart, rightListEnd);
					}
				}
			}
		}
	}

	public static void MergeSortNatural<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		var helpList = new T[list.Count];

		int FindEndIndexOfSorted(int start)
		{
			for (int i = start + 1; i < list.Count; i++)
			{
				if (LessAt(list, i, i - 1))
				{
					return i;
				}
			}

			return list.Count;
		}

		int leftStart = 0;
		int leftEnd = FindEndIndexOfSorted(leftStart);

		if (leftEnd == list.Count)
		{
			return;
		}

		int rightEnd = FindEndIndexOfSorted(leftEnd);
		Merge(list, helpList, leftStart, leftEnd, rightEnd);
	}

	public static void SelectionSort<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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

	public static void ShellSort<T>(IRandomAccessList<T> list, int[] stepSizes) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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

	public static void ShellSortWithPrattSequence<T>(IRandomAccessList<T> list) where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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

	public static void SortSmall<T>(IRandomAccessList<T> list)where T : IComparable<T>
	{
		ClearWhiteBoxContainers();
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

	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void WriteCounts()
	{
#if WHITEBOXTESTING
		Console.WriteLine(Counter.Counts.Pretty());
#endif
	}
	
	[Conditional(Diagnostics.WhiteBoxTestingDefine)]
	public static void WriteEvents()
	{
#if WHITEBOXTESTING
		Console.WriteLine(Events.Pretty());
#endif
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
	private static void ClearWhiteBoxContainers()
	{
#if WHITEBOXTESTING
		Counter.Clear();
		Events.Clear();
#endif
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


	// Ex. 2.2.22
	private static void Merge3<T>(
		IRandomAccessList<T> list,
		T[] helpList,
		int list0Start,
		int list1Start,
		int list2Start,
		int list2End) where T : IComparable<T>
	{
		for (int k = list0Start; k < list2End; k++)//this is <= in original
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
			//index0..end0 and index1..end1 represents two sublists,
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
			
			if (list0Index >= list1Start) //list 0 is empty
			{
				TakesSmallestAt(ref list1Index, list2Start, ref list2Index, list2End);
			}
			else if (list1Index >= list2Start) //list 1 is empty
			{
				TakesSmallestAt(ref list0Index, list1Start, ref list2Index, list2End);
			}
			else if(list2Index >= list2End) //list 2 is empty
			{
				TakesSmallestAt(ref list0Index, list1Start, ref list1Index, list2Start);
			}
			else if (LessAt(helpList, list0Index, list1Index) && LessAt(helpList, list0Index, list2Index))
			{ // 0 1 2 or 0 2 1
				TakeAt(ref list0Index);
			}
			else if (LessAt(helpList, list1Index, list2Index))
			{ //1 0 2 or 1 2 0
				TakeAt(ref list1Index);
			}
			else 
			{ //2 0 1 or 2 1 0
				TakeAt(ref list2Index);
			}
		}
	}
}
