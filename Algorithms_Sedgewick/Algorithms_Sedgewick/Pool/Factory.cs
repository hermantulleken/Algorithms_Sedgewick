namespace Algorithms_Sedgewick.Pool;

public static class Factory
{
	private class FuncFactory<T> : IFactory<T>
	{
		private readonly Func<T> create;
		private readonly Action<T>? destroy;

		public FuncFactory(Func<T> create, Action<T>? destroy = null)
		{
			create.ThrowIfNull(nameof(create));
			
			this.create = create;
			this.destroy = destroy;
		}
		
		public T GetNewInstance() => create();
		
		public void Reset(T instance) => destroy?.Invoke(instance);
	}
	
	public static IFactory<T> Create<T>(Func<T> create, Action<T>? destroy = null) => new FuncFactory<T>(create, destroy);
}
