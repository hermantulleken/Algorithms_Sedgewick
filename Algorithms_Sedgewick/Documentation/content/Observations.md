# Observations

## 1. IComparer vs. IComparable
- It is better  to design a system of containers to use comparers than to make them take IComparables.
- For a single container, using IComparable may be easier.
- But using comparers makes it a lot easier to change the behaviour of a container by changing the comparer instead of the contents. For example:
    - You can make a min data structure into a max structure by inverting the comparer.
    - You can make a weighted graph unweighted by changing the comparer.
## 2.Graph APIs
- The Graph APIs used here are not very good.
- The textbook prefer to keep algorithms in classes; the idea is the bulk of the algo is a preprocessing step that results in a structure you can use to make quick queries (which does make sense).
- The problem is that it is very inflexible, and algorithms do not play well with each other. It also does not abstract away the very common search and iteration strategies, so a lot of code is repeated everywhere. Moreover, I find it difficult to reason about the execution time of algorithms in this way.
- Helped on by the fact that the names are weird, it is difficult to find the right algorithms.
- This approach may be good for single-purpose algorithms, but for a system of algorithms it is not a good style. Although, I can appreciate it would be difficult to design a API that is clearer, and still allows the user to separate once-off operations from the once that need to be performed many times.
## 3. Running Benchmarks
- Running benchmarks is much more instructive than one would imagine.
- Computers are very fast, and seeing how fast is a good reminder.
- There are often surprising results.
    - Often this indicate subtle bugs not discoverable by unit tests.
    - Other results remain a mystery.
- It is satisfying to remove some bad code and get a nice performance increase.
- It shows that optimizing code to solve artificial problems has limited value. Without a real application (that is, data with real distributions and volumes), a lot of the theoretical results really are meaningless.
## 4. Optimization
- Optimizing even relatively simple algorithms (such as merge sort) can be very challenging because how tricky it is to get reliable benchmarks when dealing with small variations of an algorithm. Do you copy the code and tweak? Or do you make a configurable algorithm?
