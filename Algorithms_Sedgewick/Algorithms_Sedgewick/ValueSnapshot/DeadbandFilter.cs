namespace Algorithms_Sedgewick.ValueSnapshot;

using System.Globalization;

/// <summary>
/// Represents a filter that only updates its value if the change in value exceeds a specified threshold.
/// This is useful for filtering out noise or small fluctuations in data.
/// </summary>
public class DeadbandFilter
{
	private readonly float threshold;
	private float value;

	/// <summary>
	/// Initializes a new instance of the <see cref="DeadbandFilter"/> class with a specified threshold.
	/// </summary>
	/// <param name="threshold">The minimum change in value required for the filter to update its value.</param>
	public DeadbandFilter(float threshold)
	{
		this.threshold = threshold;
	}

	/// <summary>
	/// Gets or sets the current value of the filter.
	/// </summary>
	/// <remarks>
	/// When setting the value, the filter checks if the change in value exceeds the threshold.
	/// If the change is within the threshold, the value remains unchanged.
	/// </remarks>
	public float Value
	{
		get => value;
		set
		{
			if (Math.Abs(value - this.value) > threshold)
			{
				this.value = value;
			}
		}
	}

	/// <summary>
	/// Returns a string representation of the current value of the filter.
	/// </summary>
	/// <returns>A string representation of the current value.</returns>
	public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}
