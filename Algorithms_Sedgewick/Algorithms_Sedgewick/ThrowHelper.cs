using System.Runtime.CompilerServices;

namespace Algorithms_Sedgewick;

internal class ThrowHelper
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

	internal static ArgumentException CapacityCannotBeNegativeOrZeroException(int argument, [CallerArgumentExpression("argument")] string argumentName = null) 
		=> new(CapacityCannotBeNegativeOrZero, argumentName);
	
	internal static ArgumentException CapacityCannotBeNegativeException(int argument, [CallerArgumentExpression("argument")] string argumentName = null) 
		=> new(CapacityCannotBeNegative, argumentName);
	
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
	
}
