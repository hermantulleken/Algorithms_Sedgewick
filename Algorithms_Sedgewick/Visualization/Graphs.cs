using System.Text;
using Algorithms_Sedgewick.SearchTrees;

namespace Visualization;

public class Graphs
{
	public string GenerateDotFile<T>(BinarySearchTree<T> tree)
	{
		var builder = new StringBuilder();
		builder.AppendLine("digraph G {");
		GenerateDotFileHelper(builder, tree.Root);
		builder.AppendLine("}");
		return builder.ToString();
	}

	private void GenerateDotFileHelper<T>(StringBuilder builder, BinarySearchTree<T>.Node? node)
	{
		if (node == null)
		{
			return;
		}

		if (node.LeftChild != null)
		{
			builder.AppendLine($"{node.Id} -> {node.LeftChild.Id};");
			GenerateDotFileHelper(builder, node.LeftChild);
		}

		if (node.RightChild != null)
		{
			builder.AppendLine($"{node.Id} -> {node.RightChild.Id};");
			GenerateDotFileHelper(builder, node.RightChild);
		}
	}
}
