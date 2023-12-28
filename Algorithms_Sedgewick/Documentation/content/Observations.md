# Observations on implementing algorithms

## 1. IComparer vs. IComparable
- It is better  to design a system of containers to use comparers than to make them take IComparables.
- For a single container, using IComparable may be easier.
- But using comparers makes it a lot easier to change the behaviour of a container by changing the comparer instead of 
the contents. For example:
    - You can make a min data structure into a max structure by inverting the comparer.
    - You can make a weighted graph unweighted by changing the comparer.
- I could easily made comparers optional by using Comparer<T>.Default, but I decided against it because it is forces me
to always make them configurable. (If this was a real library, I would make them optional.)

## 2. Generic math
- Similarly, there is a choice between implementing certain features using generic math or using `Func` to supply the 
necessary operations. For example, weights in graphs can be implemented as either a IFloatingPoint type or as a general 
type with `IComparer` and `Func<TWeight, TWeight, TWeight>` for addition where required. I opted to use the latter, partly
to be consistent, but also because it allows for more flexibility (reinterpreting weights, for example).

## 3.Graph APIs
- The Graph APIs used here are not very good.
- The textbook prefer to keep algorithms in classes; the idea is the bulk of the algo is a preprocessing step that 
  results in a structure you can use to make quick queries (which does make sense).
- The problem is that it is very inflexible, and algorithms do not play well with each other. It also does not abstract
away the very common search and iteration strategies, so a lot of code is repeated everywhere. Moreover, I find it 
difficult to reason about the execution time of algorithms in this way.
- Helped on by the fact that the names are weird, it is difficult to find the right algorithms.
- This approach may be good for single-purpose algorithms, but for a system of algorithms it is not a good style.
Although, I can appreciate it would be difficult to design a API that is clearer, and still allows the user to separate
once-off operations from the once that need to be performed many times.

## 4. Running Benchmarks
- Running benchmarks is much more instructive than one would imagine.
- Computers are very fast, and seeing how fast is a good reminder.
- There are often surprising results.
    - Often this indicate subtle bugs not discoverable by unit tests.
    - Other results remain a mystery.
- It is satisfying to remove some bad code and get a nice performance increase.
- It shows that optimizing code to solve artificial problems has limited value. Without a real application (that is, 
data with real distributions and volumes), a lot of the theoretical results really are meaningless.

## 5. Optimization
- Optimizing even relatively simple algorithms (such as merge sort) can be very challenging because how tricky it is to
get reliable benchmarks when dealing with small variations of an algorithm. Do you copy the code and tweak? Or do you 
make a configurable algorithm?

## 6. SupportsX
- Similar to the ReadOnly property of collections, it turns out to be convienient to have a SupportsX property for 
graphs for certain features, such as whether the graph supports parallel edges, self-loops, etc. rather than rely 
on types.

## 7. Readonly data structures
- For each container type, I find I need a read only version pretty soon. 

## 8. Pseudocode
Pseudocode is not helpful when describing algorithms.
Pseudocode often make small omissions that can take a fair amount of time to figure out. In partocular, it is not always clear
what the range of a loop is:
```python
for k from 1 to n // is n included?
    // do something
```

Compare this pseudocode for Dijkstra's k-shortest path's algorithm (from 
https://en.wikipedia.org/wiki/K_shortest_path_routing) with the actual code.

- $P = \text{empty}$
- $\text{count}_u = 0$ for all $u \in V$
- insert path $p_s = \{s\}$ into $B$ with cost $0$
- while $B$ is not empty and $\text{count}_t < K$:
  - let $p_u$ be the shortest cost path in $B$ with cost $C$
  - $B = B - \{p_u\}$, $\text{count}_u = \text{count}_u + 1$
  - if $u = t$ then $P = P \cup \{p_u\}$
  - if $\text{count}_u < K$ then
    - for each vertex $v$ adjacent to $u$:
      - let $p_v$ be a new path with cost $C + w(u, v)$ formed by concatenating edge $(u, v)$ to path $p_u$
      - insert $p_v$ into $B$

[!code-csharp[](../../AlgorithmsSW/EdgeWeightedDigraph/KShortestPaths.cs#PseudoCodeExample)]

Although it is possible to make the pseudocode more precise, I think it is better to make an implementation read as 
well as the pseudocode.

## 9. Weight types
I really regret making the type of weights in graphs generic:
- It is a lot more work to implement. 
- Double is adequate for most applications.
- It leads to unusual design choices. For example, should the add function be stored in paths? If not, as I have chosen 
to do, it leads to the point below.
- It makes the API ugly in unexpected places, for example, you need to pass in the add function in the Combine method of
paths. 

Base on these problems, the generic math approach would have been better, although using double directly is probably 
still the best option. 

There are many other ways to improve how weights are used, for example:
- Define a custom type that puts everything together
- Define an interface weights must implement (this is not ideal, since then typical types need to be wrapped)
- Define a new WeightComparer type that holds the extra data (not ideal to have the add function there)

## 10. Benchmark Input Data
- It is important to be able to generate test data quickly so that benchmarks can run quickly too. 
- Test data needs to ne verified. It has happened more than once that generation has bugs in leading to degenerate test 
cases. This skew benchmarks, and leads to a lot of looking for the issue in the algorithm rather than the data. 
