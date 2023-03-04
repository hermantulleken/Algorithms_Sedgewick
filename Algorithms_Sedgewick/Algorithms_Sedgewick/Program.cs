
using Algorithms_Sedgewick.List;
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
		TimeSorts();
	}

	private static void TimeSorts()
	{
		const int testCount = 1;
		const int itemCountBase = 10000000;
		
		Action<IRandomAccessList<int>> AndPrintCounter(Action<IRandomAccessList<int>> action) =>
			list =>
			{
				action(list);
				Sort.WriteCounts();
			};
		
		for(int i = 1; i <= testCount; i++)
		{
			int count = itemCountBase*i;
			var items = Generator.UniformRandomInt(int.MaxValue)
				.Take(count)
				.ToResizableArray(count);

			var sorters = new List<Action<IRandomAccessList<int>>>
			{
				/*Sort.Sort.SelectionSort,
				Sort.Sort.InsertionSort,
				Sort.Sort.ShellSortWithPrattSequence,
				Sort.Sort.DequeueSortWithDeque,
				Sort.Sort.DequeueSortWithQueue,
				Sort.Sort.GnomeSort,*/
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SkipMergeWhenSorted = true}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{SmallArraySortAlgorithm = Sort.MergeSortConfig.SortAlgorithm.Insert, SmallArraySize = 12}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Vanilla with{UseFastMerge = true}),
				list => Sort.MergeSort(list, Sort.MergeSortConfig.Optimized),
				Sort.MergeSortBottomUp
			}
				.Select(AndPrintCounter);

			var names = new List<string>
			{
				/*nameof(Sort.Sort.SelectionSort),
				nameof(Sort.Sort.InsertionSort),
				nameof(Sort.Sort.ShellSort),
				nameof(Sort.Sort.DequeueSortWithDeque),
				nameof(Sort.Sort.DequeueSortWithQueue),
				nameof(Sort.Sort.GnomeSort),*/
				nameof(Sort.MergeSort) + "1",
				nameof(Sort.MergeSort) + "2",
				nameof(Sort.MergeSort) + "3",
				nameof(Sort.MergeSort) + "4",
				nameof(Sort.MergeSort) + "5",
				nameof(Sort.MergeSortBottomUp) + "1",
			};

			var times = Timer.Time<IRandomAccessList<int>>(sorters, () => items.Copy());

			foreach (var line in names.Zip(times))
			{
				Console.WriteLine($"{line.Second}");
			}
		}
	}
}
