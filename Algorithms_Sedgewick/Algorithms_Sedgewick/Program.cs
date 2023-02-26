using Algorithms_Sedgewick.List;
using Support;
using Timer = Support.Timer;

namespace Algorithms_Sedgewick;

public class Generator
{
	private static readonly Random Random = new Random();
	public static IEnumerable<int> UniformRandomInt(int maxValue)
	{
		while (true)
		{
			yield return Random.Next(maxValue);
		}
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
		for(int i = 1; i <= 50; i++)
		{
			int count = 100000*i;
			var items = Generator.UniformRandomInt(int.MaxValue)
				.Take(count)
				.ToResizableArray(count);

			var sorters = new List<Action<IRandomAccessList<int>>>
			{
				//Sort.Sort.Selection,
				//Sort.Sort.Insertion,
				Sort.Sort.ShellSortWithPrattSequence,
				//Sort.Sort.DequeueWithDeque,
				//Sort.Sort.DequeueWithQueue,
				//Sort.Sort.Gnome
				Sort.Sort.MergeSort
			};

			var names = new List<string>
			{
				nameof(Sort.Sort.SelectionSort),
				nameof(Sort.Sort.InsertionSort),
				nameof(Sort.Sort.ShellSort),
				//nameof(Sort.Sort.DequeueWithDeque),
				//nameof(Sort.Sort.DequeueWithQueue),
				nameof(Sort.Sort.GnomeSort)
			};

			var times = Timer.Time<IRandomAccessList<int>>(sorters, () => items.Copy());

			foreach (var line in names.Zip(times))
			{
				Console.WriteLine($"{line.Second}");
			}
		}
	}
}
