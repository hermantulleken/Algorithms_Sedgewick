namespace Algorithms_Sedgewick;

using List;
using Timer = Support.Timer;

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

		var array = new ResizeableArray<int>{ 0, 1, 2, 3, 3, 4, 6, 7, 9, 10, 13, 15, 17 };

		int index = array.InterpolationSearch(6);
		
		Console.WriteLine(index);

		//TimeSorts();
	}

	private static void TimeSorts()
	{
		const int testCount = 1;
		const int itemCountBase = 1 << 23;
		
		Action<IReadonlyRandomAccessList<int>> AndPrintWhiteBoxInfo(Action<IReadonlyRandomAccessList<int>> action) =>
			list =>
			{
				action(list);
				Sort.WriteCounts();
				Sort.WriteEvents();
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
				
					//Sort.MergeSortBottomsUpWithQueues,
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
				
					//Sort.MergeSortNatural,
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
				
				//nameof(Sort.MergeSortBottomsUpWithQueues),
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
				
				//nameof(Sort.MergeSortNatural),
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
