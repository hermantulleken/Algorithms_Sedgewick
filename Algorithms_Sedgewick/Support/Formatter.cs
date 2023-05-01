using System.Diagnostics.CodeAnalysis;

namespace Support;

using System.Collections;
using System.Reflection;
using System.Text;

public static class Formatter
{
	/// <summary>
	/// <see cref="BindingFlags"/> that represent all instance fields and get-properties.
	/// </summary>
	public const BindingFlags AllInstances = PublicInstances | BindingFlags.NonPublic;

	public const string DottedLine = ".....";

	/// <summary>
	/// <see cref="BindingFlags"/> that represent public instance fields and get-properties.
	/// </summary>
	public const BindingFlags PublicInstances = BindingFlags.Public |
	                                            BindingFlags.GetField |
	                                            BindingFlags.GetProperty |
	                                            BindingFlags.Instance;

	public const string StripedLine = "-----";
	public const string NullString = "null";
	
	private const string Braces = "{{{0}}}";
	private const string Brackets = "[{0}]";
	private const string CommaSpace = ", ";
	private const string IndentString = "\t";
	private const string NameMissing = "???";
	private const string Parentheses = "({0})";

	// An empty dictionary of functions used to write types.
	private static readonly IReadOnlyDictionary<Type, Func<object, string>> EmptyTypeWriters = new Dictionary<Type, Func<object, string>>();

	public static string ListVariable(IEnumerable list, string? name = null)
	{
		string printName = string.IsNullOrEmpty(name) ? NameMissing : name;
		return FormatKeyValue(printName, Pretty(list));
	}

	public static T Log<T>(this T obj)
	{
		Console.WriteLine(obj);
		return obj;
	}

	public static string ObjectDetail<T>(
		T obj,
		string? name = null,
		BindingFlags bindingFlags = PublicInstances,
		IReadOnlyDictionary<Type, Func<object, string>>? typeWriters = null)
	{
		string printName = string.IsNullOrEmpty(name) ? NameMissing : name;
		typeWriters ??= EmptyTypeWriters;
		var type = IsNull(obj) ? typeof(T) : obj.GetType();
		var builder = new StringBuilder();

		builder.AppendLine($"{printName} = {{");
		AppendObjectDetail(builder, obj, bindingFlags, typeWriters, type);
		builder.AppendLine("}");
		
		return builder.ToString();
	}

	public static string ObjectDetailVariable<T>(
		T obj,
		string? name = null,
		BindingFlags bindingFlags = PublicInstances,
		IReadOnlyDictionary<Type, Func<object, string>>? typeWriters = null)
	{
		typeWriters ??= EmptyTypeWriters;

		string printName = string.IsNullOrEmpty(name) ? NameMissing : name;
		string objText = IsNull(obj) ? NullString : Pretty(obj);
		var type = IsNull(obj) ? typeof(T) : obj.GetType();

		var builder = new StringBuilder()
			.AppendLine($"{printName} ({type.FullName}) = {objText}");

		AppendObjectDetail(builder, obj, bindingFlags, typeWriters, type);

		return builder.ToString();
	}

	/// <summary>
	/// Converts lists to strings recursively (and other objects using their <see cref="object.ToString"/> methods.)
	/// </summary>
	public static string Pretty<T>(this T obj, params int[] specialIndexes)
	{
		if (IsNull(obj))
		{
			return NullString;
		}

		switch (obj)
		{
			case string:
				return (string)(object)obj;

			// also works for dictionaries
			case IEnumerable list:
			{
				string values = Pretty(list, specialIndexes);

				return values.Wrap((obj is IDictionary) ? Braces : Brackets);
			}
			
			default:
				try
				{
					string objAsString = obj.ToString() ?? NullString;
					return objAsString;
				}
				catch (Exception e)
				{
					return ExceptionIn(e, "ToString()");
				}
		}
	}

    /// <summary>
    /// Converts lists to strings recursively (and other objects using their <see cref="object.ToString"/> methods.)
    /// </summary>
	public static string PrettyString(this IEnumerable<char> charList) 
		=> string.Join(string.Empty, charList);

