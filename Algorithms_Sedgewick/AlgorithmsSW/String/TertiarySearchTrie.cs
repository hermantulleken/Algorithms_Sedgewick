namespace AlgorithmsSW.String;

using System.Diagnostics.CodeAnalysis;
using AlgorithmsSW;
using List;
using static System.Diagnostics.Debug;

public class TertiarySearchTrie<TValue> : IStringSymbolTable<TValue>
{
	private Node? root;
	
	public class Node
	{
		public char Character;
		public Node? Left;
		public Node? Mid;
		public Node? Right;
		public TValue? Value;
		
		// ReSharper disable once StaticMemberInGenericType
		private static int nextId = 0;
		private readonly int id;
		
		public Node()
		{
			id = nextId++;
		}

		public override string ToString() 
			=> $"[{id}] C: {Character} V: {Value.AsText()} L: {Present(Left)} M: {Present(Mid)} R: {Present(Right)}";

		private static string Present(Node? obj) => obj == null ? "\u274c\ufe0f" : $"\u2714\ufe0f [{obj.id}]"; 
	}

	public IComparer<string> Comparer => StringComparer.Ordinal;
	
	// TODO : This is inefficient
	public int Count => Keys.Count();

	public IEnumerable<string> Keys
	{
		get
		{
			var list = new ResizeableArray<string>();
			CollectAll(root, string.Empty, list);
			return list;
		}
	}

	/// <inheritdoc/>
	public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
	{
		key.ThrowIfNullOrEmpty();
		
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
		key.ThrowIfNullOrEmpty();
		root = Add(root, key, value, 0);
	}

	public void RemoveKey(string key)
	{
		key.ThrowIfNullOrEmpty();
		root = Remove(root, key, 0);
	}
	
	public string? LongestPrefixOf(string str)
	{
		str.ThrowIfNullOrEmpty();
		string best = GetLongestPrefix(root, str, string.Empty, string.Empty, 0);
		
		// Design note: The empty string is not a key
		return best == string.Empty ? null : best;
	}
	
	public IEnumerable<string> KeysWithPrefix(string prefix)
	{
		prefix.ThrowIfNullOrEmpty();
		
		var list = new ResizeableArray<string>();
		Collect(root, prefix, string.Empty, 0, list);
		return list;
	}

	public IEnumerable<string> KeysThatMatch(string pattern)
	{
		pattern.ThrowIfNullOrEmpty();
		
		var list = new ResizeableArray<string>();
		CollectMatches(root, pattern, string.Empty, 0, list);
		return list;
	}

	/// <inheritdoc/>
	/// <returns><see langword="false"/>.</returns>
	public bool SupportsEmptyKeys => false;

	private Node? Get(Node? node, string key, int depth)
	{
		while (true)
		{
			if (node == null)
			{
				return null;
			}

			char @char = key[depth];
			
			if (@char < node.Character)
			{
				node = node.Left;
				continue;
			}

			if (@char > node.Character)
			{
				node = node.Right;
				continue;
			}

			if (depth < key.Length - 1)
			{
				node = node.Mid;
				depth++;
				continue;
			}

			return node;
		}
	}

	private Node Add(Node? node, string key, TValue value, int depth)
	{
		char @char = key[depth];
		
		node ??= new()
		{
			Character = @char,
		};

		if (@char < node.Character)
		{
			node.Left = Add(node.Left, key, value, depth);
		}
		else if (@char > node.Character)
		{
			node.Right = Add(node.Right, key, value, depth);
		}
		else if (depth < key.Length - 1)
		{
			node.Mid = Add(node.Mid, key, value, depth + 1);
		}
		else
		{
			node.Value = value;
		}
		
		return node;
	}
	
	private void CollectAll(Node? node, string soFar, ResizeableArray<string> list)
	{
		Console.WriteLine(node.AsText() + " " + soFar);
		
		if (node == null)
		{
			return;
		}
		
		string newValue = soFar + node.Character;
		
		if (node.Value != null)
		{
			Console.WriteLine("Adding: " + newValue);
			list.Add(newValue);
		}
		
		CollectAll(node.Left, soFar, list);
		CollectAll(node.Mid, newValue, list);
		CollectAll(node.Right, soFar, list);
	}
	
