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
- Page 121: @AlgorithmsSW.Stack.IStack`1
- Page 121: @AlgorithmsSW.Queue.IQueue`1
- Page 121: @AlgorithmsSW.Bag.IBag`1
- Page 135: @AlgorithmsSW.Stack.FixedCapacityStack`1
#### Algorithms
- Algorithm 1.1: @AlgorithmsSW.Stack.StackWithResizeableArray`1
- Algorithm 1.2: @AlgorithmsSW.Stack.StackWithLinkedList`1
- Algorithm 1.3: @AlgorithmsSW.Queue.QueueWithLinkedList`1
- Algorithm 1.4: @AlgorithmsSW.Bag.BagWithLinkedList`1
- Algorithm 1.5: @AlgorithmsSW.UnionFind
#### Section 3

- Exercise 1.3.4: @AlgorithmsSW.Applications.AreDelimitersBalanced*
- Exercise 1.3.14: @AlgorithmsSW.Queue.QueueWithResizeableArray`1
### Chapter 2

#### Page References
- Page 309: @AlgorithmsSW.PriorityQueue.IPriorityQueue`1
#### Algorithms
- Algorithm 2.1: @AlgorithmsSW.Sort.Sort.SelectionSort*
- Algorithm 2.2: @AlgorithmsSW.Sort.Sort.InsertionSort*
- Algorithm 2.3: @AlgorithmsSW.Sort.Sort.ShellSortWithPrattSequence*
- Algorithm 2.4: @AlgorithmsSW.Sort.Sort.MergeSort*
- Algorithm 2.5: @AlgorithmsSW.Sort.Sort.QuickSort*
- Algorithm 2.6: @AlgorithmsSW.PriorityQueue.FixedCapacityMinBinaryHeap`1
- Algorithm 2.7: @AlgorithmsSW.Sort.Sort.HeapSort*
#### Section 2

- Exercise 2.2.14: @AlgorithmsSW.Sort.Sort.Merge*
- Exercise 2.2.15: @AlgorithmsSW.Sort.Sort.MergeSortBottomsUpWithQueues*
- Exercise 2.2.16: @AlgorithmsSW.Sort.Sort.MergeSortNatural*
- Exercise 2.2.22: @AlgorithmsSW.Sort.Sort.Merge3Sort*
- Exercise 2.2.22: @AlgorithmsSW.Sort.Sort.MergeKSort*
#### Section 4

- Exercise 2.4.3: @AlgorithmsSW.PriorityQueue.PriorityQueueWithOrderedArray`1
- Exercise 2.4.3: @AlgorithmsSW.PriorityQueue.PriorityQueueWithOrderedLinkedList`1
- Exercise 2.4.3: @AlgorithmsSW.PriorityQueue.PriorityQueueWithUnorderedArray`1
- Exercise 2.4.3: @AlgorithmsSW.PriorityQueue.PriorityQueueWithUnorderedLinkedList`1
- Exercise 2.4.21: @AlgorithmsSW.PriorityQueue.StackWithPriorityQueue`1
- Exercise 2.4.29: @AlgorithmsSW.PriorityQueue.MinMaxPriorityQueue`1
- Exercise 2.4.30: @AlgorithmsSW.PriorityQueue.MedianDoubleHeap`1
#### Section 5

- Exercise 2.5.4: @AlgorithmsSW.Algorithms.SortAndRemoveDuplicates*
### Chapter 3

#### Section 1

- Exercise 3.1.2: @AlgorithmsSW.SymbolTable.SymbolTableWithKeyArray`2
- Exercise 3.1.2: @AlgorithmsSW.SymbolTable.SymbolTableWithSelfOrderingKeyArray`2
- Exercise 3.1.12: @AlgorithmsSW.SymbolTable.OrderedSymbolTableWithOrderedKeyArray`2
#### Section 4

- Exercise 3.4.26: @AlgorithmsSW.HashTable.HashTableWithLinearProbingAndLazyDelete`2
- Exercise 3.4.28: @AlgorithmsSW.Set.HashSet`1
- Exercise 3.4.28: @AlgorithmsSW.HashTable.HashTableWithLinearProbing2`2
### Chapter 4

#### Section 1

- Exercise 4.1.4: @AlgorithmsSW.Graph.GraphExtensions.ContainsEdge*
- Exercise 4.1.5: @AlgorithmsSW.Graph.GraphWithAdjacentsSet
- Exercise 4.1.10: @AlgorithmsSW.Graph.GraphAlgorithms.FindNodeSafeToDelete*
- Exercise 4.1.23: @AlgorithmsSW.Graph.GraphAlgorithms.DistanceHistogram*
- Exercise 4.1.26: @AlgorithmsSW.Graph.DepthFirstLimited
- Exercise 4.1.36: @AlgorithmsSW.Graph.EdgeConnectivity
#### Section 2

- Exercise 4.2.3: @AlgorithmsSW.Digraph.DigraphWithAdjacentsLists.ContainsEdge*
- Exercise 4.2.9: @AlgorithmsSW.Digraph.Algorithms.IsTopologicalOrder*
- Exercise 4.2.23: @AlgorithmsSW.Digraph.StrongComponents
- Exercise 4.2.24: @AlgorithmsSW.Digraph.HamiltonianPathWithDegrees
#### Section 3

- Exercise 4.3.14: @AlgorithmsSW.EdgeWeightedGraph.Mst.DeleteEdge*
- Exercise 4.3.15: @AlgorithmsSW.EdgeWeightedGraph.Mst.AddEdge*
- Exercise 4.3.16: @AlgorithmsSW.EdgeWeightedGraph.Mst.FindMaxWeightThat*
- Exercise 4.3.17: @AlgorithmsSW.EdgeWeightedGraph.EdgeWeightedGraphWithAdjacencyLists`1.ToString*
- Exercise 4.3.22: @AlgorithmsSW.EdgeWeightedGraph.Mst.MstForest*
#### Section 4

- Exercise 4.4.7: @AlgorithmsSW.EdgeWeightedDigraph.KShortestPaths`1
- Exercise 4.4.7: @AlgorithmsSW.EdgeWeightedDigraph.YensAlgorithm`1
- Exercise 4.4.8: @AlgorithmsSW.EdgeWeightedDigraph.Diameter`1
- Exercise 4.4.22: @AlgorithmsSW.EdgeWeightedDigraph.EdgeWeightedDigraphExtensions.ToEdgeWeightedDigraph*
- Exercise 4.4.23: @AlgorithmsSW.EdgeWeightedDigraph.DijkstraSourceSink
- Exercise 4.4.24: @AlgorithmsSW.EdgeWeightedDigraph.DijkstraMultiSource
- Exercise 4.4.25: @AlgorithmsSW.EdgeWeightedDigraph.DijkstraSets
- Exercise 4.4.27: @AlgorithmsSW.EdgeWeightedDigraph.EuclideanDistanceDigraph
- Exercise 4.4.28: @AlgorithmsSW.EdgeWeightedDigraph.AcyclicLongestPaths`1
- Exercise 4.4.31: @AlgorithmsSW.EdgeWeightedDigraph.LineGraphDistances
- Exercise 4.4.32: @AlgorithmsSW.EdgeWeightedDigraph.BellmanFordWithParentCheckingHeuristic`1
- Exercise 4.4.33: @AlgorithmsSW.Graph.GraphAlgorithms.GetFullGrid*
### Chapter 5

### Chapter 6
