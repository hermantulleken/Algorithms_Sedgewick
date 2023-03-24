using Algorithms_Sedgewick.Buffer;
using Algorithms_Sedgewick.Stack;

namespace Algorithms_Sedgewick;

public static class TestAlgorithms
{
	public static bool AreDelimitersBalanced(string s)
	{ 
		char Match(char openBracket)
		{
			return openBracket switch
			{
				'(' => ')',
				'[' => ']',
				'{' => '}',
				'<' => '>',
				_ => throw new ArgumentOutOfRangeException(nameof(openBracket), openBracket, null)
			};
		}	
		
		const string openingBrackets = "([{<";
		const string closingBrackets = ")]}>";

		var openDelimiters = new StackWithLinkedList<char>();

		foreach (char c in s)
		{
			if (openingBrackets.Contains(c))
			{
				openDelimiters.Push(c);
			}
			else if (closingBrackets.Contains(c))
			{
				if (openDelimiters.IsEmpty)
				{
					return false;
				}
				
				char openBracket = openDelimiters.Pop();
				char endBracket = Match(openBracket);

				if (c != endBracket)
				{
					return false;
				}
			}
		}

		return openDelimiters.IsEmpty;
	}

	public static IEnumerable<TOut> Filter<TIn, TOut>(IEnumerable<TIn> list, Func<TIn, TIn, TOut> filter)
	{
		int windowSize = 2;
		var buffer = new RingBuffer<TIn>(windowSize);
		foreach (var item in list)
		{
			buffer.Insert(item);
			if (buffer.Count >= windowSize)
			{
				yield return filter(buffer[0], buffer[1]);
			}
		}
	}

	public static IEnumerable<TOut> Filter<TIn, TOut>(IEnumerable<TIn> list, Func<TIn, TIn, TIn, TOut> filter)
	{
		int windowSize = 3;
		var buffer = new RingBuffer<TIn>(windowSize);
		foreach (var item in list)
		{
			buffer.Insert(item);
			if (buffer.Count >= windowSize)
			{
				yield return filter(buffer[0], buffer[1], buffer[2]);
			}
		}
	}

	public static IEnumerable<TOut> Filter<TIn, TOut>(IEnumerable<TIn> list, Func<IEnumerable<TIn>, TOut> filter, int windowSize)
	{
		var buffer = new RingBuffer<TIn>(windowSize);
		foreach (var item in list)
		{
			buffer.Insert(item);
			if (buffer.Count >= windowSize)
			{
				yield return filter(buffer);
			}
		}
	}

	public static int GetFibonacci(int n) => GetGeneralizedFibonacci(n, 0, 1);

	public static int GetGeneralizedFibonacci(int n,  params int[]  initialTerms)
	{
		if (n < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(n), "Can't be negative.");
		}
		
		if (n < initialTerms.Length)
		{
			return initialTerms[n];
		}

		var buffer = new RingBuffer<int>(initialTerms.Length);

		foreach (int term in initialTerms)
		{
			buffer.Insert(term);
		}
		
		for (int i = 0; i < 1 + n - initialTerms.Length; i++)
		{
			buffer.Insert(buffer.Sum());
		}
		
		return buffer.Last;
	}

	public static int GetLucas(int n) => GetGeneralizedFibonacci(2, 1);

	public static float Median(float a, float b, float c) =>
		(a > b) ^ (a > c) 
			? a 
			: (b < a) ^ (b < c) 
				? b 
				: c;
}
