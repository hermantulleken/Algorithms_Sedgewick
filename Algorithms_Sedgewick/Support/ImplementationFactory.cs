namespace Support;

using System.Collections;

public class ImplementationFactory<TBase> : IEnumerable<Func<TBase>>
	where TBase : notnull
{
	private readonly Dictionary<Type, Func<TBase>> factories = new();

	public void Add<TImplementation>(Func<TImplementation> factory)
		where TImplementation : TBase
	{
		factories.Add(typeof(TImplementation), () => factory());
	}

	public TImplementation GetInstance<TImplementation>()
		where TImplementation : TBase
	{
		var type = typeof(TImplementation);
		
		if (factories.TryGetValue(type, out var constructor))
		{
			return (TImplementation)constructor();
		}

		throw new InvalidOperationException($"No constructor found for type: {type}.");
	}

	public IEnumerator<Func<TBase>> GetEnumerator() => factories.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ImplementationFactory<T1, TBase> : IEnumerable<Func<T1, TBase>>
	where TBase : notnull
{
	private readonly Dictionary<Type, Func<T1, TBase>> factories = new();

	public void Add<TImplementation>(Func<T1, TImplementation> factory)
		where TImplementation : TBase
	{
		factories.Add(typeof(TImplementation), arg1 => factory(arg1));
	}

	public TImplementation GetInstance<TImplementation>(T1 arg1)
		where TImplementation : TBase
	{
		var type = typeof(TImplementation);
		
		if (factories.TryGetValue(type, out var constructor))
		{
			return (TImplementation)constructor(arg1);
		}

		throw new InvalidOperationException($"No constructor found for type: {type}.");
	}

	public IEnumerator<Func<T1, TBase>> GetEnumerator() => factories.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ImplementationFactory<T1, T2, TBase> : IEnumerable<Func<T1, T2, TBase>>
	where TBase : notnull
{
	private readonly Dictionary<Type, Func<T1, T2, TBase>> factories = new();

	public void Add<TImplementation>(Func<T1, T2, TImplementation> factory)
		where TImplementation : TBase
	{
		factories.Add(typeof(TImplementation), (arg1, arg2) => factory(arg1, arg2));
	}

	public TImplementation GetInstance<TImplementation>(T1 arg1, T2 arg2)
		where TImplementation : TBase
	{
		var type = typeof(TImplementation);
		
		if (factories.TryGetValue(type, out var constructor))
		{
			return (TImplementation)constructor(arg1, arg2);
		}

		throw new InvalidOperationException($"No constructor found for type: {type}.");
	}

	public IEnumerator<Func<T1, T2, TBase>> GetEnumerator() => factories.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ImplementationFactory<T1, T2, T3, TBase> : IEnumerable<Func<T1, T2, T3, TBase>>
	where TBase : notnull
{
	private readonly Dictionary<Type, Func<T1, T2, T3, TBase>> factories = new();

	public void Add<TImplementation>(Func<T1, T2, T3, TImplementation> factory)
		where TImplementation : TBase
	{
		factories.Add(typeof(TImplementation), (arg1, arg2, arg3) => factory(arg1, arg2, arg3));
	}

	public TImplementation GetInstance<TImplementation>(T1 arg1, T2 arg2, T3 arg3)
		where TImplementation : TBase
	{
		var type = typeof(TImplementation);
		
		if (factories.TryGetValue(type, out var constructor))
		{
			return (TImplementation)constructor(arg1, arg2, arg3);
		}

		throw new InvalidOperationException($"No constructor found for type: {type}.");
	}

	public IEnumerator<Func<T1, T2, T3, TBase>> GetEnumerator() => factories.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class ImplementationFactory<T1, T2, T3, T4, TBase> : IEnumerable<Func<T1, T2, T3, T4, TBase>>
	where TBase : notnull
{
	private readonly Dictionary<Type, Func<T1, T2, T3, T4, TBase>> factories = new();

	public void Add<TImplementation>(Func<T1, T2, T3, T4, TImplementation> factory)
		where TImplementation : TBase
	{
		factories.Add(typeof(TImplementation), (arg1, arg2, arg3, arg4) => factory(arg1, arg2, arg3, arg4));
	}

	public TImplementation GetInstance<TImplementation>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		where TImplementation : TBase
	{
		var type = typeof(TImplementation);
		
		if (factories.TryGetValue(type, out var constructor))
		{
			return (TImplementation)constructor(arg1, arg2, arg3, arg4);
		}

		throw new InvalidOperationException($"No constructor found for type: {type}.");
	}

	public IEnumerator<Func<T1, T2, T3, T4, TBase>> GetEnumerator() => factories.Values.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
