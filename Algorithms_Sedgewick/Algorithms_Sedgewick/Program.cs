using Algorithms_Sedgewick.HashTable;
using Algorithms_Sedgewick.SymbolTable;
using Support;

namespace Algorithms_Sedgewick;

using List;
using static WhiteBoxTesting;
using Timer = Support.Timer;

internal static class Program
{
	public static void Main()
	{
		int start = 4;
		int end = 5;

		// Execute the loop body concurrently for each index in the range
		Parallel.For(start, end, TimeSymbolTables);

		// imeSearchers();

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
		const int keyCount = 800000;
		
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
	
	public static void TimeSymbolTables(int n)
	{
		const int keysToFindCount = 10000000;
		
		int keysToAddCount = 1 << n;
		int range = keysToAddCount;
		
		// Console.WriteLine(keysToAddCount);
		
		var comparer = Comparer<int>.Default;
		
		void Experiment(ISymbolTable<int, int> table, int[] keysToAdd, int[] keysToFind)
		{
			int count = keysToFind.Count(table.ContainsKey);
			// Console.WriteLine($"count = {count} {table.Count} ... {(float)count / keysToFindCount}");
		}

		// keys lie between 0 and 2 range
		var keysToAdd = Generator
			.UniformRandomInt(2*range)
			.Take(keysToAddCount)
			.ToArray();
		
		// keys lie between range and 3 range
		var keysToFind = Generator
			.UniformRandomInt(2 * range)
			.Select(x => x + range)
			.Take(keysToFindCount)
			.ToArray();
		
		// Console.WriteLine("----");
		Func<ISymbolTable<int, int>> AddKeys(Func<ISymbolTable<int, int>> factory)
		{
			var table = factory();

			foreach (int key in keysToAdd)
			{
				table.Add(key, key);
			}

			return () => table;
		}

		Action<int[], int[]> FactoryToExperiment(Func<ISymbolTable<int, int>> factory)
		{
			return (keysToAdd, keysToFind) => Experiment(factory(), keysToAdd, keysToFind);
		}
		
		var factories = new Func<ISymbolTable<int, int>>[]
		{
			() => new SymbolTableWithKeyArray<int, int>(comparer),
			() => new SymbolTableWithSelfOrderingKeyArray<int, int>(comparer),
			
			() => new SymbolTableWithParallelArrays<int, int>(comparer),
			() => new OrderedSymbolTableWithUnorderedLinkedList<int, int>(comparer),
			
			() => new OrderedSymbolTableWithOrderedArray<int, int>(comparer),
			() => new SymbolTableWithOrderedParallelArray<int, int>(comparer),
			
			() => new OrderedSymbolTableWithOrderedKeyArray<int, int>(comparer),
			() => new OrderedSymbolTableWithOrderedLinkedList<int, int>(comparer),
			
			() => new SymbolTableWithBinarySearchTree<int, int>(comparer),
			
			() => new HashTableWithLinearProbing<int, int>(comparer),
			() => new HashTableWithLinearProbing2<int, int>(comparer),
			() => new HashTableWithSeparateChaining<int, int>(320000, comparer),
			() => new HashTableWithSeparateChaining2<int, int>(320000, comparer),
			() => new CuckooHashTable<int, int>(comparer),
			() => new SystemDictionary<int, int>(comparer),
		}.Select(AddKeys);
		
		var factoryTypeNames = new[]
		{
			nameof(SymbolTableWithKeyArray<int, int>),
			nameof(SymbolTableWithSelfOrderingKeyArray<int, int>),
			nameof(SymbolTableWithParallelArrays<int, int>),
			nameof(OrderedSymbolTableWithUnorderedLinkedList<int, int>),
			nameof(OrderedSymbolTableWithOrderedArray<int, int>),
			nameof(SymbolTableWithOrderedParallelArray<int, int>),
			
			nameof(OrderedSymbolTableWithOrderedKeyArray<int, int>),
			nameof(OrderedSymbolTableWithOrderedLinkedList<int, int>),
			
			nameof(SymbolTableWithBinarySearchTree<int, int>),
			
			nameof(HashTableWithLinearProbing<int, int>),
			nameof(HashTableWithLinearProbing2<int, int>),
			nameof(HashTableWithSeparateChaining<int, int>),
			nameof(HashTableWithSeparateChaining2<int, int>),
			nameof(CuckooHashTable<int, int>),
			nameof(SystemDictionary<int, int>),
		};

		var experiments 
			= factories.Select(FactoryToExperiment);

		var times = Timer.Time(experiments, () => keysToAdd, () => keysToFind);
		Console.WriteLine(Formatter.DottedLine);
		Console.WriteLine(keysToAddCount);
		Console.WriteLine(Formatter.DottedLine);
		
		foreach (var time in times.Zip(factoryTypeNames))
		{
			Console.WriteLine(time.First);
		}
		
		Console.WriteLine(Formatter.DottedLine);
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
