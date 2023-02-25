using Algorithms_Sedgewick.List;
using Support;

namespace Algorithms_Sedgewick
{
	internal static class Program
	{
		public static void Main(string[] _)
		{
			var list = new ResizeableArray<int>
			{
				//6, 1, 2, 3, 4, 5
				1, 2, 3, 4, 5, 6, 7, 8, 9
			};
			
			Sort.Sort.DequeueWithQueue(list);
			
			Console.WriteLine(Formatter.Pretty(list));

			
		}
	}
}