	public static string Pretty<T>(IEnumerable<T> list, int[] specialIndexes) 
		where T : class
	{
		var stringList = list.Select((item, i) => ToString(item, specialIndexes.Contains(i)));

		return string.Join(CommaSpace, stringList.ToArray());
	}

	public static string Pretty<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
		=> FormatKeyValue(pair.ToString(), pair.Value.Pretty()).Wrap(Braces);

	public static string Pretty(IEnumerable list, int[] specialIndexes)
	{
		var stringList = list
			.Cast<object>()
			.Select((item, i) => ToString(item, specialIndexes.Contains(i)));

		return string.Join(CommaSpace, stringList.ToArray());
	}

	internal static string KeyValueToString<TKey, TValue>(TKey key, TValue value) 
		=> FormatKeyValue(Pretty(key), Pretty(value));

	// We may use this return type if the calling implementation changes or more
	// methods that use it is added. This also makes it consistent with other StringBuilder extension methods.
	// ReSharper disable once UnusedMethodReturnValue.Local
	private static StringBuilder AppendKeyValueLine<T>(this StringBuilder builder, string key, T value)
		=> builder.AppendLine(KeyValueToString(key, value));

	private static void AppendObjectDetail<T>(StringBuilder builder, T obj, BindingFlags bindingFlags, IReadOnlyDictionary<Type, Func<object, string>> typeWriters, Type type)
	{
		if (!IsNull(obj))
		{
			foreach (var fieldInfo in type.GetFields(bindingFlags))
			{
				string value = FieldToString(fieldInfo, obj, typeWriters);

				builder.Indent()
					.AppendKeyValueLine(fieldInfo.Name, value);
			}

			// CanRead means has a getter (can be public or non-public)
			foreach (var propertyInfo in type.GetProperties(bindingFlags).Where(p => p.CanRead)) 
			{
				string value;

				// Accessing properties, unlike fields, can raise exceptions. Catch and include in string.
				try
				{
					value = PropertyToString(propertyInfo, obj, typeWriters);
				}
				catch (Exception e)
				{
					value = ExceptionIn(e, propertyInfo.Name);
				}

				builder.Indent()
					.AppendKeyValueLine(propertyInfo.Name, value);
			}
		}
	}

	private static string ExceptionIn(Exception exception, string exceptionSourceName)
		=> $"Exception {exception} raised by {exceptionSourceName}";

	private static string FieldToString(FieldInfo fieldInfo, object obj, IReadOnlyDictionary<Type, Func<object, string>> typeWriters)
	{
		object? value = fieldInfo.GetValue(obj);

		if (!typeWriters.ContainsKey(fieldInfo.FieldType))
		{
			return Pretty(value);
		}

		if (value == null)
		{
			return NullString;
		}

		return typeWriters[fieldInfo.FieldType](value);
	}

	private static string FormatKeyValue(string key, string value)
		=> key + " = " + value;

	// A fluent extension method for adding indents to a StringBuilder
	private static StringBuilder Indent(this StringBuilder builder, int indentLevel = 1)
	{
		for (int i = 0; i < indentLevel; i++)
		{
			builder.Append(IndentString);
		}

		return builder;
	}

	// This is a method so we can suppress the warning in one place. 
	// ReSharper disable once CompareNonConstrainedGenericWithNull
	private static bool IsNull<T>([NotNullWhen(false)]T obj) => obj == null;

	private static string PropertyToString(PropertyInfo propertyInfo, object obj, IReadOnlyDictionary<Type, Func<object, string>> typeWriters)
	{
		var indexParameters = propertyInfo.GetIndexParameters();

		if (indexParameters.Any())
		{
			return "[]";
		}

		var value = propertyInfo.GetValue(obj, null);

		if (!typeWriters.ContainsKey(propertyInfo.PropertyType))
		{
			return Pretty(value); // null because it is not an index property
		}

		if (value == null)
		{
			return NullString;
		}

		return typeWriters[propertyInfo.PropertyType](value);
	}

	private static string ToString<T>(T? item, bool isSpecial)
	{
		string str = item != null 
			? (item.ToString() ?? NullString) 
			: NullString;

		return isSpecial ? str.Wrap(Parentheses) : str;
	}

	private static string Wrap(this string str, string bracketsFormatString)
		=> string.Format(bracketsFormatString, str);
}
