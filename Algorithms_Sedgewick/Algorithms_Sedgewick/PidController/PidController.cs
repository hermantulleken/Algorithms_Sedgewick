namespace Algorithms_Sedgewick.PidController;

public sealed class PidController
{
	private readonly Differentiator differentiator;

	private readonly float differentiatorFactor;
	private readonly float integrationFactor;
	private readonly Integrator integrator;
	private readonly float proportionalFactor;

	public float FilteredValue => 
		proportionalFactor * Value 
		+ differentiatorFactor * differentiator.Difference 
		+ integrationFactor * integrator.Sum;

	public float Value
	{
		get => differentiator.Value; //Could also use integrator
        
		set
		{
			differentiator.Value = value;
			integrator.Value = value;
		}
	}

	public PidController(int integrationWindow, float proportionalFactor, float differentiatorFactor, float integrationFactor)
	{
		differentiator = new Differentiator();
		integrator = new Integrator(integrationWindow);

		this.proportionalFactor = proportionalFactor;
		this.differentiatorFactor = differentiatorFactor;
		this.integrationFactor = integrationFactor;
	}
}