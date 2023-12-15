using System.Diagnostics;
using System.Text;
using AlgorithmsSW.SearchTrees;

namespace Visualization;

public class Graphs
{
	public string GenerateDotFile<T>(BinarySearchTree<T> tree)
	{
		var builder = new StringBuilder();
		builder.AppendLine("digraph G {");

		if (!tree.IsEmpty)
		{
			GenerateDotFileHelper(builder, tree.Root);
		}
		
		builder.AppendLine("}");
		return builder.ToString();
	}

	private void GenerateDotFileHelper<T>(StringBuilder builder, BinarySearchTree<T>.Node? node)
	{
		Debug.Assert(node != null);

		if (node.LeftChild != null)
		{
			builder.AppendLine($"{node.Item} -> {node.LeftChild.Item};");
			GenerateDotFileHelper(builder, node.LeftChild);
		}

		if (node.RightChild != null)
		{
			builder.AppendLine($"{node.Item} -> {node.RightChild.Item};");
			GenerateDotFileHelper(builder, node.RightChild);
		}
	}
}
