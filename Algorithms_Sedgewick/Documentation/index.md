# Algorithms API Documentation
Based on the book by Robert Sedgewick and Kevin Wayne: "Algorithms", 4th edition. This code base is mostly for 
exploration of the topic of the book. 

- [API Reference](api/Algorithms_Sedgewick.html): Detailed descriptions of all APIs, classes, methods, and more.
- [DataStructures](xref:AlgorithmsSW.DataStructures): Provide default implementations of common data structures. 

## Documentation Tips
<!-- This is so that I can remember how to do this later. -->
You can include code from files in the repo in documentation markdown files using the following syntax, where the files
are relative to the markdown file it is in. 
```
[!code-csharp[](../AlgorithmsSW/TestAlgorithms.cs#TraceExample)]
```
These links also work in XML comments, although it is relative to the `api` folder in the Documentation project.  
```
[!code-csharp[](../../AlgorithmsSW/TestAlgorithms.cs#TraceExample)]
```
