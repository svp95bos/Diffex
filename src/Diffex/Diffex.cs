using System.Collections;
using System.Reflection;

using Diffex.Abstractions;

namespace Diffex;

/// <summary>
/// A class that provides functionality to compare two objects of type T and return the differences in a specified format.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TOutput">The type of the output format.</typeparam>
public class Diffex<T, TOutput>
{
    private readonly Func<List<PropertyDifference>, TOutput> _formatter;

    public Diffex() => _formatter = differences => (TOutput)(object)string.Join(Environment.NewLine, differences.Select(d => d.ToString()));

    public Diffex(Func<List<PropertyDifference>, TOutput> formatter) => _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    public TOutput Diff(T first, T second) => _formatter(GetDifferences(first, second));

    public List<PropertyDifference> GetDifferences(T first, T second, string parentPath = "")
    {
        if (first == null)
        {
            throw new ArgumentNullException(nameof(first), "The first instance must be non-null.");
        }

        if (second == null)
        {
            throw new ArgumentNullException(nameof(second), "The second instance must be non-null.");
        }

        List<PropertyDifference> differences = new();
        Type type = typeof(T);

        if (type.IsPrimitive || type.IsValueType || type == typeof(string))
        {
            if (!Equals(first, second))
            {
                differences.Add(new PropertyDifference { PropertyName = parentPath, FirstValue = first, SecondValue = second });
            }
            return differences;
        }

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<DiffexIgnoreAttribute>() != null)
            {
                continue;
            }

            if (property.GetIndexParameters().Length > 0)
            {
                // Handle indexer properties
                for (int i = 0; ; i++)
                {
                    try
                    {
                        object firstIndexedValue = property.GetValue(first, new object[] { i });
                        object secondIndexedValue = property.GetValue(second, new object[] { i });
                        string indexedPath = $"{parentPath}[{i}]";

                        if (!Equals(firstIndexedValue, secondIndexedValue))
                        {
                            differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = firstIndexedValue, SecondValue = secondIndexedValue });
                        }
                    }
                    catch (TargetInvocationException)
                    {
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }
                continue;
            }

            object firstValue = property.GetValue(first);
            object secondValue = property.GetValue(second);
            string propertyPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";

            if (property.PropertyType == typeof(DateTime))
            {
                if (!Equals(firstValue, secondValue))
                {
                    differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
                }
            }
            else if (IsEnum(property.PropertyType))
            {
                if (!Equals(firstValue, secondValue))
                {
                    differences.Add(new PropertyDifference
                    {
                        PropertyName = propertyPath,
                        FirstValue = firstValue != null ? Enum.GetName(property.PropertyType, firstValue) : "null",
                        SecondValue = secondValue != null ? Enum.GetName(property.PropertyType, secondValue) : "null"
                    });
                }
            }
            else if (IsCollection(property.PropertyType))
            {
                differences.AddRange(CompareCollections(firstValue as IEnumerable, secondValue as IEnumerable, propertyPath));
            }
            else if (IsComplexType(property.PropertyType) || IsStruct(property.PropertyType))
            {
                if (firstValue == null || secondValue == null)
                {
                    if (!Equals(firstValue, secondValue))
                    {
                        differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
                    }
                }
                else
                {
                    var nestedDifferences = GetNestedDifferences(firstValue, secondValue, propertyPath);
                    differences.AddRange(nestedDifferences);
                }
            }
            else if (!Equals(firstValue, secondValue))
            {
                differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
            }
        }

        return differences;
    }

    private List<PropertyDifference> GetNestedDifferences(object first, object second, string parentPath)
    {
        List<PropertyDifference> differences = new();
        Type type = first.GetType();

        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<DiffexIgnoreAttribute>() != null)
            {
                continue;
            }

            object firstValue = property.GetValue(first);
            object secondValue = property.GetValue(second);
            string propertyPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";

            if (property.PropertyType == typeof(DateTime))
            {
                if (!Equals(firstValue, secondValue))
                {
                    differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
                }
            }
            else if (IsEnum(property.PropertyType))
            {
                if (!Equals(firstValue, secondValue))
                {
                    differences.Add(new PropertyDifference
                    {
                        PropertyName = propertyPath,
                        FirstValue = firstValue != null ? Enum.GetName(property.PropertyType, firstValue) : "null",
                        SecondValue = secondValue != null ? Enum.GetName(property.PropertyType, secondValue) : "null"
                    });
                }
            }
            else if (IsCollection(property.PropertyType))
            {
                differences.AddRange(CompareCollections(firstValue as IEnumerable, secondValue as IEnumerable, propertyPath));
            }
            else if (IsComplexType(property.PropertyType) || IsStruct(property.PropertyType))
            {
                if (firstValue == null || secondValue == null)
                {
                    if (!Equals(firstValue, secondValue))
                    {
                        differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
                    }
                }
                else
                {
                    var nestedDifferences = GetNestedDifferences(firstValue, secondValue, propertyPath);
                    differences.AddRange(nestedDifferences);
                }
            }
            else if (!Equals(firstValue, secondValue))
            {
                differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = firstValue, SecondValue = secondValue });
            }
        }

        return differences;
    }

    private List<PropertyDifference> CompareCollections(IEnumerable first, IEnumerable second, string propertyPath)
    {
        var differences = new List<PropertyDifference>();

        if (first == null || second == null)
        {
            if (!Equals(first, second))
            {
                differences.Add(new PropertyDifference { PropertyName = propertyPath, FirstValue = first, SecondValue = second });
            }
            return differences;
        }

        var firstList = first.Cast<object>().ToList();
        var secondList = second.Cast<object>().ToList();

        int maxLength = Math.Max(firstList.Count, secondList.Count);

        for (int i = 0; i < maxLength; i++)
        {
            string indexedPath = $"{propertyPath}[{i}]";

            if (i >= firstList.Count)
            {
                differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = null, SecondValue = secondList[i] });
            }
            else if (i >= secondList.Count)
            {
                differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = firstList[i], SecondValue = null });
            }
            else if (firstList[i].GetType().IsPrimitive || firstList[i].GetType().IsValueType || firstList[i].GetType() == typeof(string))
            {
                if (!Equals(firstList[i], secondList[i]))
                {
                    differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = firstList[i], SecondValue = secondList[i] });
                }
            }
            else if (IsComplexType(firstList[i].GetType()) || IsStruct(firstList[i].GetType()))
            {
                var nestedDifferences = GetNestedDifferences(firstList[i], secondList[i], indexedPath);
                differences.AddRange(nestedDifferences);
            }
        }

        return differences;
    }

    private static bool IsEnum(Type type) => type.IsEnum;

    private static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal);

    private static bool IsComplexType(Type type) => type.IsClass && type != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(type);

    private static bool IsCollection(Type type) => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
}
