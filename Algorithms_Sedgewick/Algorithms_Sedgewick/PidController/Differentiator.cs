using Algorithms_Sedgewick.Buffer;

namespace Algorithms_Sedgewick;

public sealed class Differentiator
{
	private readonly IBuffer<float> buffer;

	public float Value
	{
		get => buffer.Last;
		set => buffer.Insert(value);
	}
    
	public float PreviousValue => buffer.First;

	/*
        Technically to be a derivative we need to divide by the time.
        If we assume a constant sample rate, this is a constant, that 
        can be absorbed by the PID filter. 
    */
	public float Difference =>
		buffer.Count == 2
			? Value - PreviousValue
			: throw new InvalidOperationException("Not enough values set to calculate a derivative");

	public Differentiator() => buffer = new RingBuffer<float>(2);
}