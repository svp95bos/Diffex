using System.Collections;
using System.Reflection;

namespace Diffster;

public class Diffster<T, TOutput>
{
    private readonly Func<List<PropertyDifference>, TOutput> _formatter;

    public Diffster() => _formatter = differences => (TOutput)(object)string.Join(Environment.NewLine, differences.Select(d => d.ToString()));

    public Diffster(Func<List<PropertyDifference>, TOutput> formatter) => _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

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

        List<PropertyDifference> differences = [];
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
                    var nestedDiffer = new Diffster<T, TOutput>(_formatter);
                    var nestedDifferences = nestedDiffer.GetDifferences((T)firstValue, (T)secondValue, propertyPath);
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
            else if (IsComplexType(firstList[i].GetType()) || IsStruct(firstList[i].GetType()))
            {
                var nestedDiffer = new Diffster<T, TOutput>(_formatter);
                var nestedDifferences = nestedDiffer.GetDifferences((T)firstList[i], (T)secondList[i], indexedPath);
                differences.AddRange(nestedDifferences);
            }
            else if (!Equals(firstList[i], secondList[i]))
            {
                differences.Add(new PropertyDifference { PropertyName = indexedPath, FirstValue = firstList[i], SecondValue = secondList[i] });
            }
        }

        return differences;
    }

    private static bool IsEnum(Type type) => type.IsEnum;

    private static bool IsStruct(Type type) => type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal);

    private static bool IsComplexType(Type type) => type.IsClass && type != typeof(string) && !typeof(IEnumerable).IsAssignableFrom(type);

    private static bool IsCollection(Type type) => typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
}
