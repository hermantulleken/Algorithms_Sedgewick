namespace AlgorithmsSW;

using System.Diagnostics.CodeAnalysis;
using List;

/// <summary>
/// The MaxAggregator class is used to aggregate values based on a comparison value and keep track of the maximum
/// values and their set.
/// </summary>
/// <typeparam name="TValue">The type of the value to be aggregated.</typeparam>
/// <typeparam name="TComparisonValue">The type of the comparison value used to compare the values.</typeparam>
public class MaxAggregator<TValue, TComparisonValue>(IComparer<TComparisonValue> comparer)
{
	private TComparisonValue? maxComparisonValue = default;
	private ResizeableArray<TValue> maxValues = [];

	/// <summary>
	/// Gets the Comparer used for comparing comparison values for this aggregator. 
	/// </summary>
	public IComparer<TComparisonValue> Comparer { get; } = comparer;

	/// <summary>
	/// Gets the set of values that correspond to the maximum comparison value. 
	/// </summary>
	public IEnumerable<TValue> MaxValues => maxValues;

	/// <summary>
	/// Gets the maximum comparison value among the aggregated values.
	/// </summary>
	/// <value>The maximum comparison value.</value>
	/// <exception cref="InvalidOperationException">Thrown when there are no values in <see cref="MaxValues"/>.
	/// </exception>
	public TComparisonValue MaxComparisonComparisonValue
	{
		get
		{
			if (!HasMaxComparisonValue)
			{
				throw new InvalidOperationException("No MaxValue for an empty set.");
			}

			return maxComparisonValue;
		}
	}

	/// <summary>
	/// Gets a value indicating whether the MaxAggregator has a maximum comparison value.
	/// </summary>
	/// <remarks>
	/// This property returns true if the MaxAggregator has at least one value and therefore has a maximum comparison value.
	/// If the MaxAggregator is empty, the property returns false.
	/// </remarks>
	/// <value>
	/// True if the MaxAggregator has a maximum comparison value; false otherwise.
	/// </value>
	[MemberNotNullWhen(true, nameof(MaxComparisonComparisonValue))]
	[MemberNotNullWhen(true, nameof(maxComparisonValue))]
	public bool HasMaxComparisonValue => MaxValues.Any();

	/// <summary>
	/// Adds a value to the MaxAggregator if the provided comparison value is greater than the current maximum comparison value.
	/// </summary>
	/// <typeparam name="TValue">The type of the value to be added.</typeparam>
	/// <typeparam name="TComparisonValue">The type of the comparison value.</typeparam>
	/// <param name="value">The value to be added.</param>
	/// <param name="comparisonValue">The comparison value used to compare the values.</param>
	public void AddIfBigger(TValue value, TComparisonValue comparisonValue)
	{
		if (!HasMaxComparisonValue || Comparer.Less(maxComparisonValue, comparisonValue))
		{
			maxValues = [value];
			maxComparisonValue = comparisonValue;
		}
		else if (Comparer.Equal(maxComparisonValue, comparisonValue))
		{
			maxValues.Add(value);
		}
	}
}
