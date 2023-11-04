using System.Diagnostics.CodeAnalysis;
using System.Text;
using Algorithms_Sedgewick;
using Algorithms_Sedgewick.Buffer;
using Algorithms_Sedgewick.List;
using Algorithms_Sedgewick.LSystem;
using Algorithms_Sedgewick.Sort;
using Support;

namespace PerformanceTests;

using static WhiteBoxTesting;
using Timer = Support.Timer;

internal static class Program
{
	[SuppressMessage("ReSharper", "UnreachableSwitchCaseDueToIntegerAnalysis")]
	public static void Main()
	{
		var runner = new LinkedListWithPooledNodesTest();
		runner.Run();
		
		//Buffers();
		
		/*TestSequenceInterpolation1();
		TestSequenceInterpolation2();*/
		return;
		
		var tests = new SymbolTablePerformanceTests();

		var experimentType = ExperimentType.TimeAddKeys; // Change this to run a different experiment

		switch (experimentType)
		{
			case ExperimentType.LookupKeys:
				tests.RunLookupKeys();
				break;
        
			case ExperimentType.TimeAddKeys:
				tests.RunTimeAddKeys();
				break;

			case ExperimentType.Sorts:
				TimeSorts();
				break;

			case ExperimentType.Searchers:
				TimeSearchers();
				break;

			default:
				Console.WriteLine("Invalid experiment type");
				break;
		}
	}

	public static string GenerateSvgFromFloatCoordinates(List<(float, float)> pixelCoordinates, float strokeWidth = 1, float padding = 10)
	{
		float minX = pixelCoordinates.Min(coord => coord.Item1);
		float maxX = pixelCoordinates.Max(coord => coord.Item1);
		float minY = pixelCoordinates.Min(coord => coord.Item2);
		float maxY = pixelCoordinates.Max(coord => coord.Item2);

		float width = maxX - minX + 2 * padding;
		float height = maxY - minY + 2 * padding;

		StringBuilder svgBuilder = new StringBuilder();

		svgBuilder.AppendLine($"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" viewBox=\"0 0 {width} {height}\">");
		svgBuilder.AppendLine($"<path d=\"M{pixelCoordinates[0].Item1 - minX + padding},{height - (pixelCoordinates[0].Item2 - minY + padding)}");

		for (int i = 1; i < pixelCoordinates.Count; i++)
		{
			svgBuilder.Append($" L{pixelCoordinates[i].Item1 - minX + padding},{height - (pixelCoordinates[i].Item2 - minY + padding)}");
		}

		svgBuilder.AppendLine($"\" stroke=\"black\" fill=\"none\" stroke-width=\"{strokeWidth}\" />");
		svgBuilder.AppendLine("</svg>");

		return svgBuilder.ToString();
	}
	
	private static void TestSequenceInterpolation1()
	{
		const int iterationCount = 4;
		var floatCoordinates = LSystem2D.Hilbert.GenerateCoordinates(iterationCount);
		string svg = GenerateSvgFromFloatCoordinates(floatCoordinates, 0.1f, 0.1f);
		File.WriteAllText("hilbert_{iterationCount}.svg", svg);
	}
	
	private static void TestSequenceInterpolation2()
	{
		const int iterationCount = 4;
		var floatCoordinates = LSystem2D.Gosper.GenerateCoordinates(iterationCount);
		string svg = GenerateSvgFromFloatCoordinates(floatCoordinates, 0.1f, 0.1f);
		File.WriteAllText($"curve_{iterationCount}.svg", svg);
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
			}
			.Select(AndPrintWhiteBoxInfo);

		var times = Timer.Time(searchers, () => array, () => keys);
			
		foreach (var time in times)
		{
			Console.WriteLine(time);
		}
	}

	private static void TimeSorts()
	{
		const int testCount = 1;
		const int itemCountBase = 1 << 23;
		
		Action<IRandomAccessList<int>> AndPrintWhiteBoxInfo(Action<IRandomAccessList<int>> action) =>
			list =>
			{
				action(list);
				__WriteCounts();
				__WriteEvents();
				__ClearWhiteBoxContainers();
			};
		
		for (int i = 1; i <= testCount; i++)
		{
			int count = itemCountBase * i;
			var items = Generator.UniformRandomInt(int.MaxValue)
				.Take(count)
				.ToResizableArray(count);

			var sorters = new List<Action<IRandomAccessList<int>>>
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
					list => Sort.QuickSort(list, new Sort.QuickSortConfig { PivotSelection = Sort.QuickSortConfig.PivotSelectionAlgorithm.MedianOfThreeFirst }),
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

			var times = Timer.Time(sorters, () => items.Copy());

			foreach (var line in names.Zip(times))
			{
				Console.WriteLine($"{line.Second}");
			}
		}
	}

	private static void Buffers()
	{
		int range = 100000;
		int count = 100000000;
		
		var sourceList = 
			Generator
			.UniformRandomInt(range)
			.Take(count)
			.Select(n => n / (float) range)
			.ToResizableArray(count);

		void CalcAverageDistance(IBuffer<float> buffer, ResizeableArray<float> values)
		{
			float differenceSum = 0;
			
			foreach (float f in values)
			{
				buffer.Insert(f);

				if (buffer.Count == 2)
				{
					float difference = buffer.Last - buffer.First;
					differenceSum += difference;
				}
			}
			
			Console.WriteLine(differenceSum / (values.Count - 1));
		}

		var tests = new List<Action<ResizeableArray<float>>>()
		{
			list => CalcAverageDistance(new Capacity2Buffer<float>(), list),
			list => CalcAverageDistance(new RingBuffer<float>(2), list),
			
			list => CalcAverageDistance(new Capacity2Buffer<float>(), list),
			list => CalcAverageDistance(new RingBuffer<float>(2), list),
			
			list => CalcAverageDistance(new Capacity2Buffer<float>(), list),
			list => CalcAverageDistance(new RingBuffer<float>(2), list),
			
			list => CalcAverageDistance(new Capacity2Buffer<float>(), list),
			list => CalcAverageDistance(new RingBuffer<float>(2), list),
			
			list => CalcAverageDistance(new Capacity2Buffer<float>(), list),
			list => CalcAverageDistance(new RingBuffer<float>(2), list),
		};

		var names = new List<string>
		{
			nameof(Capacity2Buffer<float>),
			nameof(RingBuffer<float>),
			
			nameof(Capacity2Buffer<float>),
			nameof(RingBuffer<float>),
			
			nameof(Capacity2Buffer<float>),
			nameof(RingBuffer<float>),
			
			nameof(Capacity2Buffer<float>),
			nameof(RingBuffer<float>),
			
			nameof(Capacity2Buffer<float>),
			nameof(RingBuffer<float>),
		};
		
		var times = Timer.Time(tests, () => sourceList.Copy());
		
		foreach (var line in names.Zip(times))
		{
			Console.WriteLine($"{line.Second}");
		}
	}
}
