namespace Algorithms_Sedgewick;

public static class ToStringUtilities
{
	public static string ToString2<T>(this T? obj) 
		=> (obj == null ? "<null>" : obj.ToString()) ?? "<ToString() returned null>";
}
