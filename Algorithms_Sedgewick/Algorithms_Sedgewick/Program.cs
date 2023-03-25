using Support;

namespace Algorithms_Sedgewick;

using List;
using static WhiteBoxTesting;
using Timer = Support.Timer;

public static class Extensions
{
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
internal static class Program
{
	public static void Main()
	{
		TimeSearchers();
		
		// TimeSorts();
	}

	public static void TimeSearchers()
	{
		Action<int[], int[]> AndPrintWhiteBoxInfo(Action<int[], int[]> action) =>
			(list, keys) =>
			{
				action(list, keys);
				__WriteCounts();
				__ClearWhiteBoxContainers();
			};

		void InterpolationSearch(int[] list, int[] keys)
		{
			foreach (int key in keys)
			{
				list.InterpolationSearch(key);
			}
		}
		
		void BinarySearch(int[] list, int[] keys)
		{
			foreach (int key in keys)
			{
				list.BinarySearch(key);
			}
		}
		
		void CSharpBinarySearch(int[] list, int[] keys)
		{
			foreach (int key in keys)
			{
				Array.BinarySearch(list, key);
			}
		}
		
		void SequentialSearch(int[] list, int[] keys)
		{
			foreach (int key in keys)
			{
				list.SequentialSearch(key);
			}
		}

		const int size = 1000000;
		const int range = 100000000;
		const int keyCount = 100000;
		
		var list = Generator.UniformRandomInt(range).Take(size).ToResizableArray(size);
		
		Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla);
		int[] array = list.ToArray();
		
		int[] keys = Generator.UniformRandomInt(range)
			.Take(keyCount)
			.ToArray();

		var searchers = new List<Action<int[], int[]>>()
		{
			SequentialSearch,
			BinarySearch,
			CSharpBinarySearch,
			InterpolationSearch,
			
			SequentialSearch,
			BinarySearch,
			CSharpBinarySearch,
			InterpolationSearch,
			
		}.Select(AndPrintWhiteBoxInfo);

		var times = Timer.Time<int[], int[]>(searchers, () => array, () => keys);
		
		foreach (var time in times)
		{
			Console.WriteLine(time);
		}
	}

	private static void TimeSorts()
	{
		const int testCount = 1;
		const int itemCountBase = 1 << 23;
		
		Action<IReadonlyRandomAccessList<int>> AndPrintWhiteBoxInfo(Action<IReadonlyRandomAccessList<int>> action) =>
			list =>
			{
				action(list);
				__WriteCounts();
				__WriteEvents();
				__ClearWhiteBoxContainers();
			};
		
		for (int i = 1; i <= testCount; i++)
		{
			int count = itemCountBase*i;
			var items = Generator.UniformRandomInt(int.MaxValue)
				.Take(count)
				.ToResizableArray(count);

			var sorters = new List<Action<IReadonlyRandomAccessList<int>>>
				{
					/*Sort.SelectionSort,
				Sort.InsertionSort,
				Sort.ShellSortWithPrattSequence,
				Sort.DequeueSortWithDeque,
				Sort.DequeueSortWithQueue,
				Sort.GnomeSort,*/
				
					list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla),
					/*list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 12}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Optimized),*/
				
					list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla),
					/*list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
				list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
				list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 12}),
				list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Optimized),*/
				
					// Sort.MergeSortBottomsUpWithQueues,
					Sort.Merge3Sort,
					list => Sort.MergeKSort(list, 3),
					list => Sort.MergeKSort(list, 4),
					list => Sort.MergeKSort(list, 5),
					list => Sort.MergeKSort(list, 6),
					list => Sort.MergeKSort(list, 7),
				
					list => Sort.MergeKSortBottomUp(list, 3),
					list => Sort.MergeKSortBottomUp(list, 4),
					list => Sort.MergeKSortBottomUp(list, 5),
					list => Sort.MergeKSortBottomUp(list, 6),
					list => Sort.MergeKSortBottomUp(list, 7),
				
					// Sort.MergeSortNatural,
					list => Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla),
					list => Sort.QuickSort(list, Sort.QuickSortConfig.Vanilla with{ PivotSelection = Sort.QuickSortConfig.PivotSelectionAlgorithm.MedianOfThreeFirst}),
				}
				.Select(AndPrintWhiteBoxInfo);

			var names = new List<string>
			{
				/*nameof(Sort.SelectionSort),
				nameof(Sort.InsertionSort),
				nameof(Sort.ShellSort),
				nameof(Sort.DequeueSortWithDeque),
				nameof(Sort.DequeueSortWithQueue),
				nameof(Sort.GnomeSort),*/
				
				nameof(Sort.MergeSort) + "1",
				/*nameof(Sort.MergeSort) + "2",
				nameof(Sort.MergeSort) + "3",
				nameof(Sort.MergeSort) + "4",
				nameof(Sort.MergeSort) + "5",*/
				
				nameof(Sort.MergeSortBottomUp) + "1",
				/*nameof(Sort.MergeSortBottomUp) + "2",
				nameof(Sort.MergeSortBottomUp) + "3",
				nameof(Sort.MergeSortBottomUp) + "4",
				nameof(Sort.MergeSortBottomUp) + "5",*/
				
				// nameof(Sort.MergeSortBottomsUpWithQueues),
				nameof(Sort.Merge3Sort),
				nameof(Sort.MergeKSort) + "3",
				nameof(Sort.MergeKSort) + "4",
				nameof(Sort.MergeKSort) + "5",
				nameof(Sort.MergeKSort) + "6",
				nameof(Sort.MergeKSort) + "7",
				
				nameof(Sort.MergeKSortBottomUp) + "3",
				nameof(Sort.MergeKSortBottomUp) + "4",
				nameof(Sort.MergeKSortBottomUp) + "5",
				nameof(Sort.MergeKSortBottomUp) + "6",
				nameof(Sort.MergeKSortBottomUp) + "7",
				
				// nameof(Sort.MergeSortNatural),
				nameof(Sort.QuickSort),
				nameof(Sort.QuickSort),
			};

			var times = Timer.Time<IReadonlyRandomAccessList<int>>(sorters, () => items.Copy());

			foreach (var line in names.Zip(times))
			{
				Console.WriteLine($"{line.Second}");
			}
		}
	}
}
