namespace Algorithms_Sedgewick;

using System.Diagnostics;
using System.Numerics;
using List;
using Stack;

public static class GeometricAlgorithms
{
	private readonly struct PointRelativeToAnchor : IComparable<PointRelativeToAnchor>
	{
		public readonly Vector2 Point;
		public readonly double AngleWithAnchor;
		private readonly double distanceFromAnchor;

		public PointRelativeToAnchor(Vector2 point, Vector2 anchor)
		{
			Point = point;
			var offset = point - anchor;
			AngleWithAnchor = Math.Atan2(offset.Y, offset.X);
			distanceFromAnchor = offset.Length();
		}
		
		public int CompareTo(PointRelativeToAnchor other)
		{
			double angleDifference = AngleWithAnchor - other.AngleWithAnchor;

			return angleDifference switch
			{
				< 0 => -1,
				> 0 => 1,
				_ => -distanceFromAnchor.CompareTo(other.distanceFromAnchor), // the minus sign is so we get the furthest points first.
			};
		}
	}
	
	public static IEnumerable<Vector2> GrahamsScan(IEnumerable<Vector2> points)
	{
		Vector2 LeftMostBottomMost(Vector2 point1, Vector2 point2)
		{
			if (point1.Y < point2.Y)
			{
				return point1;
			}

			if (point1.Y > point2.Y)
			{
				return point2;
			}
			
			return point1.X <= point2.X ? point1 : point2;
		}

		double Cross(Vector2 vec0, Vector2 vec1) 
			=> (vec0.Y * vec1.X) - (vec0.X * vec1.Y);

		bool IsCounterClockwise(Vector2 point0, Vector2 point1, Vector2 point2) 
			=> Cross(point1 - point0, point2 - point1) < 0; // Check this calculation

		var anchor = points.Aggregate(LeftMostBottomMost);

		PointRelativeToAnchor ToPointAngle(Vector2 point) => new(point, anchor);

		var pointAngles = points
			.Where(p => p != anchor)
			.Select(ToPointAngle)
			.ToResizableArray(points.Count());
		
		Sort.InsertionSort(pointAngles);

		if (pointAngles.IsEmpty)
		{
			throw new InvalidOperationException("All points coincide.");
		}

		var filteredList = new ResizeableArray<PointRelativeToAnchor> { pointAngles.First() };

		foreach (var pointAngle in pointAngles.Skip(1))
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			// For now we are ignoring floating point issues
			if (pointAngle.AngleWithAnchor != filteredList.Last().AngleWithAnchor)
			{
				filteredList.Add(pointAngle);
			}
		}
		
		// total number of points, including angle, less than 3
		if (filteredList.Count <= 1) 
		{
			throw new InvalidOperationException("All points are collinear.");
		}
		
		var stack = new FixedCapacityStack<Vector2>(pointAngles.Count + 1);

		Vector2 PeekNext() => stack.Skip(1).First();

		var point0 = anchor;
		var point1 = filteredList.First().Point;

		stack.Push(point0);
		stack.Push(point1);

		foreach (var pointAngle in filteredList.Skip(1))
		{
			var point2 = pointAngle.Point;
			
			while (!IsCounterClockwise(point0, point1, point2))
			{
				stack.Pop();
				point1 = stack.Peek;
				point0 = PeekNext();
				
				Debug.Assert(!stack.IsEmpty);
			}
			
			stack.Push(point2);
		}

		return stack;
	}
}
