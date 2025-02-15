using System.Collections;
using System.Reflection;

namespace Diffex;

/// <summary>
/// A class that provides functionality to compare two objects of type T and return the differences in a specified format.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TOutput">The type of the output format.</typeparam>
public class Diffex<T, TOutput>
{
    private readonly Func<List<PropertyDifference>, TOutput> _formatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="Diffex{T, TOutput}"/> class with a default formatter.
    /// </summary>
    public Diffex() => _formatter = differences => (TOutput)(object)string.Join(Environment.NewLine, differences.Select(d => d.ToString()));

    /// <summary>
    /// Initializes a new instance of the <see cref="Diffex{T, TOutput}"/> class with a specified formatter.
    /// </summary>
    /// <param name="formatter">The formatter function to format the list of property differences.</param>
    /// <exception cref="ArgumentNullException">Thrown when the formatter is null.</exception>
    public Diffex(Func<List<PropertyDifference>, TOutput> formatter) => _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

    /// <summary>
    /// Compares two objects of type T and returns the differences in the specified format.
    /// </summary>
    /// <param name="first">The first object to compare.</param>
    /// <param name="second">The second object to compare.</param>
    /// <returns>The differences between the two objects in the specified format.</returns>
    public TOutput Diff(T first, T second) => _formatter(GetDifferences(first, second));

    /// <summary>
    /// Gets the differences between two objects of type T.
    /// </summary>
    /// <param name="first">The first object to compare.</param>
    /// <param name="second">The second object to compare.</param>
    /// <param name="parentPath">The parent path for nested properties.</param>
    /// <returns>A list of property differences between the two objects.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either the first or second object is null.</exception>
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
            // Handle indexed properties (e.g., collections)
            if (property.GetIndexParameters().Length > 0)
            {
                if (IsCollection(property.DeclaringType))
                {
                    differences.AddRange(CompareCollections(first as IEnumerable, second as IEnumerable, parentPath));
                }
                continue;
            }

            object firstValue = property.GetValue(first);
            object secondValue = property.GetValue(second);
            string propertyPath = string.IsNullOrEmpty(parentPath) ? property.Name : $"{parentPath}.{property.Name}";

            if (IsEnum(property.PropertyType))
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
                    var nestedDifferences = GetDifferences((T)firstValue, (T)secondValue, propertyPath);
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

    /// <summary>
    /// Compares two collections and returns the differences.
    /// </summary>
    /// <param name="first">The first collection to compare.</param>
    /// <param name="second">The second collection to compare.</param>
    /// <param name="propertyPath">The property path for the collections.</param>
    /// <returns>A list of property differences between the two collections.</returns>
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
                var nestedDifferences = GetDifferences((T)firstList[i], (T)secondList[i], indexedPath);
                differences.AddRange(nestedDifferences);
            }
        }

        return differences;
    }

    /// <summary>
    /// Determines whether the specified type is an enumeration.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>true if the type is an enumeration; otherwise, false.</returns>
    private static bool IsEnum(Type type) => type.IsEnum;

    /// <summary>
    /// Determines whether the specified type is a struct.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>true if the type is a struct; otherwise, false.</returns>
    private static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal);

    /// <summary>
    /// Determines whether the specified type is a complex type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>true if the type is a complex type; otherwise, false.</returns>
    private static bool IsComplexType(Type type) => type.IsClass && type != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(type);

    /// <summary>
    /// Determines whether the specified type is a collection.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>true if the type is a collection; otherwise, false.</returns>
    private static bool IsCollection(Type type) => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
}
