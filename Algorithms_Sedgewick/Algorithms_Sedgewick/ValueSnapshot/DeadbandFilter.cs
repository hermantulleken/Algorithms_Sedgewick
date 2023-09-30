using System.Globalization;

namespace Algorithms_Sedgewick.ValueSnapshot;

public class DeadbandFilter
{
	private readonly float threshold;
	
	private float value;
	
	public DeadbandFilter(float threshold)
	{
		this.threshold = threshold;
	}

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

	/// <inheritdoc />
	public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}
