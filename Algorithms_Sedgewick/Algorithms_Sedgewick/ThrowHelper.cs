using System.Runtime.CompilerServices;
using Algorithms_Sedgewick.List;
using global::System.Collections.Generic;

namespace Algorithms_Sedgewick;

internal static class ThrowHelper
{
	internal static readonly InvalidOperationException ContainerEmptyException = new InvalidOperationException(ContainerEmpty);
	internal static readonly InvalidOperationException ContainerFullException = new InvalidOperationException(ContainerFull);
	internal static readonly InvalidOperationException ContainerIsAtMaximumCapacityException = new InvalidOperationException(TheContainerIsAtMaximumCapacity);
	internal static readonly InvalidOperationException IteratingOverModifiedContainerException = new InvalidOperationException(IteratingOverModifiedContainer);

	internal const string IteratingOverModifiedContainer = "Iterating over a modified container.";
	internal const string CapacityCannotBeNegative = "Capacity cannot be negative.";
	internal const string CapacityCannotBeNegativeOrZero = "Capacity cannot be negative or zero.";
	internal const string ContainerEmpty = "The container is empty.";
	internal const string TheContainerIsAtMaximumCapacity = "The container is at maximum capacity.";
	internal const string ContainerFull = "The container is full.";

	//{0} - minimum needed
	internal const string NotEnoughElements = "The container does not have enough elements. It needs at least {0}.";
	
	//{0} - key
	internal const string KeyNotFound = "Key {0} not found in conainer.";

	internal static ArgumentException CapacityCannotBeNegativeOrZeroException(int argument, [CallerArgumentExpression("argument")] string argumentName = null) 
		=> new(CapacityCannotBeNegativeOrZero, argumentName);
	
	internal static ArgumentException CapacityCannotBeNegativeException(int argument, [CallerArgumentExpression("argument")] string argumentName = null) 
		=> new(CapacityCannotBeNegative, argumentName);

	internal static KeyNotFoundException KeyNotFoundException<TKey>(TKey key) 
		=> new(string.Format(KeyNotFound, key));
	
	internal static void ThrowContainerEmpty() 
		=> throw ContainerEmptyException;
	
	internal static void ThrowContainerFull() 
		=> throw ContainerFullException;
	
	internal static void ThrowTheContainerIsAtMaximumCapacity() 
		=> throw ContainerIsAtMaximumCapacityException;
	
	internal static void ThrowCapacityCannotBeNegativeOrZero(int argument, [CallerArgumentExpression("argument")] string argumentName=null)
		=> throw new ArgumentException(CapacityCannotBeNegativeOrZero, argumentName);
	
	internal static void ThrowCapacityCannotBeNegative(int argument, [CallerArgumentExpression("argument")] string argumentName=null) 
		=> throw new ArgumentException(CapacityCannotBeNegative, argumentName);
	
	internal static void ThrowIteratingOverModifiedContainer() 
		=> throw IteratingOverModifiedContainerException;
	
	internal static T ThrowIfNull<T>(this T obj, [CallerArgumentExpression("obj")] string objArgName=null)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(objArgName);
		}

		return obj;
	}

	internal static int ThrowIfOutOfRange(this int n, int end, [CallerArgumentExpression("n")] string objArgName = null)
		=> ThrowIfOutOfRange(n, 0, end, objArgName);
	
	internal static int ThrowIfOutOfRange(this int n, int start, int end, [CallerArgumentExpression("n")] string objArgName=null)
	{
		if (n < start || n >= end)
		{
			throw new ArgumentOutOfRangeException(objArgName);
		}

		return n;
	}

	internal static T[] ThrowIfEmpty<T>(this T[] list, [CallerArgumentExpression("list")] string listArgName = null)
	{
		if (list.Length == 0)
		{
			ThrowContainerEmpty();
		}

		return list;
	}
	
	internal static IReadonlyRandomAccessList<T> ThrowIfEmpty<T>(this IReadonlyRandomAccessList<T> list, [CallerArgumentExpression("list")] string listArgName = null)
	{
		if (list.IsEmpty)
		{
			ThrowContainerEmpty();
		}

		return list;
	}

	public static void ThrowKeyNotFound<TKey>(TKey key) 
		=> throw KeyNotFoundException(key);

	public static void ThrowNotEnoughElements(int minNeeded)
		=> throw new InvalidOperationException(string.Format(NotEnoughElements, minNeeded));

	[Obsolete("Use a one of the other Throw methods that will throw a more specific exception.")]
	public static void ThrowException(string message)
	{
		throw new Exception(message);
	}
}
