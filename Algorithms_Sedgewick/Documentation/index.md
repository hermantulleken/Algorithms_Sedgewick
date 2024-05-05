# Algorithms API Documentation
Based on the book by Robert Sedgewick and Kevin Wayne: "Algorithms", 4th edition. This code base is mostly for 
exploration of the topic of the book. 

- @AlgorithmsSW: Detailed descriptions of all APIs, classes, methods, and more.
- @AlgorithmsSW.DataStructures: Provide default implementations of common data structures.

## My purpose
I wanted to get experience with programming algorithms, including:

- Writing them as readable as possible. It annoys me how poorly written algorithms are (in textbooks, papers and 
Wikipedia).
nk readability is important at writing correct code. 
- Developing debugging techniques.
- Getting experience with using Unit tests to ensure correctness. 
- To write documentation for the algorithms (although there is a lot lacking).
- To get experience with measuring performance and optimizing algorithms. 

## Contents

(This list is incomplete; I only started my method to mark code recently, so not everything is marked yet.)

### Chapter 1
#### Page References
- Page 121: Class @AlgorithmsSW.Stack.IStack`1
- Page 121: Class @AlgorithmsSW.Queue.IQueue`1
- Page 121: Class @AlgorithmsSW.Bag.IBag`1
- Page 135: Class @AlgorithmsSW.Stack.FixedCapacityStack`1
#### Algorithms
- Algorithm 1.1: Class @AlgorithmsSW.Stack.StackWithResizeableArray`1
- Algorithm 1.2: Class @AlgorithmsSW.Stack.StackWithLinkedList`1
- Algorithm 1.3: Class @AlgorithmsSW.Queue.QueueWithLinkedList`1
- Algorithm 1.4: Class @AlgorithmsSW.Bag.BagWithLinkedList`1
- Algorithm 1.5: Class @AlgorithmsSW.UnionFind
#### Section 3
- Exercise 1.3.1: Property @AlgorithmsSW.Stack.FixedCapacityStack`1.IsFull
- Exercise 1.3.4: Method @AlgorithmsSW.Applications.AreDelimitersBalanced*
- Exercise 1.3.7: Property @AlgorithmsSW.Stack.FixedCapacityStack`1.Peek
- Exercise 1.3.14: Class @AlgorithmsSW.Queue.QueueWithResizeableArray`1
- Exercise 1.3.19: Method @AlgorithmsSW.List.ListExtensions.RemoveLast*
- Exercise 1.3.24: Method @AlgorithmsSW.List.LinkedList`1.RemoveAfter*
- Exercise 1.3.25: Method @AlgorithmsSW.List.LinkedList`1.InsertAfter*
- Exercise 1.3.30: Method @AlgorithmsSW.List.LinkedList`1.Reverse*
- Exercise 1.3.31: Class @AlgorithmsSW.List.DoublyLinkedList`1
- Exercise 1.3.32: Class @AlgorithmsSW.Steque`1
- Exercise 1.3.33: Class @AlgorithmsSW.Deque.DequeWithDoublyLinkedList`1
- Exercise 1.3.37: Method @AlgorithmsSW.Algorithms.JosephusSequence*
- Exercise 1.3.39: Class @AlgorithmsSW.Buffer.RingBuffer`1
- Exercise 1.3.44: Class @AlgorithmsSW.GapBuffer.GapBufferWithStacks`1
- Exercise 1.3.50: Method @AlgorithmsSW.Stack.FixedCapacityStack`1.GetEnumerator*
### Chapter 2
#### Page References
- Page 309: Class @AlgorithmsSW.PriorityQueue.IPriorityQueue`1
#### Algorithms
- Algorithm 2.1: Method @AlgorithmsSW.Sort.Sort.SelectionSort*
- Algorithm 2.2: Method @AlgorithmsSW.Sort.Sort.InsertionSort*
- Algorithm 2.3: Method @AlgorithmsSW.Sort.Sort.ShellSortWithPrattSequence*
- Algorithm 2.4: Method @AlgorithmsSW.Sort.Sort.MergeSort*
- Algorithm 2.5: Method @AlgorithmsSW.Sort.Sort.QuickSort*
- Algorithm 2.6: Class @AlgorithmsSW.PriorityQueue.FixedCapacityMinBinaryHeap`1
- Algorithm 2.7: Method @AlgorithmsSW.Sort.Sort.HeapSort*
#### Section 1
- Exercise 2.1.14: Method @AlgorithmsSW.Sort.Sort.DequeueSortWithDeque*
- Exercise 2.1.14: Method @AlgorithmsSW.Sort.Sort.DequeueSortWithDeque*
- Exercise 2.1.14: Method @AlgorithmsSW.Sort.Sort.DequeueSortWithQueue*
#### Section 2
- Exercise 2.2.14: Method @AlgorithmsSW.Sort.Sort.Merge*
- Exercise 2.2.15: Method @AlgorithmsSW.Sort.Sort.MergeSortBottomsUpWithQueues*
- Exercise 2.2.16: Method @AlgorithmsSW.Sort.Sort.MergeSortNatural*
- Exercise 2.2.22: Method @AlgorithmsSW.Sort.Sort.Merge3Sort*
- Exercise 2.2.22: Method @AlgorithmsSW.Sort.Sort.MergeKSort*
- Exercise 2.2.25: Method @AlgorithmsSW.Sort.Sort.MergeK*
#### Section 4
- Exercise 2.4.3: Class @AlgorithmsSW.PriorityQueue.PriorityQueueWithOrderedArray`1
- Exercise 2.4.3: Class @AlgorithmsSW.PriorityQueue.PriorityQueueWithOrderedLinkedList`1
- Exercise 2.4.3: Class @AlgorithmsSW.PriorityQueue.PriorityQueueWithUnorderedArray`1
- Exercise 2.4.3: Class @AlgorithmsSW.PriorityQueue.PriorityQueueWithUnorderedLinkedList`1
- Exercise 2.4.21: Class @AlgorithmsSW.PriorityQueue.StackWithPriorityQueue`1
- Exercise 2.4.29: Class @AlgorithmsSW.PriorityQueue.MinMaxPriorityQueue`1
- Exercise 2.4.30: Class @AlgorithmsSW.PriorityQueue.MedianDoubleHeap`1
#### Section 5
- Exercise 2.5.4: Method @AlgorithmsSW.Algorithms.SortAndRemoveDuplicates*
### Chapter 3
#### Section 1
- Exercise 3.1.2: Class @AlgorithmsSW.SymbolTable.SymbolTableWithKeyArray`2
- Exercise 3.1.2: Class @AlgorithmsSW.SymbolTable.SymbolTableWithSelfOrderingKeyArray`2
- Exercise 3.1.12: Class @AlgorithmsSW.SymbolTable.OrderedSymbolTableWithOrderedKeyArray`2
#### Section 4
- Exercise 3.4.26: Class @AlgorithmsSW.HashTable.HashTableWithLinearProbingAndLazyDelete`2
- Exercise 3.4.28: Class @AlgorithmsSW.Set.HashSet`1
- Exercise 3.4.28: Class @AlgorithmsSW.HashTable.HashTableWithLinearProbing2`2
### Chapter 4
#### Algorithms
- Algorithm 4.9: Class @AlgorithmsSW.EdgeWeightedDigraph.Dijkstra`1
#### Section 1
- Exercise 4.1.4: Method @AlgorithmsSW.Graph.GraphExtensions.ContainsEdge*
- Exercise 4.1.5: Class @AlgorithmsSW.Graph.GraphWithAdjacentsSet
- Exercise 4.1.10: Method @AlgorithmsSW.Graph.GraphAlgorithms.FindNodeSafeToDelete*
- Exercise 4.1.23: Method @AlgorithmsSW.Graph.GraphAlgorithms.DistanceHistogram*
- Exercise 4.1.26: Class @AlgorithmsSW.Graph.DepthFirstLimited
- Exercise 4.1.33: Property @AlgorithmsSW.Graph.Bipartite.HasOddCycles
- Exercise 4.1.36: Class @AlgorithmsSW.Graph.EdgeConnectivity
#### Section 2
- Exercise 4.2.3: Method @AlgorithmsSW.Digraph.DigraphWithAdjacentsLists.ContainsEdge*
- Exercise 4.2.9: Method @AlgorithmsSW.Digraph.Algorithms.IsTopologicalOrder*
- Exercise 4.2.23: Class @AlgorithmsSW.Digraph.StrongComponents
- Exercise 4.2.24: Class @AlgorithmsSW.Digraph.HamiltonianPathWithDegrees
#### Section 3
- Exercise 4.3.14: Method @AlgorithmsSW.EdgeWeightedGraph.Mst.DeleteEdge*
- Exercise 4.3.15: Method @AlgorithmsSW.EdgeWeightedGraph.Mst.AddEdge*
- Exercise 4.3.16: Method @AlgorithmsSW.EdgeWeightedGraph.Mst.FindMaxWeightThat*
- Exercise 4.3.17: Method @AlgorithmsSW.EdgeWeightedGraph.EdgeWeightedGraphWithAdjacencyLists`1.ToString*
- Exercise 4.3.22: Method @AlgorithmsSW.EdgeWeightedGraph.Mst.MstForest*
#### Section 4
- Exercise 4.4.7: Class @AlgorithmsSW.EdgeWeightedDigraph.KShortestPaths`1
- Exercise 4.4.7: Class @AlgorithmsSW.EdgeWeightedDigraph.OverlappingYensAlgorithm`1
- Exercise 4.4.7: Class @AlgorithmsSW.EdgeWeightedDigraph.YensAlgorithm`1
- Exercise 4.4.8: Class @AlgorithmsSW.EdgeWeightedDigraph.Diameter`1
- Exercise 4.4.22: Method @AlgorithmsSW.EdgeWeightedDigraph.EdgeWeightedDigraphExtensions.ToEdgeWeightedDigraph*
- Exercise 4.4.23: Class @AlgorithmsSW.EdgeWeightedDigraph.DijkstraBidirectional`1
- Exercise 4.4.23: Class @AlgorithmsSW.EdgeWeightedDigraph.DijkstraSourceSink`1
- Exercise 4.4.24: Class @AlgorithmsSW.EdgeWeightedDigraph.DijkstraMultiSource
- Exercise 4.4.25: Class @AlgorithmsSW.EdgeWeightedDigraph.DijkstraSets
- Exercise 4.4.27: Class @AlgorithmsSW.EdgeWeightedDigraph.EuclideanDistanceDigraph
- Exercise 4.4.28: Class @AlgorithmsSW.EdgeWeightedDigraph.AcyclicLongestPaths`1
- Exercise 4.4.31: Class @AlgorithmsSW.EdgeWeightedDigraph.LineGraphDistances
- Exercise 4.4.32: Class @AlgorithmsSW.EdgeWeightedDigraph.BellmanFordWithParentCheckingHeuristic`1
- Exercise 4.4.33: Method @AlgorithmsSW.Graph.GraphAlgorithms.GetFullGrid*
- Exercise 4.4.37: Class @AlgorithmsSW.EdgeWeightedDigraph.CriticalEdgesExamineIntersectingShortestPaths`1
- Exercise 4.4.37: Class @AlgorithmsSW.EdgeWeightedDigraph.ICriticalEdge`1
- Exercise 4.4.39: Class @AlgorithmsSW.EdgeWeightedDigraph.DijkstraLazy`1
### Chapter 5
#### Algorithms
- Algorithm 5.1: Method @AlgorithmsSW.String.StringSort.LeastSignificantDigitSort*
- Algorithm 5.1: Method @AlgorithmsSW.String.StringSort.LeastSignificantDigitSort*
- Algorithm 5.1: Method @AlgorithmsSW.String.StringSort.LeastSignificantDigitSort*
- Algorithm 5.2: Method @AlgorithmsSW.String.StringSort.MostSignificantDigitSort*
#### Section 1
- Exercise 5.1.1: Method @AlgorithmsSW.String.StringSort.CountSort*
- Exercise 5.1.7: Method @AlgorithmsSW.String.StringSort.CountSortWithQueues*
### Chapter 6
