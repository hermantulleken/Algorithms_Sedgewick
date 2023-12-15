namespace AlgorithmsSW.LSystem;

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
