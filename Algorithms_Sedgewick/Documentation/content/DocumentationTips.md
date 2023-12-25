﻿# Documentation Tips
<!-- This is so that I can remember how to do this later. -->
You can include code from files in the repo in documentation markdown files using the following syntax, where the files
are relative to the markdown file it is in. 
```md
[!code-csharp[](../AlgorithmsSW/TestAlgorithms.cs#TraceExample)]
```
These links also work in XML comments, although it is relative to the `api` folder in the Documentation project.  
```md
[!code-csharp[](../../AlgorithmsSW/TestAlgorithms.cs#TraceExample)]
```

You can also link to topic pages from XML comments. 
Here is how to link to a page in the content folder of the documentation project:
```md
[Weights](../content/Weights.md)
```

You can link to API documentation using this syntax:
```
@AlgorithmsSW.Algorithms
```