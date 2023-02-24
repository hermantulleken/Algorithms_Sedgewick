using System.Collections;

namespace Algorithms_Sedgewick.List;

public static class ListExtensions
{
	private sealed class ListWrapper<T> : IRandomAccessList<T>
	{
		private readonly IList<T> list;

		public ListWrapper(IList<T> list)
		{
			this.list = list;
		}

		public int Count => list.Count;
		public bool IsEmpty => list.Count == 0;

		public T this[int index]
		{
			get => list[index];
			set => list[index] = value;
		}

		public IRandomAccessList<T> Copy() => list.ToList().ToRandomAccessList();
		
		public static implicit operator  ListWrapper<T>(T[] list) => new(list);
		public static implicit operator  ListWrapper<T>(List<T> list) => new(list);

		public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
	
	public static IRandomAccessList<T> ToRandomAccessList<T>(this IList<T> list) => new ListWrapper<T>(list);
}
