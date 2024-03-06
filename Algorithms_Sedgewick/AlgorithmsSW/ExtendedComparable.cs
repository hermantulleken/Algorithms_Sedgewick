namespace AlgorithmsSW;

using System.ComponentModel;
using System.Numerics;

/// <summary>
/// Wraps a value in a type that can also represent infinity. This gives us the ability to work more uniformly with
/// floats and integers.
/// </summary>
/// <typeparam name="TComparable">The type of value to wrap.</typeparam>
/*
	The design of this class is flawed:
		- It does not work well with the infinities that float and double already can express.
		- It does not work well with NaN.
		- It is somewhat awkward to have the negative infinity present for unsigned numbers.

	Designing this class to solve the above problems seem possible, but a very big task to do correctly.
	The design here is sufficient for using it in our algorithms though.
*/
public readonly struct ExtendedComparable<TComparable>
	where TComparable : IComparisonOperators<TComparable, TComparable, bool>
{
	private enum ValueType
	{
		NegativeInfinity = -1,
		Finite = 0,
		PositiveInfinity = 1,
	}
	
	public static readonly ExtendedComparable<TComparable> NegativeInfinity = new(ValueType.NegativeInfinity);

	public static readonly ExtendedComparable<TComparable> PositiveInfinity = new(ValueType.PositiveInfinity);
	
	private readonly ValueType type;
	private readonly TComparable finiteValue;

	public TComparable FiniteValue => 
		type == ValueType.Finite ? finiteValue : throw new InvalidOperationException("Not a finite value.");

	/// <summary>
	/// Initializes a new instance of the <see cref="ExtendedComparable{TComparable}"/> struct that represents a finite
	/// value.
	/// </summary>
	/// <param name="finiteValue">The finite value this <see cref="ExtendedComparable{TComparable}"/> represents.</param>
	/// <remarks>
	/// Use <see cref="NegativeInfinity"/> and <see cref="PositiveInfinity"/> for non-finite values. 
	/// </remarks>
	public ExtendedComparable(TComparable finiteValue)
	{
		this.finiteValue = finiteValue;
		type = ValueType.Finite;
	}

	private ExtendedComparable(ValueType type)
	{
		this.type = type;
		finiteValue = default!; // Just to be neat
	}
	
	public static bool operator <(ExtendedComparable<TComparable> left, ExtendedComparable<TComparable> right)
	{
		if (left.type == ValueType.PositiveInfinity)
		{
			return false;
		}

		if (right.type == ValueType.NegativeInfinity)
		{
			return false;
		}

		if (left.type == ValueType.NegativeInfinity && right.type != ValueType.NegativeInfinity)
		{
			return true;
		}

		if (right.type == ValueType.PositiveInfinity && left.type != ValueType.PositiveInfinity)
		{
			return true;
		}
		
		return left.FiniteValue < right.FiniteValue;
	}

	public static bool operator >(ExtendedComparable<TComparable> left, ExtendedComparable<TComparable> right) 
		=> right < left;

	public static bool operator <=(ExtendedComparable<TComparable> left, ExtendedComparable<TComparable> right) 
		=> left < right || left == right;

	public static bool operator >=(ExtendedComparable<TComparable> left, ExtendedComparable<TComparable> right) 
		=> right <= left;

	public static bool operator ==(ExtendedComparable<TComparable> left, ExtendedComparable<TComparable> right)
	{
		if (left.type != right.type)
		{
			return false;
		}

		if (left.type == ValueType.Finite && right.type == ValueType.Finite)
		{
			return left.FiniteValue == right.FiniteValue;
		}

		// If both are not finite, they are equal if they are the same type of infinity
		return true;
	}

	public static bool operator !=(ExtendedComparable<TComparable> left, ExtendedComparable<TComparable> right) 
		=> !(left == right);

	public static explicit operator ExtendedComparable<TComparable>(TComparable value) => new(value);

	// Override Equals and GetHashCode to be consistent with the operator overloads
	public override bool Equals(object? obj) 
		=> obj is ExtendedComparable<TComparable> other && this == other;

	public override int GetHashCode() 
		=> type == ValueType.Finite 
			? HashCode.Combine(type, finiteValue) 
			: HashCode.Combine(type);

	public override string ToString() 
		=> type switch
		{
			ValueType.Finite => finiteValue.ToString()!,
			ValueType.PositiveInfinity => "+∞",
			ValueType.NegativeInfinity => "-∞",
			_ => throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(ValueType)),
		};
}
