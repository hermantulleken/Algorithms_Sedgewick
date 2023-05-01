namespace Algorithms_Sedgewick;

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

public class LSystem2D
{
	public static readonly LSystem2D<char> Hilbert = new LSystem2DChar(
		LSystem.Hilbert,
		StringToCoordinates4,
		ConvertIntegerToSquarePixelCoordinates,
		4, 
		true);

	public static readonly LSystem2D<char> Gosper = new LSystem2DChar(
		LSystem.Gosper,
		StringToCoordinates6,
		ConvertIntegerToHexPixelCoordinates,
		6,
		false);
	
	public static List<(int x, int y)> StringToCoordinates4(char[] input, int startX, int startY)
	{
		List<(int x, int y)> coordinates = new List<(int x, int y)>();

		int x = startX;
		int y = startY;

		coordinates.Add((x, y));

		foreach (char c in input)
		{
			switch (c)
			{
				case '1':
					y++;
					break;
				case '3':
					y--;
					break;
				case '2':
					x--;
					break;
				case '0':
					x++;
					break;
			}

			coordinates.Add((x, y));
		}

		return coordinates;
	}
	
	public static List<(int x, int y)> StringToCoordinates6(char[] input, int x, int y)
	{
		List<(int x, int y)> coordinates = new List<(int x, int y)>();
		coordinates.Add((x, y));

		foreach (char command in input)
		{
			switch (command)
			{
				case '0': // Right
					x++;
					break;
				case '1': // Up-Right
					y++;
					break;
				case '2': // Up-Left
					x--;
					y++;
					break;
				case '3': // Left
					x--;
					break;
				case '4': // Down-Left
					y--;
					break;
				case '5': // Down-Right
					x++;
					y--;
					break;
				default:
					continue;
			}

			coordinates.Add((x, y));
		}

		return coordinates;
	}
	
	public static List<(float x, float y)> ConvertIntegerToHexPixelCoordinates(List<(int x, int y)> coordinates, float size)
	{
		float sqrt3 = (float)Math.Sqrt(3);

		Func<(int x, int y), (float x, float y)> axialToPixel = (coord) =>
		{
			float x = size * (3.0f / 2.0f * coord.x);
			float y = size * (sqrt3 * (coord.x * 0.5f + coord.y));
			return (x, y);
		};

		return coordinates.Select(axialToPixel).ToList();
	}

	public static List<(float x, float y)> ConvertIntegerToSquarePixelCoordinates(List<(int x, int y)> coordinates, float size)
	{
		return coordinates.Select(coord => (coord.x * size, coord.y * size)).ToList();
	}
}

public abstract class LSystem2D<T>
{
	public LSystem<T> LSystem { get; }

	public Func<T[], int, int, List<(int x, int y)>> ToCoordinates { get; }
    
	public Func<List<(int x, int y)>, float, List<(float x, float y)>> ToFloatCoordinates { get; }

	public LSystem2D(LSystem<T> lSystem, Func<T[], int, int, List<(int x, int y)>> toCoordinates, Func<List<(int x, int y)>, float, List<(float x, float y)>> toFloatCoordinates)
	{
		LSystem = lSystem;
		ToCoordinates = toCoordinates;
		ToFloatCoordinates = toFloatCoordinates;
	}

	public abstract List<(float x, float y)> GenerateCoordinates(int iterationCount);
}

public class LSystem2DChar : LSystem2D<char>
{
	private readonly int directionCount;
	private readonly bool useF;
	
	public LSystem2DChar(
		LSystem<char> lSystem, Func<char[], int, int, List<(int x, int y)>> toCoordinates, 
		Func<List<(int x, int y)>, float, List<(float x, float y)>> toFloatCoordinates,
		int directionCount, 
		bool useF)
		: base(lSystem, toCoordinates, toFloatCoordinates)
	{
		this.directionCount = directionCount;
		this.useF = useF;
	}

	public static char[] RelativeToAbsoluteCommands(char[] input, int directionCount, bool useF)
	{
		List<char> absoluteCommands = new List<char>();
		int direction = 0;

		foreach (char command in input)
		{
			switch (command)
			{
				case 'F' when useF:
					absoluteCommands.Add(direction.ToString()[0]);
					break;
				case '+':
					direction = (direction + 1) % directionCount;
					break;
				case '-':
					direction = (direction + directionCount - 1) % directionCount;
					break;
				case 'A' when !useF:
				case 'B' when !useF:
					absoluteCommands.Add(direction.ToString()[0]);
					break;
			}
		}

		return absoluteCommands.ToArray();
	}

	public override List<(float x, float y)> GenerateCoordinates(int iterationCount)
	{
		char[] charList = LSystem.Axiom.Substitute(LSystem.Rules, iterationCount).ToArray();
		char[] str = RelativeToAbsoluteCommands(charList, directionCount, useF);
		var coordinates = ToCoordinates(str, 0, 0);
		var floatCoordinates = ToFloatCoordinates(coordinates, 10);

		return floatCoordinates;
	}
}

public class LSystem
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
