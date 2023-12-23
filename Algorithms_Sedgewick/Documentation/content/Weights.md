## Weight types in edge-weighted graphs

The edge-weighted graph algorithms support arbitrary weight types, as long as your type has the following concepts:

MinValue
: A value smaller than all weights, including zero.

MaxValue
: A value larger than all weights, including zero.

Zero
: A value that, when added to a weight, does not affect the weight, and is smaller than all positive weights and bigger than all negative weights.

Add
: A function that adds two weights and returns a weight that represents their sum.

Here are the values to use for common types:

| Type     | MinValue          | MaxValue                  | Zero  | Add Function      |
|----------|-------------------|---------------------------|-------|-------------------|
| `double` | `double.MinValue` | `double.PositiveInfinity` | `0.0` | `(a, b) => a + b` |
| `int`    | `int.MinValue`    | `int.MaxValue`            | `0`   | `(a, b) => a + b` |
