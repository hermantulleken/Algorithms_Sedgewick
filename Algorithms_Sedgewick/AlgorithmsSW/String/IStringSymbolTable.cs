namespace AlgorithmsSW.String;

using SymbolTable;

/// <summary>
/// A symbol table with string keys.
/// </summary>
/// <typeparam name="TValue">The type of the values in the symbol table.</typeparam>
public interface IStringSymbolTable<TValue> : ISymbolTable<string, TValue>, IStringCollection;
