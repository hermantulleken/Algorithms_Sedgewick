namespace AlgorithmsSW.String;

using System.Diagnostics.CodeAnalysis;
using System.Text;
using Queue;
using static System.Diagnostics.Debug;
using static ThrowHelper;

/// <summary>
/// Represents a Trie data structure with values of type TValue.
/// </summary>
/// <typeparam name="TValue">The type of the values stored in the Trie.</typeparam>
[AlgorithmReference(5, 4)]
public class Trie<TValue>(int radix = 256) : IStringSymbolTable<TValue>
{
	static Trie() => ThrowIfNotReferenceOrNullable<TValue>();
	
	public class Node(int radix = 256) 
	{
		public TValue? Value { get; set; }
		
		public Node?[] Next { get; } = new Node[radix];

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append('[');
			
			if (Value != null)
			{
				sb.Append('(');
				sb.Append(Value);
				sb.Append("): ");
			}
			
			var nodes = Next
				.Select((node, index) => (node, index))
				.Where(pair => pair.node != null)
				.Select(pair => $"{(char)pair.index} {pair.node}");
			
			string content = string.Join(", ", nodes);
			sb.Append(content);
			sb.Append(']');
		
			return sb.ToString();
		}
	}
	
	private Node? root;

	public int Radix { get; } = radix;

	/// <inheritdoc/>
	public IComparer<string> Comparer => StringComparer.Ordinal;

	/// <inheritdoc/>
	// TODO: This is inefficient. Replace.
	public int Count => Keys.Count();

	/// <inheritdoc/>
	public IEnumerable<string> Keys => KeysWithPrefix(string.Empty);
	
	/// <inheritdoc/>
	/// <returns><see langword="false"/>.</returns>
	public bool SupportsEmptyKeys => false;

	/// <inheritdoc/>
	public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
	{
		key.ThrowIfNull();
		
		var node = Get(root, key, 0);
		
		if (node == null)
		{
			value = default;
			return false;
		}
		
		value = node.Value;
		
		Assert(value != null);
		
		return true;
	}

	/// <inheritdoc/>
	public void Add(string key, TValue value)
	{
		key.ThrowIfNull();
		value.ThrowIfNull();
		
		root = Add(root, key, value, 0);
	}

	/// <inheritdoc/>
	public void RemoveKey(string key)
	{
		key.ThrowIfNull();
		
		root = Remove(root, key, 0);
	}

	/// <inheritdoc/>
	public string? LongestPrefixOf(string str)
	{
		str.ThrowIfNull();
		
		int length = FindPrefixLength(root, str, 0, 0);
		
		// Design note: The empty string is not a key
		return length == 0 ? null : str[..length];
	}

	/// <inheritdoc/>
	public IEnumerable<string> KeysWithPrefix(string prefix)
	{
		prefix.ThrowIfNull();
		
		var queue = DataStructures.Queue<string>();
		Collect(Get(root, prefix, 0), prefix, queue);
		
		return queue;
	}

	/// <inheritdoc/>
	public IEnumerable<string> KeysThatMatch(string pattern)
	{
		pattern.ThrowIfNull();
		
		var queue = DataStructures.Queue<string>();
		Collect(root, string.Empty, pattern, queue);
		return queue;
	}
	
	// Return value associated with key in the subtrie rooted at x.
	private Node? Get(Node? node, string key, int depth)
	{
		while (true)
		{
			if (node == null)
			{
				return null;
			}

			if (depth == key.Length)
			{
				return node;
			}

			node = node.Next[key[depth++]];
		}
	}
	
	// Change value associated with key if in subtrie rooted at x.
	private Node Add(Node? node, string key, TValue value, int depth)
	{
		node ??= new();

		if (depth == key.Length)
		{
			node.Value = value; 
			
			return node;
		}
		
		char index = key[depth];
		node.Next[index] = Add(node.Next[index], key, value, depth + 1);
		return node;
	}
	
	private void Collect(Node? node, string prefix, IQueue<string> queue)
	{
		if (node == null)
		{
			return;
		}

		if (node.Value != null)
		{
			queue.Enqueue(prefix);
		}
		
		for (int i = 0; i < Radix; i++)
		{
			Collect(node.Next[i], prefix + (char)i, queue);
		}
	}
	
	private void Collect(Node? node, string prefix, string pattern, IQueue<string> queue)
	{
		int depth = prefix.Length;
		
		if (node == null)
		{
			return;
		}

		if (depth == pattern.Length && node.Value != null)
		{
			queue.Enqueue(prefix);
		}

		if (depth == pattern.Length)
		{
			return;
		}
		char next = pattern[depth];
		
		for (int i = 0; i < Radix; i++)
		{
			if (next == '.' || next == i)
			{
				Collect(node.Next[i], prefix + (char)i, pattern, queue);
			}
		}
	}

	private int FindPrefixLength(Node? node, string key, int depth, int length)
	{
		while (true)
		{
			if (node == null)
			{
				return length;
			}

			if (node.Value != null)
			{
				length = depth;
			}

			if (depth == key.Length)
			{
				return length;
			}

			node = node.Next[key[depth++]];
		}
	}
	
	private Node? Remove(Node? node, string key, int depth)
	{
		if (node == null)
		{
			return null;
		}
		
		if (depth == key.Length)
		{
			node.Value = default;
		}
		else
		{
			char index = key[depth];
			node.Next[index] = Remove(node.Next[index], key, depth + 1);
		}

		if (node.Value != null)
		{
			return node;
		}
		
		for (int i = 0; i < Radix; i++)
		{
			if (node.Next[i] != null)
			{
				return node;
			}
		}
		
		return null;
	}
	
	private string floor(Node node, string key, int depth, string sofar, string bestKey)
	{ 
		int c = key[depth];
		
		while (true)
		{
			var next = node.Next[c];

			if (next != null)
			{
				if (depth == key.Length - 1)
				{
					
				}
				
			}
		}
	}
}
