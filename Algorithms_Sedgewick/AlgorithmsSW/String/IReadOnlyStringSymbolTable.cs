namespace AlgorithmsSW.String;

using SymbolTable;

/// <summary>
/// A read-only symbol table with string keys.
/// </summary>
/// <typeparam name="TValue">The type of the values in the symbol table.</typeparam>
public interface IReadOnlyStringSymbolTable<TValue> : IReadOnlySymbolTable<string, TValue>, IStringCollection;
