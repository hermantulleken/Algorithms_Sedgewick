# Documentation Tips
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
```md
@AlgorithmsSW.Algorithms
```

And this works for methods (it links to all overloads of a method; you can link to a specific method, but the syntax for
this is very complex for generic methods)
```md
@AlgorithmsSW.Sort.Sort.Merge*
```
## Steps To Configure in a project
(Assuming DocFx is already installed)
1. Make a new empty project in your solution called Documentation
2. Run `docfx init` in the Documentation project folder
3. You will need to specify the path to the source code and additional markdown files. (I usually use placeholder
names that are easy to find in the files, such as `__source__` or `__docs__`, and then go and set the correct paths once
I have figured them out.)
4. Add the generated `docfx.json`, `index.md`, and `toc.yml` files to the Documentation project.
5. If you have not already specified the correct source path, edit the source path in `docfx.json`. This is 
typically `..` or `../..`.
6. If you have not already specified the correct additional markdown files path, edit path in the `toc.yml` file. 
This is typical a new folder you can create in the Documentation project, say called content, in which case the 
path to specify is `content/`.
7. Run `docfx docfx.json --serve` to build the documentation and serve it locally. Click on the link in the console to
see if the documentation was created correctly.
8. You need to add a contents toc.yml file to the contents folder to have those pages show up.
9. Make a build command to build and view the docs. Use the JavaScript Debug template, and set the the index
file in the URL field. Add building the solution and running docfx (external command) as pre-build steps.
10. Add a configuration to view the documentation. Use the JAvaScript Debug template, and select the index file in 
the URL field. 

You can also do the following:
    * Make a folder for namespace documentation etc. Typically called "overwrites", and add a overwrites section to 
your docfx.json, like this:
```json
  "build": {
    "content": [
      {
        "files": [
          "**/*.{md,yml}"
        ],
        "exclude": [
          "_site/**",
          "overwrites/**"
        ]
      },
      {
        "files": ["**/benchmarks/**/*.md"],
        "dest": "benchmarks"
      }
    ],
    "overwrite": [{
      "files": [
        "overwrites/**.md"
      ]
    }
    ],
    "resource": [
      {
        "files": [
          "images/**"
        ]
      }
    ],
...
