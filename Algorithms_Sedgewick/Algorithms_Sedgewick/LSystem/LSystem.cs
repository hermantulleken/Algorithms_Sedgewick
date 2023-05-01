namespace Algorithms_Sedgewick.LSystem;

public static class LSystem
{
	public static readonly LSystem<char> Hilbert = new(
		"A".ToArray(),
		new[] 
		{
			('A', "-BF+AFA+FB-".ToArray()),
			('B', "+AF-BFB-FA+".ToArray()),
		});
	
	public static readonly LSystem<char> Gosper = new(
		"A".ToArray(),
		new[] 
		{
			('A', "A-B--B+A++AA+B-".ToArray()),
			('B', "+A-BB--B-A++A+B".ToArray()),
		});
}

public class LSystem<T>
{
	public T[] Axiom { get; }

	public (T, T[])[] Rules { get; }

	public LSystem(T[] axiom, (T, T[])[] rules)
	{
		Axiom = axiom;
		Rules = rules;
	}
}
