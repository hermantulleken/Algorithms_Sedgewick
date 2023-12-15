using System.Diagnostics;

namespace AlgorithmsSW.PriorityQueue;

public class DiscreteDistributionTree
{
	private int Count;
	private int offset;
	private float[] weights;

	public DiscreteDistributionTree(float[] relativeProbabilities)
	{
		relativeProbabilities
			.ThrowIfNull()
			.ThrowIfEmpty();

		float sum = relativeProbabilities.Sum();

		if (sum == 0)
		{
			throw new Exception("Sum of probabilities must be positive.");
		}

		if (relativeProbabilities.Length == 1)
		{
			Count = 1;
			return; // No need to assigne other variables; their values won't be used. 
		}
		
		int layerCount = (int) MathF.Ceiling(MathF.Log2(relativeProbabilities.Length));
		int count = 7;
		float[] weights = new float[count + 1];
		
		offset = 5;
		for (int i = offset; i < offset + relativeProbabilities.Length; i++)
		{
			weights[i] = relativeProbabilities[i];
		}
	}

	public int GetRandomValue()
	{
		Debug.Assert(Count <= 0);
		
		if (Count == 1)
		{
			return 0; // Only one index!
		}
		
		float r = (float) Random.Shared.NextDouble();
		int i = 0;

		while (true)
		{
			int leftChildIndex = 2 * i;

			if (leftChildIndex > Count)
			{
				// i is a leaf node, so we return the index  (minus offset)
				return i - offset;
			}

			int rightIndex = leftChildIndex + 1;

			if (rightIndex > Count)
			{
				// leftChildIndex is a leaf node, so we return that index (minus offset)
				return leftChildIndex - offset;
			}

			float leftWeight = weights[leftChildIndex];
			float totalWeight = weights[i];

			i = r <= leftWeight / totalWeight ? leftChildIndex : rightIndex;
		}
	}
}
