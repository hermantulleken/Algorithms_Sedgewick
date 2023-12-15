using System.Collections.Generic;
using AlgorithmsSW.Queue;
using NUnit.Framework;

namespace UnitTests
{
	[TestFixture]
	public class QueueTests
	{
		[TestCaseSource(nameof(QueueImplementations))]
		public void TestIsEmpty(IQueue<int> queue)
		{
			Assert.That(queue.IsEmpty, Is.True);
			queue.Enqueue(1);
			Assert.That(queue.IsEmpty, Is.False);
		}

		[TestCaseSource(nameof(QueueImplementations))]
		public void TestIsSingleton(IQueue<int> queue)
		{
			queue.Enqueue(1);
			Assert.That(queue.IsSingleton, Is.True);
			queue.Enqueue(1);
			Assert.That(queue.IsSingleton, Is.False);
		}

		[TestCaseSource(nameof(QueueImplementations))]
		public void TestEnqueueDequeue(IQueue<int> queue)
		{
			queue.Enqueue(1);
			Assert.That(queue.Dequeue, Is.EqualTo(1));
		}

		[TestCaseSource(nameof(QueueImplementations))]
		public void TestPeek(IQueue<int> queue)
		{
			queue.Enqueue(1);
			Assert.That(queue.Peek, Is.EqualTo(1));
		}

		[TestCaseSource(nameof(QueueImplementations))]
		public void TestClear(IQueue<int> queue)
		{
			queue.Enqueue(1);
			queue.Clear();
			Assert.That(queue.IsEmpty, Is.True);
		}

		private static IEnumerable<IQueue<int>> QueueImplementations()
		{
			yield return new FixedCapacityQueue<int>(10);
			yield return new QueueWithLinkedList<int>();
			yield return new QueueWithCircularLinkedList<int>();
			yield return new QueueWithLinkedListAndNodePool<int>(10);
		}
	}
}
