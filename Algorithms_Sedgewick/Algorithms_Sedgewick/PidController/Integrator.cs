using Algorithms_Sedgewick.Buffer;

namespace Algorithms_Sedgewick.PidController;

public sealed class Integrator
{
	private readonly IBuffer<float> buffer;

	public float Value
	{
		get => buffer.Last;
		set => buffer.Insert(value);
	}
    
	public float PreviousValue => buffer.First;

	/*
        Technically, we need to scale each interval by the time between 
        samples. We assume the sample rate is constant, and that it can
        be absorbed by the factor in the PID controller.  
    */
	public float Sum => buffer.Sum();

	public Integrator(int sumWindow) => 
		buffer = new RingBuffer<float>(sumWindow);
}