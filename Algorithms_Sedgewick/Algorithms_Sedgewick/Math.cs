namespace Algorithms_Sedgewick;

public class Math2
{
	public static int IntegerCeilLog2(int n) 
		=> n <= 1 ? 0 : (int)Math.Ceiling(Math.Log(n, 2));
}
