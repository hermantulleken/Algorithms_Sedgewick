using System.Collections;
using System.Reflection;
using System.Text;

namespace Support;

public static class Formatter
{
	private const string NullString = "null";
	private const string NameMissing = "???";
	private const string CommaSpace = ", ";
	private const string IndentString = "\t";
	
	/// <summary>
	/// <see cref="BindingFlags"/> that represent public instance fields and get-properties.
	/// </summary>
	public const BindingFlags PublicInstances = BindingFlags.Public |
	                                            BindingFlags.GetField |
	                                            BindingFlags.GetProperty |
	                                            BindingFlags.Instance;

	/// <summary>
	/// <see cref="BindingFlags"/> that represent all instance fields and get-properties.
	/// </summary>
	public const BindingFlags AllInstances = PublicInstances | BindingFlags.NonPublic;
	
	// An empty dictionary of functions used to write types.
	public static readonly IDictionary<Type, Func<object, string>> EmptyTypeWriters = new Dictionary<Type, Func<object, string>>();
	
	public static string ListVariable(IEnumerable list, string name = null)
	{
		string printName = string.IsNullOrEmpty(name) ? NameMissing : name;
		return FormatKeyValue(printName, List(list));
	}
	
	public static string ObjectDetailVariable<T>(
		T obj,
		string name = null,
		BindingFlags bindingFlags = PublicInstances,
		IDictionary<Type, Func<object, string>> typeWriters = null)
	{
		typeWriters ??= EmptyTypeWriters;

		string printName = string.IsNullOrEmpty(name) ? NameMissing : name;
		string objText = (IsNull(obj) ? NullString : List(obj));
		var type = IsNull(obj) ? typeof(T) : obj.GetType();

		var builder = new StringBuilder()
			.AppendLine($"{printName} ({type.FullName}) = {objText}");

		AppendObjectDetail(builder, obj, bindingFlags, typeWriters, type);

		return builder.ToString();
	}
	
	public static string ObjectDetail<T>(
		T obj,
		string name = null,
		BindingFlags bindingFlags = PublicInstances,
		IDictionary<Type, Func<object, string>> typeWriters = null)
	{
		typeWriters ??= EmptyTypeWriters;
		var type = IsNull(obj) ? typeof(T) : obj.GetType();
		var builder = new StringBuilder();

		builder.AppendLine("{");
		AppendObjectDetail(builder, obj, bindingFlags, typeWriters, type);
		builder.AppendLine("}");
		
		return builder.ToString();
	}

	private static void AppendObjectDetail<T>(StringBuilder builder, T obj, BindingFlags bindingFlags, IDictionary<Type, Func<object, string>> typeWriters, Type type)
	{
		if (!IsNull(obj))
		{
			foreach (var fieldInfo in type.GetFields(bindingFlags))
			{
				string value = FieldToString(fieldInfo, obj, typeWriters);

				builder.Indent()
					.AppendKeyValueLine(fieldInfo.Name, value);
			}

			foreach (var propertyInfo in type.GetProperties(bindingFlags).Where(p => p.CanRead)) //CanRead means has a getter (can be public or non-public)
			{
				string value;

				//Accessing properties, unlike fields, can raise exceptions. Catch and include in string.
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

	private static string FieldToString(FieldInfo fieldInfo, object obj, IDictionary<Type, Func<object, string>> typeWriters)
	{
		object value = fieldInfo.GetValue(obj);

		if (!typeWriters.ContainsKey(fieldInfo.FieldType))
		{
			return List(value);
		}

		if (value == null)
		{
			return NullString;
		}

		return typeWriters[fieldInfo.FieldType](value);
	}

	// This is a method so we can suppress the warning in one place. 
	// ReSharper disable once CompareNonConstrainedGenericWithNull
	private static bool IsNull<T>(T obj) => obj == null;

	private static string PropertyToString(PropertyInfo propertyInfo, object obj, IDictionary<Type, Func<object, string>> typeWriters)
	{
		var indexParameters = propertyInfo.GetIndexParameters();

		if (indexParameters.Any())
		{
			return "[]";
		}

		var value = propertyInfo.GetValue(obj, null);

		if (!typeWriters.ContainsKey(propertyInfo.PropertyType))
		{
			return List(value); //null because it is not an index property
		}

		if (value == null)
		{
			return NullString;
		}

		return typeWriters[propertyInfo.PropertyType](value);
	}
	
	private static string FormatKeyValue(string key, string value)
		=> key + " = " + value;
	
	/// <summary>
	/// Converts lists to strings recursively (and other objects using their <see cref="object.ToString"/> methods.)
	/// </summary>
	public static string List<T>(T obj)
	{
		if (IsNull(obj))
		{
			return NullString;
		}

		const string brackets = "[{0}]";
		const string braces = "{{{0}}}";

		switch (obj)
		{
			case string:
				return (string)(object)obj;
			//also works for dictionaries
			case IEnumerable list:
			{
				string values = ToString(list);

				return Wrap(values, (obj is IDictionary) ?  braces : brackets);
			}
			default:
				try
				{
					string objAsString = obj.ToString();
					return objAsString;
				}
				catch (Exception e)
				{
					return ExceptionIn(e, "ToString()");
				}
		}
	}

	private static string ExceptionIn(Exception exception, string exceptionSourceName)
		=> $"Exception {exception} raised by {exceptionSourceName}";

	public static string ToString<T>(IEnumerable<T> list) where T : class
	{
		var stringList = list.Select(item => item == null ? NullString : item.ToString());

		return string.Join(CommaSpace, stringList.ToArray());
	}
	public static string ToString(IEnumerable list)
	{
		var stringList = list.Cast<object>().Select(item => item == null ? NullString : item.ToString());

		return string.Join(CommaSpace, stringList.ToArray());
	}
	
	private static string Wrap(string str, string bracketsFormatString)
		=> string.Format(bracketsFormatString, str);
	
	// A fluent extension method for adding indents to a StringBuilder
	private static StringBuilder Indent(this StringBuilder builder, int indentLevel = 1)
	{
		for (int i = 0; i < indentLevel; i++)
		{
			builder.Append(IndentString);
		}

		return builder;
	}
	
	//We may use this return type if the calling implementation changes or more
	//methods that use it is added. This also makes it consistent with other StringBuilder extension methods.
	// ReSharper disable once UnusedMethodReturnValue.Local
	private static StringBuilder AppendKeyValueLine<T>(this StringBuilder builder, string key, T value)
		=> builder.AppendLine(KeyValueToString(key, value));

	internal static string KeyValueToString<TKey, TValue>(TKey key, TValue value) 
		=> FormatKeyValue(List(key), List(value));
}
