﻿using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace AlgorithmsSW;

using System.Runtime.CompilerServices;
using List;

/// <summary>
/// Provides methods that throw exceptions.
/// </summary>
internal static class ThrowHelper
{
	internal const string CapacityCannotBeNegative = "Capacity cannot be negative.";
	internal const string CapacityCannotBeNegativeOrZero = "Capacity cannot be negative or zero.";
	internal const string ContainerEmpty = "The container is empty.";
	internal const string ContainerFull = "The container is full.";
	internal const string IteratingOverModifiedContainer = "Iterating over a modified container.";

	// {0} - key
	internal const string KeyNotFound = "Key {0} not found in container.";

	// {0} - minimum needed
	internal const string NotEnoughElements = "The container does not have enough elements. It needs at least {0}.";
	internal const string TheContainerIsAtMaximumCapacity = "The container is at maximum capacity.";

	internal static readonly InvalidOperationException ContainerEmptyException = new(ContainerEmpty);
	internal static readonly InvalidOperationException ContainerFullException = new(ContainerFull);
	internal static readonly InvalidOperationException ContainerIsAtMaximumCapacityException = new(TheContainerIsAtMaximumCapacity);
	internal static readonly InvalidOperationException IteratingOverModifiedContainerException = new(IteratingOverModifiedContainer);
	internal static readonly Exception UnreachableCodeException = new InvalidOperationException("Unreachable code.");

	[Obsolete("Use a one of the other Throw methods that will throw a more specific exception.")]
	public static void ThrowException(string message) => throw new Exception(message);

	public static void ThrowGapAtBeginning()
		=> throw new InvalidOperationException("The gap is already at the beginning.");

	public static void ThrowGapAtEnd()
		=> throw new InvalidOperationException("The gap is already at the end.");

	public static void ThrowInvalidOperationException(string message) 
		=> throw new InvalidOperationException(message);
	
	public static void ThrowArgumentOutOfRangeException(string message) 
		=> throw new ArgumentOutOfRangeException(message);

	public static void ThrowKeyNotFound<TKey>(TKey key) => throw KeyNotFoundException(key);

	public static void ThrowNotEnoughElements(int minNeeded)
		=> throw new InvalidOperationException(string.Format(NotEnoughElements, minNeeded));

	internal static ArgumentException CapacityCannotBeNegativeException(int argument, [CallerArgumentExpression("argument")] string? argumentName = null) 
		=> new(CapacityCannotBeNegative, argumentName);

	internal static ArgumentException CapacityCannotBeNegativeOrZeroException(int argument, [CallerArgumentExpression("argument")] string? argumentName = null) 
		=> new(CapacityCannotBeNegativeOrZero, argumentName);

	internal static KeyNotFoundException KeyNotFoundException<TKey>(TKey key) 
		=> new(string.Format(KeyNotFound, key));

	[DoesNotReturn]
	internal static void ThrowCapacityCannotBeNegative(int argument, [CallerArgumentExpression("argument")] string? argumentName = null) 
		=> throw new ArgumentException(CapacityCannotBeNegative, argumentName);

	[DoesNotReturn]
	internal static void ThrowCapacityCannotBeNegativeOrZero(int argument, [CallerArgumentExpression("argument")] string? argumentName = null)
		=> throw new ArgumentException(CapacityCannotBeNegativeOrZero, argumentName);

	[DoesNotReturn]
	internal static void ThrowContainerEmpty() 
		=> throw ContainerEmptyException;

	[DoesNotReturn]
	internal static void ThrowContainerFull() 
		=> throw ContainerFullException;

	internal static T?[] ThrowIfEmpty<T>(this T?[] list, [CallerArgumentExpression("list")] string? listArgName = null)
	{
		if (list.Length == 0)
		{
			ThrowContainerEmpty();
		}

		return list;
	}

	internal static IReadonlyRandomAccessList<T> ThrowIfEmpty<T>(this IReadonlyRandomAccessList<T> list, [CallerArgumentExpression("list")] string? listArgName = null)
	{
		if (list.IsEmpty)
		{
			ThrowContainerEmpty();
		}

		return list;
	}

	internal static T ThrowIfNull<T>(
		[System.Diagnostics.CodeAnalysis.NotNull, NoEnumeration] this T? obj, 
		[CallerArgumentExpression("obj")] string? objArgName = null)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(objArgName);
		}

		return obj;
	}

	internal static int ThrowIfOutOfRange(this int n, int end, [CallerArgumentExpression("n")] string? objArgName = null)
		=> ThrowIfOutOfRange(n, 0, end, objArgName);

	[AssertionMethod]
	internal static int ThrowIfOutOfRange(this int n, int start, int end, [CallerArgumentExpression("n")] string? objArgName = null)
	{
		if (n < start || n >= end)
		{
			throw new ArgumentOutOfRangeException(objArgName);
		}

		return n;
	}
	
	[AssertionMethod]
	internal static int ThrowIfNegative(this int n, [CallerArgumentExpression("n")] string? objArgName = null)
	{
		if (n < 0)
		{
			throw new ArgumentOutOfRangeException(objArgName);
		}

		return n;
	}
	
	[AssertionMethod]
	internal static int ThrowIfNotPositive(this int n, [CallerArgumentExpression("n")] string? objArgName = null)
	{
		if (n <= 0)
		{
			throw new ArgumentOutOfRangeException(objArgName);
		}

		return n;
	}

	internal static void ThrowIfVersionMismatch(this int version, int expectedVersion)
	{
		if (version != expectedVersion)
		{
			ThrowIteratingOverModifiedContainer();
		}
	}

	internal static void ThrowIteratingOverModifiedContainer() 
		=> throw IteratingOverModifiedContainerException;

	internal static void ThrowTheContainerIsAtMaximumCapacity() 
		=> throw ContainerIsAtMaximumCapacityException;

	public static void ThrowEndOfStream()
	{
		throw new InvalidOperationException("Reached the end of the stream.");
	}

	public static Exception TriedButFailed(string queryMethodName, string hasResultMethodName, string tryQueryMethodName)
	{
		return new InvalidOperationException($"Tried to get the result of {queryMethodName} but {hasResultMethodName} returned false. " +
											$"Try using {tryQueryMethodName} instead, or call {hasResultMethodName} before calling {queryMethodName}.");
	}
}
