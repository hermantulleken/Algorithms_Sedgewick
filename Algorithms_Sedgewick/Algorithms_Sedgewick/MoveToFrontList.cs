namespace Algorithms_Sedgewick;

public class MoveToFrontList<T>
{
	private readonly DoublyLinkedList<T> list = new DoublyLinkedList<T>();

	public void Insert(T item)
	{
		var node = list.Nodes.FirstOrDefault(n => n.Item.Equals(item));

		if (node == null)
		{
			list.InsertAtFront(item);
		}
		else
		{
			list.RemoveNode(node);
			list.InsertAtFront(item);
		}
	}
}
