namespace Algorithms_Sedgewick.PidController;

using Buffer;

/// <summary>
/// Represents an integrator that calculates the sum of float values over a specified window.
/// </summary>
/// <remarks>
/// The integrator assumes a constant sample rate. Therefore, while technically the integral
/// requires scaling each interval by the time between samples, this constant can be absorbed 
/// by <see cref="PidController"/>, allowing the integrator to focus solely on the sum of values.
/// </remarks>
public sealed class Integrator
{
	private readonly IBuffer<float> buffer;
	private float sum;

	/// <summary>
	/// Gets the sum of the values in the buffer.
	/// </summary>
	/// <remarks>
	/// Technically, we need to scale each interval by the time between samples. We assume the
	/// sample rate is constant, and that it can be absorbed by the factor in <see cref="PidController"/>.
	/// </remarks>
	public float Sum => sum;

	/// <summary>
	/// Gets the number of values in this <see cref="Integrator"/>.
	/// </summary>
	public int Count => buffer.Count;

	/// <summary>
	/// Gets or sets the value of this <see cref="Integrator"/>.
	/// </summary>
	public float Value
	{
		get => buffer.Last;
		set
		{
			sum += value;

			if (Count >= buffer.Capacity)
			{
				sum -= buffer.First();
			}

			buffer.Insert(value);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Integrator"/> class.
	/// </summary>
	/// <param name="sumWindow">The number of values to consider in the sum.</param>
	public Integrator(int sumWindow) => buffer = new RingBuffer<float>(sumWindow);
}
