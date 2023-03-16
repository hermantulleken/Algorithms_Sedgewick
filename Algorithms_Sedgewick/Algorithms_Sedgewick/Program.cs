using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.Sort;
using Timer = Support.Timer;

namespace Algorithms_Sedgewick;

//Note: The methods in this class are infinite. Use Take to get a finite amount of elements. 
public static class Generator
{
	private static readonly Random Random = new();
	
	public static IEnumerable<int> UniformRandomInt(int maxValue)
	{
		while (true)
		{
			yield return Random.Next(maxValue);
		}
		// ReSharper disable once IteratorNeverReturns
	}
}

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
	public static void Main(string[] _)
	{
		// var list = new ResizeableArray<int>() {1, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		//
		// Sort.MergeSortNatural(list);
		//
		// Console.WriteLine(list.Pretty());

		TimeSorts();
	}

	private static void TimeSorts()
	{
		const int testCount = 1;
		const int itemCountBase = 1 << 23;
		
		Action<IReadonlyRandomAccessList<int>> AndPrintWhiteBoxInfo(Action<IReadonlyRandomAccessList<int>> action) =>
			list =>
			{
				action(list);
				Sort.Sort.WriteCounts();
				Sort.Sort.WriteEvents();
			};
		
		for(int i = 1; i <= testCount; i++)
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
				
					list => Sort.Sort.MergeSort(list, Sort.Sort.MergeSortConfig.Vanilla),
					/*list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 12}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Optimized),*/
				
					list => Sort.Sort.MergeSortBottomUp(list, Sort.Sort.MergeSortConfig.Vanilla),
					/*list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
				list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
				list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 12}),
				list => Sort.MergeSortBottomUp(list, Sort.MergeSortConfig.Optimized),*/
				
					//Sort.MergeSortBottomsUpWithQueues,
					Sort.Sort.Merge3Sort,
					list => Sort.Sort.MergeKSort(list, 3),
					list => Sort.Sort.MergeKSort(list, 4),
					list => Sort.Sort.MergeKSort(list, 5),
					list => Sort.Sort.MergeKSort(list, 6),
					list => Sort.Sort.MergeKSort(list, 7),
				
					list => Sort.Sort.MergeKSortBottomUp(list, 3),
					list => Sort.Sort.MergeKSortBottomUp(list, 4),
					list => Sort.Sort.MergeKSortBottomUp(list, 5),
					list => Sort.Sort.MergeKSortBottomUp(list, 6),
					list => Sort.Sort.MergeKSortBottomUp(list, 7),
				
					//Sort.MergeSortNatural,
					list => Sort.Sort.QuickSort(list, Sort.Sort.QuickSortConfig.Vanilla),
					list => Sort.Sort.QuickSort(list, Sort.Sort.QuickSortConfig.Vanilla with{ PivotSelection = Sort.Sort.QuickSortConfig.PivotSelectionAlgorithm.MedianOfThreeFirst}),
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
				
				nameof(Sort.Sort.MergeSort) + "1",
				/*nameof(Sort.MergeSort) + "2",
				nameof(Sort.MergeSort) + "3",
				nameof(Sort.MergeSort) + "4",
				nameof(Sort.MergeSort) + "5",*/
				
				nameof(Sort.Sort.MergeSortBottomUp) + "1",
				/*nameof(Sort.MergeSortBottomUp) + "2",
				nameof(Sort.MergeSortBottomUp) + "3",
				nameof(Sort.MergeSortBottomUp) + "4",
				nameof(Sort.MergeSortBottomUp) + "5",*/
				
				//nameof(Sort.MergeSortBottomsUpWithQueues),
				nameof(Sort.Sort.Merge3Sort),
				nameof(Sort.Sort.MergeKSort) + "3",
				nameof(Sort.Sort.MergeKSort) + "4",
				nameof(Sort.Sort.MergeKSort) + "5",
				nameof(Sort.Sort.MergeKSort) + "6",
				nameof(Sort.Sort.MergeKSort) + "7",
				
				nameof(Sort.Sort.MergeKSortBottomUp) + "3",
				nameof(Sort.Sort.MergeKSortBottomUp) + "4",
				nameof(Sort.Sort.MergeKSortBottomUp) + "5",
				nameof(Sort.Sort.MergeKSortBottomUp) + "6",
				nameof(Sort.Sort.MergeKSortBottomUp) + "7",
				
				//nameof(Sort.MergeSortNatural),
				nameof(Sort.Sort.QuickSort),
				nameof(Sort.Sort.QuickSort),
			};

			var times = Timer.Time<IReadonlyRandomAccessList<int>>(sorters, () => items.Copy());

			foreach (var line in names.Zip(times))
			{
				Console.WriteLine($"{line.Second}");
			}
		}
	}
}