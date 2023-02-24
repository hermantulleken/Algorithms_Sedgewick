using Algorithms_Sedgewick.List;
using Support;

namespace Algorithms_Sedgewick
{
	internal static class Program
	{
		public static void Main(string[] _)
		{
			var list = new ResizeableArray<int>{6, 1, 2, 3, 4, 5};
			
			Sort.Sort.Dequeue2(list);
			
			Console.WriteLine(Formatter.List(list));

			
		}
	}
}
