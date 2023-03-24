namespace Algorithms_Sedgewick_Tests;

using System.Linq;
using System.Numerics;
using Algorithms_Sedgewick;
using NUnit.Framework;
using Support;

[Parallelizable]
public class TestGeometricAlgorithms
{
	[Test]
	public void TestTriangle()
	{
		var points = new[]
		{
			new Vector2(0, 0),
			new Vector2(2, 2),
			new Vector2(0, 2),
		};

		var hull = GeometricAlgorithms.GrahamsScan(points);
		
		Assert.That(hull, Is.EqualTo(points));
		
		Console.WriteLine(hull.Pretty());
	}
	
	[Test]
	public void TestSquareAndCenter()
	{
		var points = new[]
		{
			// Four points arranged in a square and fifth in center.
			new Vector2(0, 0),
			new Vector2(2, 0),
			new Vector2(2, 2),
			new Vector2(0, 2),
			new Vector2(1, 1),
		};

		var expectedHull = points.Take(4);

		var hull = GeometricAlgorithms.GrahamsScan(points);
		
		Assert.That(hull, Is.EqualTo(expectedHull));
		
		Console.WriteLine(hull.Pretty());
	}
}
