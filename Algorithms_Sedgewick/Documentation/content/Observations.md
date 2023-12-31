# Observations on implementing algorithms

## IComparer vs. IComparable
- It is better  to design a system of containers to use comparers than to make them take IComparables.
- For a single container, using IComparable may be easier.
- But using comparers makes it a lot easier to change the behaviour of a container by changing the comparer instead of 
the contents. For example:
    - You can make a min data structure into a max structure by inverting the comparer.
    - You can make a weighted graph unweighted by changing the comparer.
- I could easily made comparers optional by using Comparer<T>.Default, but I decided against it because it is forces me
to always make them configurable. (If this was a real library, I would make them optional.)

## Generic math
- Similarly, there is a choice between implementing certain features using generic math or using `Func` to supply the 
necessary operations. For example, weights in graphs can be implemented as either a IFloatingPoint type or as a general 
type with `IComparer` and `Func<TWeight, TWeight, TWeight>` for addition where required. I opted to use the latter, partly
to be consistent, but also because it allows for more flexibility (reinterpreting weights, for example).

## Sort methods
Sorting parts of lists are very useful, so in general provide a method to sort a list between specified indexes, and 
then use that to implement the method to implement the full list.

## Symbol Tables and Sets
- It is annoying that sets and symbol tables cannot share their implementation in some way without overhead. The central
question here is realy whether to implement a set of KEyValuePairs, or a separate Symbol table that do not construct new
objects.

## Graph APIs
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

## Running Benchmarks
- Running benchmarks is much more instructive than one would imagine.
- Computers are very fast, and seeing how fast is a good reminder.
- There are often surprising results.
    - Often this indicate subtle bugs not discoverable by unit tests.
    - Other results remain a mystery.
- It is satisfying to remove some bad code and get a nice performance increase.
- It shows that optimizing code to solve artificial problems has limited value. Without a real application (that is, 
data with real distributions and volumes), a lot of the theoretical results really are meaningless.

## Benchmark mysteries
- For small lists, shell or insert is faster than merge sort. However, a merge sort that uses insertion sort for small
lists is as part of its implementation does not get a speed benefit. I suspect this is probably a bug, however, I have
thoroughly tested the code and have not uncovered any bugs.

## Optimization
- Optimizing even relatively simple algorithms (such as merge sort) can be very challenging because how tricky it is to
get reliable benchmarks when dealing with small variations of an algorithm. Do you copy the code and tweak? Or do you 
make a configurable algorithm?

## Unsafe Containers
Usually, you want to prevent users from invalidating your containers, and therefore you do not expose operations that
allow them to do that. However, I often found that when using containers to implement other containers, 
a unsafe container can be useful to get a more efficient implementation. For example, exposing LinkedList nodes
allows the user to mess with the links and break the list; but it makes it a lot cleaner to implement 
other containers on top of it. Although I have not done it in the code, sharing the resize functionality would
similarly be useful (to implement for example stacks and queues).

## SupportsX
- Similar to the ReadOnly property of collections, it turns out to be convienient to have a SupportsX property for 
graphs for certain features, such as whether the graph supports parallel edges, self-loops, etc. rather than rely 
on types.

## Readonly data structures
- For each container type, I find I need a read only version pretty soon. 

## Pseudocode
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

## Weight types
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

## Benchmark Input Data
- It is important to be able to generate test data quickly so that benchmarks can run quickly too. 
- Test data needs to ne verified. It has happened more than once that generation has bugs in leading to degenerate test 
cases. This skew benchmarks, and leads to a lot of looking for the issue in the algorithm rather than the data. 

## Adapting algorithms from other sources
There is almost always things you do not like about an algorithm implemented by someone else: maybe they start their 
indexes at 1 or they use arrays instead of `IEnumerable`s. 

However, it is important to not adapt and refactor at the same time; even small refactorings can introduce bugs, and
you will waste a lot of time debugging wondering whether the original code is faulty or whether it's because of your
changes.

Here is my recommended procedure for adapting algorithms to your own system:

1. If it is implemented in another language, and it is reasonably fast to get it up and running, run tests in the 
algorithm without modification. Generate some tests, and make them easy to compare to similar tests in your target
language (once you have them).

2. Now convert it to your target language, making as few modification as possible. Do not change the types of data 
structures used, or small math details. For example, do not change if(x < y) to if(y >= x). Now replicate the tests you 
made in 1, and compare the results. 

3. Now create proper unit tests for the algorithm, and run them against the translated code. For any tests that fail, 
test whether the same results are in the original code, so that you can see if the bug s with the translation. 

4. Once your tests pass, you can start the refactoring. Keep all the original code too; if it is defined in a class, 
define a new class. Keeping the original code allows you to examine internals to make it easier to find where the
refactored version deviates. It also allows you to run benchmarks in the next step.

5. Once all the tests pass, run a benchmark comparing the two implementations and make sure you did not introduce any
regressions. Comnfirm that the results match what you would expect from the time complexity of the algorithm. 

6. Once you have things tested properly for correctness and performance, you may consider removing the implementation. 
However, I would recommend keeping it (marking it as internal, obsolete), especially if it is a clear implementation. 
You may discover some cases where the new implementation is not correct, and it is useful to have the old one to 
compare.