	private void Collect(Node? node, string prefix, string soFar, int depth, ResizeableArray<string> list)
	{
		Console.WriteLine(node.AsText() + " | " + soFar);
		
		if (node == null)
		{
			return;
		}
		
		string newValue = soFar + node.Character;
		
		if (depth < prefix.Length)
		{
			char @char = prefix[depth];
			
			if (@char < node.Character)
			{
				Collect(node.Left, prefix, soFar, depth, list);
			}
			else if (@char > node.Character)
			{
				Collect(node.Right, prefix, soFar, depth, list);
			}
			else
			{
				if (depth == prefix.Length - 1 && node.Value != null)
				{
					Console.WriteLine("Adding: " + newValue);
					list.Add(newValue);
				}
				
				Collect(node.Mid, prefix, soFar + node.Character, depth + 1, list);
			}
		}
		else
		{
			if (node.Value != null)
			{
				Console.WriteLine("Adding: " + newValue);
				list.Add(newValue);
			}
			
			Collect(node.Left, prefix, soFar, depth, list);
			Collect(node.Mid, prefix, newValue, depth + 1, list);
			Collect(node.Right, prefix, soFar, depth, list);
		}
	}

	private void CollectMatches(Node? node, string pattern, string soFar, int depth, ResizeableArray<string> list)
	{
		Console.WriteLine("depth : " + depth + " | " + node.AsText() + " | " + soFar);
		
		if (node == null)
		{
			return;
		}
		
		string newValue = soFar + node.Character;

		if (depth >= pattern.Length)
		{
			return;
		}

		char @char = pattern[depth];
		Console.WriteLine("Char: " + @char);
		
		if (@char == node.Character || @char == '.')
		{
			if (depth == pattern.Length - 1 && node.Value != null)
			{
				Console.WriteLine("Adding: " + newValue);
				list.Add(newValue);
			}
			else
			{
				if (@char == '.')
				{
					CollectMatches(node.Left, pattern, soFar, depth, list);
					CollectMatches(node.Right, pattern, soFar, depth, list);
				}
				
				CollectMatches(node.Mid, pattern, newValue, depth + 1, list);
			}
		}	
		else if (@char < node.Character)
		{
			CollectMatches(node.Left, pattern, soFar, depth, list);
		}
		else
		{
			Assert(@char > node.Character);
			CollectMatches(node.Right, pattern, soFar, depth, list);
		}
	}

	private string GetLongestPrefix(Node? node, string str, string soFar, string bestPrefix, int depth)
	{
		if (node == null)
		{
			return bestPrefix;
		}
		
		if (depth == str.Length)
		{
			return bestPrefix;
		}
		
		char @char = str[depth];
		
		if (@char < node.Character)
		{
			return GetLongestPrefix(node.Left, str, soFar, bestPrefix, depth);
		}

		if (@char > node.Character)
		{
			return GetLongestPrefix(node.Right, str, soFar, bestPrefix, depth);
		}

		string newValue = soFar + node.Character;
			
		if (node.Value != null)
		{
			bestPrefix = newValue;
		}
			
		return GetLongestPrefix(node.Mid, str, newValue, bestPrefix, depth + 1);
	}
	
	private Node? Remove(Node? node, string key, int depth)
	{
		if (node == null)
		{
			return null;
		}

		char @char = key[depth];

		if (@char < node.Character)
		{
			node.Left = Remove(node.Left, key, depth);
		}
		else if (@char > node.Character)
		{
			node.Right = Remove(node.Right, key, depth);
		}
		else if (depth < key.Length - 1)
		{
			node.Mid = Remove(node.Mid, key, depth + 1);
		}
		else
		{
			node.Value = default;
		}

		// If node has a value, then it's still valid; return it
		if (node.Value != null)
		{
			return node;
		}

		// If node has no children, it can be removed (by returning null)
		if (node.Left == null && node.Mid == null && node.Right == null)
		{
			return null;
		}

		// If node has children, it must remain to preserve the structure for the remaining keys
		return node;
	}
}
